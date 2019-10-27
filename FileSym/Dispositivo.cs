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
            //Calculo de la cantidad de bloques
            //ceiling ??
            int cantBloques = (tamDispositivo / tamBloques);

            //Creo el arreglo de floats para almacenar los diferentes estados de cada bloque
            //TablaBloques[numBloque] = float estado que varia entre 0 (disponible) y 1 (ocupado).
            this.TablaBloques = new Bloque[cantBloques];
            this.SetTlectura(tLectura);
            this.SetTescritura(tEscritura);
            this.SetTseek(tSeek);
            this.SetTamBloques(tamBloques);
            this.SetTamDispositivo(tamDispositivo);
            this.SetCantBloques(cantBloques);
            this.SetTprocesamiento(tProcesamiento);
        }


        public bool isBloqueReservado(int numBloque)
        {
            return this.TablaBloques[numBloque].estadoReserva;
        }

        public void reservarBloque(int numBloque, bool reserva)
        {
            this.TablaBloques[numBloque].estadoReserva = reserva;
        }

        public int estadoBloque(int numBloque)
        {
            // esto seria asi? o habria que devolver el estado de la reserva y en otro metodo
            // la ocupacion del bloque?
            return (this.TablaBloques[numBloque].uAOcupado + this.TablaBloques[numBloque].uABurocracia);
        }

        public void CambiarEstado(int numBloque, float Estado)
        {
            // Habria que cambiar este metodo
            //this.TablaBloques[numBloque] = Estado;
        }

        /*
         * Devuelve el tiempo de gestion para buscar los bloques libres necesarios basado en la
         * organizacion fisica y el tiempo de realocar.
         * Tambien modifica la tDirecciones enviada, ya sea agregando direcciones o 
         * realocando todo el archivo y modificandola por completo
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

