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
        private int tamBloque;
        private int tamDispositivo;
        private int cantBloques;
        private int tProcesamiento;
        private int tamIndice;
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

        public int GetLibres(int uAdeseada, string OrgaFisica, ref Archivo arch)
        {
            int tiempo;
            
            if (OrgaFisica.Equals("contigua"))
            { //////@AYRTON DESDE LA INTERFAZ MANDA LOS NOMBRES EN MINUSCULA
                int bloquesDeseados = (int)Math.Ceiling((decimal)uAdeseada / (decimal)tamBloque);

            }
            else if (OrgaFisica.Equals("enlazada"))
            {
                int bloquesDeseados = (int)Math.Ceiling((decimal) (uAdeseada+tamIndice) / (decimal)tamBloque);
                arch.TablaDirecciones.AddRange(getDireccionBloqueLibre(bloquesDeseados)); //Obtengo Array List de los bloques libres
            }
            else if (OrgaFisica.Equals("indexado"))
            {
                int bloquesDeseados = (int)Math.Ceiling((decimal)uAdeseada / (decimal)tamBloque);
                arch.TablaDirecciones.AddRange(getDireccionBloqueLibre(bloquesDeseados)); //Actualizo direcciones
                arch.TablaDirecciones.AddRange(getDireccionBloqueLibreIndice(bloquesDeseados, arch));
            }
            return tiempo;
        }


        public ArrayList getDireccionBloqueLibreIndice( int BloquesDeseados, Archivo arch )
        {


          //  int cantI = (int)Math.Ceiling((decimal)BloquesDeseados / (decimal)tamIndice);
            //int cantBloquesI = (int)Math.Ceiling((decimal)tamBloque / (decimal)cantI);
            ArrayList bloquesLibres = new ArrayList();
            if (arch.TablaDireccionesIndice.Count == 0 )
            {
                
            }


        }

        public ArrayList getDireccionBloqueLibre ( int bloquesDeseados ) // solo valido para indexada/ enlazada
        {
            ArrayList bloquesLibres = new ArrayList(bloquesDeseados);
            int aux = 0;
            int contaTabla = 0;

            while ((aux <= bloquesDeseados) && (contaTabla < cantBloques))
            {
               if (!TablaBloques[contaTabla].estadoReserva) // este valor devuelve true si no esta reservado
                {
                    bloquesLibres[aux] = contaTabla;
                    aux++;
                }
                contaTabla++;
            }
            if ( aux < bloquesDeseados ) // No alcanzan los bloques disponibles para almacenar el archivo. Debo cortar la funcion
            {
                throw new Exception("No hay suficiente espacio de almacenamiento para el archivo solicitado");
            }
            else
            {
                for (int i = 0; i < aux; i++)
                {
                    TablaBloques[(int)bloquesLibres[i]].estadoReserva = true; ///Reservo los bloques 
                }
            }
          
            return bloquesLibres;
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
            return tamBloque;
        }

        private void SetTamBloques(int value)
        {
            tamBloque = value;
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

