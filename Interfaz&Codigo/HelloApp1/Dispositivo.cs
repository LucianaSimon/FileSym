using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public struct Bloque
{
    public int uAOcupado;
    public int uABurocracia;
    public bool estadoReserva;
}

namespace FireSim
{

    public class Dispositivo
    {
        private int tLectura;
        private int tEscritura;
        private int tSeek;
        private int tamBloques;
        private int tamDispositivo;
        private int cantBloques;
        private int tProcesamiento;
        private Bloque[] TablaBloques;  //arreglo fijo, dispositivo no puede crecer en tamaño fisico

        public Dispositivo(int tLectura, int tEscritura, int tSeek, int tamBloques, int tamDispositivo, int tProcesamiento)
        {
            //seteamos los parametros de entrada
            this.SetTlectura(tLectura);
            this.SetTescritura(tEscritura);
            this.SetTseek(tSeek);
            this.SetTamBloques(tamBloques);
            this.SetTamDispositivo(tamDispositivo);
            this.SetCantBloques(tamDispositivo / tamBloques);
            this.SetTprocesamiento(tProcesamiento);

            //Creo el arreglo de bloques para almacenar los diferentes estados de cada bloque
            this.TablaBloques = new Bloque[this.GetCantBloques()];

        }

        public bool isBloqueReservado(int numBloque)
        {
            return this.TablaBloques[numBloque].estadoReserva;
        }

        public void CambiarEstadoReserva(int numBloque, bool reserva)
        {
            this.TablaBloques[numBloque].estadoReserva = reserva;
        }

        public int estadoBloque(int numBloque)
        {   
            //Con este numero en la interfaz se comprueba si el bloque esta completo o no, x la cantidad de uA
            return (this.TablaBloques[numBloque].uAOcupado + this.TablaBloques[numBloque].uABurocracia);
        }

        public void CambiarEstadoOcupado(int numBloque, int modificar_uA)
        {
            this.TablaBloques[numBloque].uAOcupado = modificar_uA;
        }

        public void CambiarEstadoBurocracia(int numBloque, int modificar_uA)
        {
            this.TablaBloques[numBloque].uABurocracia = modificar_uA;
        }

        /*
         * Devuelve el tiempo de gestion para buscar los bloques libres necesarios basado en la
         * organizacion fisica y el tiempo de realocar. Tambien modifica la tDirecciones enviada,
         * ya sea agregando direcciones o realocando todo el archivo y modificandola por completo.
         */

        public int GetLibres(int BloquesDeseado, string OrgaFisica, ArrayList tDirecciones)
        {
            return 1;
        }

        public int GetCantBloques()
        {
            return cantBloques;
        }

        private void SetCantBloques(int value)
        {
            cantBloques = value;
        }

        public int GetTamDispositivo()
        {
            return tamDispositivo;
        }

        private void SetTamDispositivo(int value)
        {
            tamDispositivo = value;
        }

        public int GetTlectura()
        {
            return tLectura;
        }

        private void SetTlectura(int value)
        {
            tLectura = value;
        }

        public int GetTescritura()
        {
            return tEscritura;
        }

        private void SetTescritura(int value)
        {
            tEscritura = value;
        }

        public int GetTseek()
        {
            return tSeek;
        }

        private void SetTseek(int value)
        {
            tSeek = value;
        }

        public int GetTamBloques()
        {
            return tamBloques;
        }

        private void SetTamBloques(int value)
        {
            tamBloques = value;
        }

        public int GetTprocesamient()
        {
            return tProcesamiento;
        }

        private void SetTprocesamiento(int value)
        {
            tProcesamiento = value;
        }

    }
}

