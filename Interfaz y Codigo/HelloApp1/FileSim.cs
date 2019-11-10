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
        private List<Operacion> TablaOperaciones;
        private List<int> ColaEspera; 
                                            //esta lista almacenaria las posiciones en la TablaOperaciones de las operaciones que 
                                            // no se pudieron realizar todavia (por ejemplo una operacion esta esperando que se cierre un archivo)
                                             
        private Dispositivo disp;
        private int ContadorOp;
        private List<Archivo> TablaArchivos;
        //c/ string NombreArchivo se tiene asociado una estructura Indicadores que almacena los resultados de la simulacion
        private List<Indicadores> indicadoresOP;
        private int tSimulacion;
       
        public FileSim(int tProc, Org orgFisica, Libres admEspacio, Acceso metAcceso,
                       int tLectura, int tEscritura, int tSeek, int tAcceso, int tamBloques, int tamDispositivo, string ruta)
        {
            //Crea las listas de operaciones, tabla de archivos y la cola de espera (vacias!)
            this.TablaOperaciones = new List<Operacion>();
            this.TablaArchivos = new List<Archivo>();
            this.ColaEspera = new List<int>();
            this.SetContadorOp(0);

            // Por cada operacion se agrega un objeto Indicadores a la lista
            this.indicadoresOP = new List<Indicadores>();

            //Se crea el dispositivo --> se le pasan los parametros configurables relacionados con disp
            this.disp = new Dispositivo(tLectura, tEscritura, tSeek, tAcceso, tamBloques, tamDispositivo, tProc);

            //setters parametros fileSim
            SetOrganizacionFisica(orgFisica);
            SetAdminEspacio(admEspacio);
            SetMetodoAcceso(metAcceso);

            //Carga las operaciones desde el archivo ingresado x usuario --> las almacena en la tabla operaciones
            CargarOperaciones(ruta);

            this.tSimulacion = 0;

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
                    opAux.IdOperacion = valores[1];
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
            Operacion nextOp = TablaOperaciones[GetContadorOp()];
            // Todos los metodos deben devolver el tiempo que tardo en ejecutarse la operacion
            switch (nextOp.IdOperacion)
            {
                case "CREATE":
                    {
                        tSimulacion += Create(nextOp.NumProceso, nextOp.Offset, nextOp.CantidadUA, nextOp.NombreArchivo);
                        break;
                    }
                case "DELETE":
                    {
                        tSimulacion += Delete(nextOp.NombreArchivo);
                        break;
                    }
                case "OPEN":
                    {
                        tSimulacion += Open(nextOp.NombreArchivo, nextOp.NumProceso);
                        break;
                    }
                case "CLOSE":
                    {
                        tSimulacion += Close(nextOp.NombreArchivo, nextOp.NumProceso);
                        break;
                    }
                case "READ":
                    {
                        try
                        {
                            tSimulacion += Read(nextOp.NombreArchivo, nextOp.NumProceso, nextOp.Offset, nextOp.CantidadUA);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e);
                        }
                        break;
                    }
                case "WRITE":
                    {
                        tSimulacion += Write(nextOp.NombreArchivo, nextOp.NumProceso, nextOp.Offset, nextOp.CantidadUA);
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

        public int Create(int idProc, int offset, int cant_uA, string name)
        {
            int tOP = 0;

            if (BuscaArch(name) == -1) //comprueba que el archivo no este creado ya
            {
                Archivo archivo = new Archivo(name, cant_uA);

                if (disp.GetLibres(cant_uA, GetOrganizacionFisica(), ref archivo))
                {
                    TablaArchivos.Add(archivo); //agregamos el nuevo archivo a la tabla 

                    // tOP = ;
                    // Se agrega a la lista de indicadores de operaciones los indicadores de esta operacion
                    // opRealizada(0,0,0,0,"CREATE");
                }
            }
            else //si el archivo ya esta creado
            {
                Console.WriteLine("Error: El archivo ya se encuentra creado!");
            }
            return tOP;
        }

        public int Write(string name, int idProc, int offset, int cant_uA)
        {
            int tOP = 0;

            int posArch = BuscaArch(name);
            // Verifico que el archivo este creado
            if (posArch != -1)
            {   // Verifico que el proceso que abrio el archivo sea el que lo quiere leer
                if (TablaArchivos[posArch].getEstado() == idProc)
                {

                }
                else
                {
                    ColaEspera.Add(GetContadorOp()); // Si no es asi, lo agrego a la cola de espera
                }
            }

            return tOP;
        }

        public int Read(string name, int idProc, int offset, int cant_uA)
        {
            // Aca solo irian calculos de tiempo no?
            int tOP = 0;

            int posArch = BuscaArch(name);

            // Verifico que el archivo este creado
            if (posArch != -1)
            {   // Verifico que el proceso que abrio el archivo sea el que lo quiere leer
                if (TablaArchivos[posArch].getEstado() == idProc)
                {
                    Archivo arch = TablaArchivos[posArch];
                    Indicadores indicador = new Indicadores();
                    int inicio = (int)Math.Ceiling((decimal)(offset) / (decimal)(disp.GetTamBloques())); 
                    int fin = (int)Math.Ceiling((decimal)(offset + cant_uA) / (decimal)(disp.GetTamBloques()));
                    // Control de cuanto se quiere leer
                    if (fin > arch.getTablaDireccion().Count)
                    {
                        throw new Exception("Se quiere leer mas de lo que hay");
                    }
                    switch (GetOrganizacionFisica())
                    {
                        case Org.Contigua:
                            {
                                break;
                            }
                        case Org.Enlazada:
                            {
                                int bloqueLeido = -1;
                                for (int i=0; i<fin; i++)
                                {
                                    // Si el bloque anterior era contiguo al actual no hay tiempo de seek
                                    if (arch.getTablaDireccion()[i] != bloqueLeido - 1)
                                    {
                                        indicador.tSatisfaccion += disp.GetTseek();
                                    }

                                    indicador.tLectoEscritura += disp.GetTlectura();
                                    bloqueLeido = arch.getTablaDireccion()[i];
                                }
                                break;
                            }
                        case Org.Indexada:
                            {
                                break;
                            }
                    }

                    indicador.tSatisfaccion += disp.GetTprocesamient();
                    indicador.tEspera = tSimulacion - TablaOperaciones[GetContadorOp()].Tarribo;
                    indicadoresOP.Add(indicador);
                    tOP = indicador.tSatisfaccion;
                }
                else // Si no es asi, lo agrego a la cola de espera
                {
                    ColaEspera.Add(GetContadorOp());
                }
            }
            return tOP;
        }

        public int Delete(string nameArchivo)
        {
            int numBloque = 0;
            int tOP = 0;
            int posArch = BuscaArch(nameArchivo);
  
            // Corroboro que el archivo se encuentre en la tabla (por nombre) y que se encuentre cerrado
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
                tOP = disp.GetTprocesamient();

            }
            else if (TablaArchivos[posArch].getEstado() != -1) // Si el archivo se encuentra abierto por algun proceso, lo agrego a la cola de espera
            {
                ColaEspera.Add(GetContadorOp());
            }

            return tOP;
        }

        public int Open(string nameArchivo, int processID)
        {
            int posArch = BuscaArch(nameArchivo);
            int tOP = 0;

            // Si el archivo se encuentra en la tabla, y si el estado es -1, lo abro cambiando el numero de estado por 
            // el numero de proceso que lo quiere abrir.
            if (posArch != -1 && TablaArchivos[posArch].getEstado() == -1)
            {
                Indicadores indicador = new Indicadores();

                indicador.tGestionTotal = disp.GetTprocesamient(); //DUDA lu: no se si esto esta bien!
                indicador.tipoOp = 'O';

                TablaArchivos[posArch].setEstado(processID);
                tOP = disp.GetTprocesamient();
                // Se agrega a la lista de indicadores de operaciones los indicadores de esta operacion
            }
            else if (TablaArchivos[posArch].getEstado() != -1) // Si el archivo ya se encuentra abierto, agrego esta operacion a la cola de espera
            {
                ColaEspera.Add(GetContadorOp());
            }

            return tOP;
        }

        public int Close(string nameArchivo, int processID)
        {
            int posArch = BuscaArch(nameArchivo);
            int tOP = 0;

            // Si el archivo se encuentra en la tabla, y si el num de proceso que quiere cerrarlo es el que lo tiene
            // abierto, lo cierro cambiando el estado a -1
            if (posArch != -1 && TablaArchivos[posArch].getEstado() == processID)
            {
                Indicadores indicador = new Indicadores();

                indicador.tGestionTotal = disp.GetTprocesamient(); //DUDA lu: no se si esto esta bien!
                indicador.tipoOp = 'C';

                TablaArchivos[posArch].setEstado(-1);

                tOP = disp.GetTprocesamient();
           
            }
            else if (TablaArchivos[posArch].getEstado() != -processID) // si el archivo lo tiene abierto otro proceso, agrego esta operacion a la cola de espera
            {
                ColaEspera.Add(GetContadorOp());
            }

            return tOP;
        }


        // Función que te busca un archivo en la Tabla por el nombre
        public int BuscaArch(string nameArchivo)
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

        public int ObtenerTmax(char tipo) //funcion para obtener el maximo dependiento el tiempo de operacion
        {
            int Tmax = 0;

            for (int i = 0; i < indicadoresOP.Count; i++)
            {
                if (indicadoresOP[i].tipoOp == tipo)
                {
                    if (indicadoresOP[i].tSatisfaccion > Tmax)
                    {
                        Tmax = indicadoresOP[i].tSatisfaccion;
                    }
                }
            }

            return Tmax;
        }

        public int ObtenerTmin(char tipo) //funcion que devuelve el minimo dependiento el tiempo de operacion
        {
            int Tmin = 0;

            for (int i = 0; i < indicadoresOP.Count; i++)
            {
                if (indicadoresOP[i].tipoOp == tipo)
                {
                    if (indicadoresOP[i].tSatisfaccion < Tmin)
                    {
                        Tmin= indicadoresOP[i].tSatisfaccion;
                    }
                }
            }

            return Tmin;
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

        public Dispositivo GetDispositivo()
        {
            return this.disp;
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

        public List<Operacion> getTablaOperaciones()
        {
            return TablaOperaciones;
        }
    }
}


public class ComparaOp : IComparer<Operacion>
{
    public int Compare(Operacion x, Operacion y)
    {
        if (x.Tarribo < y.Tarribo)
        {
            return -1;
        }
        else if (x.Tarribo == y.Tarribo)
        {
            return 0;
        }
        else
        {
            return 1; 
        }
    }
}
