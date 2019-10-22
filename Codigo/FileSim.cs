using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;


namespace FireSim
{
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

            //setters de los otros parametros

            //Se crea el dispositivo 
            this.disp = new Dispositivo(tLectura, tEscritura, tSeek, tamBloques, tamDispositivo);

        }

        public void CargarOperaciones(string ruta)
        {
            // Lee el archivo, se cargan las operaciones en this.TablaOperaciones y se ordena por tArribo
            // Se genera el mapa
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
            return (Operacion) this.TablaOperaciones[this.ContadorOp];
        }

        
        public void SimularSiguienteOp()
        {
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

