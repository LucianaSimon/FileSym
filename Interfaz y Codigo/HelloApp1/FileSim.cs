using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;


namespace FireSim
{
    public enum Libres { MapadeBits, ListadeLibres, ListadeLibresdePrincipioyCuenta, Vacio = -1 };
    public enum Acceso { Secuencial, Directo, Indexado, Vacio = -1 };
    public enum Org { Contigua, Enlazada, Indexada, Vacio = -1 };

    public class FileSim
    {
        private Org organizacionFisica;
        private Libres adminEspacio;
        private Acceso metodoAcceso;
        private ArrayList TablaOperaciones;
        private List<Operacion> ColaEspera; //DUDA @LU : tendria que ser array list como tabla de operaciones??
                                            //esta lista almacenaria todas las operaciones que no se pudieron realizar
                                            //porque en su tarribo ya 
        private Dispositivo disp;
        private int ContadorOp;
        private List<Archivo> TablaArchivos;
        private Dictionary<string, Indicadores> indicadorArchivo; 
        //c/ string NombreArchivo se tiene asociado una estructura Indicadores que almacena los resultados de la simulacion
       
        public FileSim(int tProc, Org orgFisica, Libres admEspacio, Acceso metAcceso,
                       int tLectura, int tEscritura, int tSeek, int tAcceso, int tamBloques, int tamDispositivo, string ruta)
        {
            //Crea las listas de operaciones, tabla de archivos y la cola de espera (vacias!)
            this.TablaOperaciones = new ArrayList();
            this.TablaArchivos = new List<Archivo>();
            this.ColaEspera = new List<Operacion>();
            this.SetContadorOp(0);

            //DUDA @LU : el mapa se inicializaria aca vacio tambn???
            this.indicadorArchivo = new Dictionary<string, Indicadores>();

            //Se crea el dispositivo --> se le pasan los parametros configurables relacionados con disp
            this.disp = new Dispositivo(tLectura, tEscritura, tSeek, tAcceso, tamBloques, tamDispositivo, tProc);

            //setters parametros fileSim
            SetOrganizacionFisica(orgFisica);
            SetAdminEspacio(admEspacio);
            SetMetodoAcceso(metAcceso);

            //Carga las operaciones desde el archivo ingresado x usuario --> las almacena en la tabla operaciones
            CargarOperaciones(ruta);

        }

        private void CargarOperaciones(string ruta)
        {
            // Lee el archivo, se cargan las operaciones en this.TablaOperaciones y se ordena por tArribo
            
            try
            {
                // El metodo ReadAllLines de File, cierra el archivo automaticamente.
                string[] lineas = File.ReadAllLines(ruta);
            
                Operacion opAux = new Operacion();
                foreach (var linea in lineas)
                {
                    var valores = linea.Split(';');
                    opAux.NombreArchivo = valores[0];
                    opAux.IdOperacion = char.Parse(valores[1]);
                    switch (opAux.IdOperacion.ToString())
                    {
                        case "N":
                            opAux.NombreOperacion = "CREATE";
                            break;
                        case "C": 
                            opAux.NombreOperacion = "CLOSE";
                            break;
                        case "O":
                            opAux.NombreOperacion = "OPEN";
                            break;
                        case "D":
                            opAux.NombreOperacion = "DELETE";
                            break;
                        case "W":
                            opAux.NombreOperacion = "WRITE";
                            break;
                        case "R":
                            opAux.NombreOperacion = "READ";
                            break;
                    }
                    opAux.NumProceso = Int32.Parse(valores[2]);
                    opAux.Tarribo = Int32.Parse(valores[3]);
                    opAux.Offset = Int32.Parse(valores[4]);
                    opAux.CantidadUA = Int32.Parse(valores[5]);
                    TablaOperaciones.Add(opAux);
                }

                TablaOperaciones.Sort(new ComparaOp());
                
                /*  Bloque solo de testeo de metodo Sort*/
                foreach(Operacion op in TablaOperaciones)
                {
                    Console.WriteLine(op.NombreArchivo + " " + op.IdOperacion + " " + op.NumProceso + " " +
                        op.Tarribo + " " + op.Offset + " " + op.CantidadUA);
                }
                

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }

        }

        public int CantBloques()
        {
            return this.disp.GetCantBloques();
        }

        // EstadoBloque devuelve un float de 0 a 1 indicando el estado del bloque
        // si es 0 el bloque esta vacio, si es 1 esta lleno, lo demas es el porcentaje de ocupacion del bloque
        public float EstadoBloque(int bloque) 
        {
            return disp.estadoBloque(bloque);
        }

        public void SimularSiguienteOp()
        {
            Operacion nextOp = ((Operacion)TablaOperaciones[GetContadorOp()]);
            //case --> CREATE(n) / WRITE(w) / READ(r) / OPEN(o) / CLOSE(c) / DELETE(d)
            switch (Char.ToUpper(nextOp.IdOperacion))
            {
                case 'N':
                    {
                        Create(nextOp.NumProceso, nextOp.Offset, nextOp.CantidadUA, nextOp.NombreArchivo);
                        break;
                    }
                case 'D':
                    {
                        Delete(nextOp.NombreArchivo);
                        break;
                    }
                case 'O':
                    {
                        Open(nextOp.NombreArchivo, nextOp.NumProceso);
                        break;
                    }
                case 'C':
                    {
                        Close(nextOp.NombreArchivo, nextOp.NumProceso);
                        break;
                    }
                case 'R':
                    {
                        Read();
                        break;
                    }
                case 'W':
                    {
                        Write();
                        break;
                    }

                default:
                    {
                        Console.WriteLine("Operacion Incorrecta, se descarta");
                        break;
                    }
            }
            SetContadorOp(GetContadorOp() + 1);
        }

        public void Create(int idProc, int offset, int cant_uA, string name)
        {
            if (BuscaArch(name) == -1) //comprueba que el archivo esta cerrado
            {
                Archivo archivo = new Archivo(name, cant_uA);
                Indicadores indicador = new Indicadores();

                if (disp.GetLibres(cant_uA, GetOrganizacionFisica(), ref archivo))
                {
                    TablaArchivos.Add(archivo);

                    indicador.tGestionTotal = disp.TprocesamientoBloquesLibres(this.GetAdminEspacio(), cant_uA);
                    indicador.tEscritura = 0; ///DUDA @lu: Aca me surgio la re duda de si estos tiempos estarian todos en cero......
                                              ///porque en el metodo TprocesamientoBloquesLibres estamos actualizando tiempos
                                              ///capaz lo que tendriamos que hacer es actualizar los indicadores desde la otra funcion???
                    indicador.tLectura = 0;
                    indicador.tSatisfaccion = 0;
                    indicador.tMax = 0;
                    indicador.tMin = 0;

                    this.indicadorArchivo.Add(archivo.getNombre(),indicador);


                }
            }
            else
            {
                Console.WriteLine("Error: El archivo ya se encuentra creado!");
            }
        }

        public void Write()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read()
        {
            // Aca solo irian calculos de tiempo no?
            throw new Exception("The method or operation is not implemented.");
        }

                // Función que te busca un archivo en la Tabla por el nombre
        public int BuscaArch (string nameArchivo)
        {
            int posArch = -1;

            for (int i = 0; i < TablaArchivos.Count; i++)
            {
                if (nameArchivo == TablaArchivos[i].getNombre())
                {
                    return i;
                } 
            }

            return posArch; //Si devuelve -1 es que no está en la tabla
        }

        public void Delete(string nameArchivo)
        {
            int numBloque = 0;

            int posArch = BuscaArch(nameArchivo);
  
                // Corroboro que el archivo se encuentre en la tabla (por nombre) y que el proceso que se encuentre cerrado
                if ( posArch != -1 && TablaArchivos[posArch].getEstado() == -1) //Estado -1 significa que el archivo está cerrado
                {
                    // Dejo cada bloque como libre
                    for (int j=0; j<TablaArchivos[posArch].getTablaDireccion().Count; j++)
                    {
                        numBloque = TablaArchivos[posArch].getTablaDireccion()[j];
                        disp.CambiarEstadoOcupado(numBloque, 0);
                        // DUDA: Aca habria que analizar el tipo de AdminLibres o la OrgaFisica????

                        if (organizacionFisica == Org.Contigua)
                        {
                            disp.CambiarEstadoBurocracia(numBloque, 0);
                        }
                        else
                        {
                            disp.CambiarEstadoBurocracia(numBloque, 0);
                        }

                        disp.CambiarEstadoReserva(numBloque, false);
                    }

                    // Lo quito de la tabla de archivos
                    TablaArchivos.RemoveAt(posArch);
                }
        }

        public void Open(string nameArchivo, int processID)
        {
            int posArch = BuscaArch(nameArchivo);
            // Si el archivo se encuentra en la tabla, y si el estado es -1, lo abro cambiando el numero de estado por 
            // el numero de proceso que lo quiere abrir.
            if (posArch != -1 && TablaArchivos[posArch].getEstado() == -1)
            {
                TablaArchivos[posArch].setEstado(processID);
            }
        }

        public void Close(string nameArchivo, int processID)
        {
            int posArch = BuscaArch(nameArchivo);

            // Si el archivo se encuentra en la tabla, y si el num de proceso que quiere cerrarlo es el que lo tiene
            // abierto, lo cierro cambiando el estado a -1
            if (posArch != -1 && TablaArchivos[posArch].getEstado() == processID)
            {
                TablaArchivos[posArch].setEstado(-1);
            }
        }

        /**
         * Getters y Setters
        **/

        public int GetCantidadOp()
        {
            return this.TablaOperaciones.Count;
        }

        public int GetContadorOp()
        {
            return this.ContadorOp;
        }

        public void SetContadorOp(int value)
        {
            ContadorOp = value;
        }

        public Operacion GetOperacion() //devuelve la operacion actual?
        {
            return (Operacion)this.TablaOperaciones[this.ContadorOp];
        }

        //Una vez cargada la configuracion vamos a permitir cambiarla, porq esto significa que
        //vamos a tener que modificar al dispositivo tambien (no solo en el constructor de FileSim)!!!
        // No entendi (Fede)
        public Org GetOrganizacionFisica()
        {
            return organizacionFisica;
        }

        public void SetOrganizacionFisica(Org value)
        {
            organizacionFisica = value;
        }

        public Libres GetAdminEspacio()
        {
            return adminEspacio;
        }

        public void SetAdminEspacio(Libres value)
        {
            switch(value)
            {
                case Libres.MapadeBits:
                    {
                        // Reservo un bloque al final para el mapa de bits, y lo marco lleno de uA de Burocracia
                        disp.CambiarEstadoReserva(disp.GetCantBloques() - 1, true);
                        disp.CambiarEstadoBurocracia(disp.GetCantBloques() - 1, disp.GetTamBloques());
                        break;
                    }
                case Libres.ListadeLibres:
                    {
                        // @FEDE 
                        // Aca no estaria entendiendo como se "guarda" la lista de libres
                        break;
                    }
                case Libres.ListadeLibresdePrincipioyCuenta:
                    {
                        // Reservo 2 bloques al final para la lista y lo marco lleno de uA de Burocracia
                        disp.CambiarEstadoReserva(disp.GetCantBloques() - 2, true);
                        disp.CambiarEstadoBurocracia(disp.GetCantBloques() - 2, disp.GetTamBloques());
                        disp.CambiarEstadoReserva(disp.GetCantBloques() - 2, true);
                        disp.CambiarEstadoBurocracia(disp.GetCantBloques() - 2, disp.GetTamBloques());

                        break;
                    }
            }
            adminEspacio = value;
        }

        public Acceso GetMetodoAcceso()
        {
            return metodoAcceso;
        }

        public void SetMetodoAcceso(Acceso value)
        {
            metodoAcceso = value;
        }

        public Bloque[] getTablaBloques()
        {
            return disp.getTablaBloques();
        }

        public ArrayList getTablaOperaciones()
        {
            return TablaOperaciones;
        }
    }
}


public class ComparaOp : IComparer
{
    public int Compare(object x, object y)
    {
        if (((Operacion)x).Tarribo < ((Operacion)y).Tarribo)
        {
            return -1;
        }
        else if (((Operacion)x).Tarribo == ((Operacion)y).Tarribo)
        {
            return 0;
        }
        else
        {
            return 1; 
        }
    }
}
