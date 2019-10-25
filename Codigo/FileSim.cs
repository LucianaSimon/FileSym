using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;


namespace FireSim
{
        public struct Indicadores
    {
        private float tSatisfaccion;
        private float tLectura;
        private float tEscritura;
        private float tGestionTotal;
        private float tMax;
        private float tMin;
    }


    public class FileSim
    {
        private int tprocesamiento;
        private string organizacionFisica;
        private string algoritmoBusqueda;
        private string adminEspacio;
        private int metocoAcceso;
        private ArrayList TablaOperaciones;
        private Dispositivo disp;
        private int ContadorOp;
        private Dictionary<string, Indicadores> indicadorArchivo; //para cada 
                                                                  //string NombreArchivo se tiene asociado una estructura Indicadores que almacena los resultados de la simulacion

        public FileSim()
        {
            // En el constructor de FileSim se crearia el array de operaciones (vacio)
            this.TablaOperaciones = new ArrayList();
        }

        //En la pantalla de config el simulador obtendra todos los siguientes parametros
        //para crear el disp
        public void cargaConfig(int tProc, string orgFisica, string algBusqueda, string admEspacio, int metAcceso,
                                int tLectura, int tEscritura, int tSeek, int tamBloques, int tamDispositivo)
        {

            //setters parametros fileSim
            SetTprocesamiento(tProc);
            SetOrganizacionFisica(orgFisica);
            SetAlgoritmoBusqueda(algBusqueda);
            SetAdminEspacio(admEspacio);
            SetMetocoAcceso(metAcceso);

            //Se crea el dispositivo con sus respectivos parametros
            this.disp = new Dispositivo(tLectura, tEscritura, tSeek, tamBloques, tamDispositivo);

        }

        public void CargarOperaciones(string ruta)
        {
            // Lee el archivo, se cargan las operaciones en this.TablaOperaciones y se ordena por tArribo
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(ruta))
                {
                    string linea;
                    string [] separa;

                    while ((linea = sr.ReadLine()) != null) //leo hasta que alcanzo el final del archivo
                    {
                        separa = linea.Split(';');

                        Operacion op = new Operacion(separa[0],  char.Parse(separa[1]), int.Parse(separa[2]), int.Parse(separa[3]), int.Parse(separa[4]), int.Parse(separa[5]), 0);

                        TablaOperaciones.Add(op);
                        
                        //mapa time stamp para organizar por tiempo de arribo

                        
                        indicadorArchivo.Add(separa[0],????); //Aca se generaria el mapa??? no tenems ningun indicador todavia ?
                    }

                }
            }
            catch (Exception e)
            {
                // Muestro al usuario el error
                Console.WriteLine("El archivo no pudo ser leido:");
                Console.WriteLine(e.Message);
            }
        }

        public int CantBloques()
        {
            return this.disp.GetCantBloques();
        }

        public int EstadoBloque() //que hace este metodo?
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int GetCantidadOp()
        {
            return this.TablaOperaciones.Count;
        }

        public int GetContadorOp()
        {
            return this.ContadorOp;
        }

        public Operacion GetOperacion() //devuelve la operacion actual?
        {
            return (Operacion)this.TablaOperaciones[this.ContadorOp];
        }


        public void SimularSiguienteOp()
        {
            switch(TablaOperaciones.)
            {
                case 'c':
                    Create();

            }
            //case --> CREATE(n) / WRITE(w) / READ(r) / OPEN(o) / CLOSE(c) / DELETE(d)
        }

        public void Create()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Write()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Read()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Delete()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Open()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Close()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //Una vez cargada la configuracion vamos a permitir cambiarla, porq esto significa que
        //vamos a tener que modificar al dispositivo tambien (no solo en el constructor de FileSim)!!!
        public int GetTprocesamiento()
        {
            return tprocesamiento;
        }

        public void SetTprocesamiento(int value)
        {
            tprocesamiento = value;
        }

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

        public string GetAdminEspacio()
        {
            return adminEspacio;
        }

        public void SetAdminEspacio(string value)
        {
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

    }

}

