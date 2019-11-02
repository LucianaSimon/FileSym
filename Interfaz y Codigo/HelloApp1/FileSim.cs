using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;


namespace FireSim
{
    public enum Libres { MapadeBits, ListadeLibres, ListadeLibresdePrincipioyCuenta };
    public class FileSim
    {
        private string organizacionFisica;
        private string algoritmoBusqueda;
        private Libres adminEspacio;
        private int metocoAcceso;
        private ArrayList TablaOperaciones;
        private Dispositivo disp;
        private int ContadorOp;
        private List<Archivo> TablaArchivos;
        private Dictionary<string, Indicadores> indicadorArchivo; //para cada 
                                                                  //string NombreArchivo se tiene asociado una estructura Indicadores que almacena los resultados de la simulacion
       
        public FileSim(int tProc, string orgFisica, string algBusqueda, Libres admEspacio, int metAcceso,
                       int tLectura, int tEscritura, int tSeek, int tAcceso, int tamBloques, int tamDispositivo, int espacioLibre, string ruta)
        {
            // En el constructor de FileSim se crearia el array de operaciones (vacio)
            this.TablaOperaciones = new ArrayList();
            this.TablaArchivos = new List<Archivo>();
            this.SetContadorOp(0);

            //Se crea el dispositivo 
            this.disp = new Dispositivo(tLectura, tEscritura, tSeek, tAcceso, tamBloques, tamDispositivo, tProc, espacioLibre);

            //setters parametros fileSim
            SetOrganizacionFisica(orgFisica);
            SetAlgoritmoBusqueda(algBusqueda);
            SetAdminEspacio(admEspacio);
            SetMetocoAcceso(metAcceso);


            CargarOperaciones(ruta);

        }

        private void CargarOperaciones(string ruta)
        {
            // Lee el archivo, se cargan las operaciones en this.TablaOperaciones y se ordena por tArribo
            // Se genera el mapa
            
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
            if (BuscaArch(name) == -1)
            {
                Archivo archivo = new Archivo(name, cant_uA);
                if (disp.GetLibres(cant_uA, GetOrganizacionFisica(), ref archivo))
                {
                    TablaArchivos.Add(archivo);
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

                        if (organizacionFisica == "contigua")
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
        public string GetOrganizacionFisica()
        {
            return organizacionFisica;
        }

        public void SetOrganizacionFisica(string value)
        {
            organizacionFisica = value;
        }

        public string GetAlgoritmoBusqueda()
        {
            return algoritmoBusqueda;
        }

        public void SetAlgoritmoBusqueda(string value)
        {
            algoritmoBusqueda = value;
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

        public int GetMetocoAcceso()
        {
            return metocoAcceso;
        }

        public void SetMetocoAcceso(int value)
        {
            metocoAcceso = value;
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
