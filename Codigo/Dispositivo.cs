using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

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
        private float[] TablaBloques;  //arreglo fijo, dispositivo no puede crecer en tamaño fisico

        public Dispositivo(int tLectura, int tEscritura, int tSeek, int tamBloques, int tamDispositivo)
        {
            //Calculo de la cantidad de bloques
            //ceiling ??
            int cantBloques = (tamDispositivo / tamBloques);

            //Creo el arreglo de floats para almacenar los diferentes estados de cada bloque
            //TablaBloques[numBloque] = float estado que varia entre 0 (disponible) y 1 (ocupado).
            this.TablaBloques = new float[cantBloques];
            this.SetTlectura(tLectura);
            this.SetTescritura(tEscritura);
            this.SetTseek(tSeek);
            this.SetTamBloques(tamBloques);
            this.SetTamDispositivo(tamDispositivo);
            this.SetCantBloques(cantBloques);
        }

        public float estadoBloque(int numBloque)
        {
            return (this.TablaBloques[numBloque]);
        }

        public void CambiarEstado(int numBloque, float Estado)
        {
            this.TablaBloques[numBloque] = Estado;
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

    }
}

