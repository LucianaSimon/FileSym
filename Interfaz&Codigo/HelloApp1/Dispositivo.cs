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

        public int GetLibres(int uAdeseada, string OrgaFisica, string AdminLibres, ref Archivo arch)
        {
            int tiempo = -1;
            // TODO: Falta hacer las cuentas de los tiempos para cada organizacion, para eso se usa AdminLibres
            // Fede: creo que realocar para contigua se tiene que hacer en un metodo aparte
            if (OrgaFisica.Equals("contigua"))
            { //////@AYRTON DESDE LA INTERFAZ MANDA LOS NOMBRES EN MINUSCULA
                int bloquesDeseados = (int)Math.Ceiling((decimal)uAdeseada / (decimal)tamBloque);
                arch.TablaDirecciones.AddRange(getDireccionBloqueContiguo(bloquesDeseados));
            }
            else if (OrgaFisica.Equals("enlazada"))
            {
                int bloquesDeseados = (int)Math.Ceiling((decimal) (uAdeseada+tamIndice) / (decimal)tamBloque);
                arch.TablaDirecciones.AddRange(getDireccionBloqueLibre(bloquesDeseados)); //Obtengo Array List de los bloques libres
                // Asigno el tamaño del indice a las uABurocracia de cada bloque asignado
                for (int i=0; i< bloquesDeseados; i++)
                {
                    // Obtengo la posicion del bloque
                    int posBloque = (int)arch.TablaDirecciones[i];
                    TablaBloques[posBloque].uABurocracia = tamIndice;
                }
            }
            else if (OrgaFisica.Equals("indexado"))
            {
                int bloquesDeseados = (int)Math.Ceiling((decimal)uAdeseada / (decimal)tamBloque);
                if (checkStorage(bloquesDeseados, arch.TablaDireccionesIndice))
                {
                    arch.TablaDirecciones.AddRange(getDireccionBloqueLibre(bloquesDeseados)); //Actualizo direcciones
                    arch.TablaDireccionesIndice.AddRange(getDireccionBloqueLibreIndice(bloquesDeseados, arch.TablaDireccionesIndice));
                }
                
            }
            return tiempo;
        }


        private ArrayList getDireccionBloqueLibreIndice( int BloquesDeseados, ArrayList TablaIndices )
        {
            ArrayList bloquesLibresIndices = new ArrayList();
            if (TablaIndices.Count == 0 )
            {
                /* 
                 * Para este caso hay que crear todos los bloques de indices para ese archivo, por eso
                 * se busca la cantidad de bloques necesarios, segun los deseados
                */
                
                //Se obtienen la cantidad de uA que ocupan los indices para los BloquesDeseados
                int cant_uaI = BloquesDeseados * tamIndice;
                // Se divide la cantidad anterior por el tamaño de bloque para obtener cuantos bloques
                // son necesarios para almacenar todos los indices necesarios
                int cant_bloquesI = (int)Math.Ceiling((decimal)cant_uaI / (decimal)tamBloque);

                // Se recorre desde el final al principio para dejar los indices en el final de la tabla de bloques
                //(No se, se me ocurrio a mi, sino aca se pude usar el metodo getDireccionBloqueLibre)(FEDE)
                int posBloque = GetCantBloques();
                int posIndice = 0;
                while ((posIndice < cant_bloquesI) && (posBloque >= 0))
                {
                    if (!TablaBloques[posBloque].estadoReserva)
                    {
                        bloquesLibresIndices[posIndice] = posBloque;
                    }
                    posBloque--;
                }
                if (posIndice == cant_bloquesI)
                {
                    for (int i=0; i<posIndice; i++)
                    {
                        TablaBloques[(int)bloquesLibresIndices[posIndice]].estadoReserva = true;
                        // Voy agregando "indices" al bloque indice hasta llenarlo o ya no necesitar guardar indices
                        while((TablaBloques[posIndice].uABurocracia <= tamBloque) && (cant_uaI > 0))
                        {
                            TablaBloques[posIndice].uABurocracia += tamIndice;
                            cant_uaI -= tamIndice;
                        }
                    }
                }
                else
                {
                    // Quito todos los elementos de la tabla de indices ya que no se pudo hacer la reserva
                    // despues lanzo excepcion
                    TablaIndices.Clear();
                    throw new Exception("No hay suficiente espacio de almacenamiento para el archivo solicitado");
                }
            }
            else
            {
                /*
                 * Para este caso hay que completar el ultimo bloque indice con la cantidad de indices que entren
                 * y luego crear la cantidad de bloques de indices necesarios nuevos y completarlos con la 
                 * cantidad de bloque indice restantes
                 */
                int ultimoIndice = TablaIndices.Count;
                //Se obtienen la cantidad de uA que ocupan los indices para los BloquesDeseados
                int cant_uaI = BloquesDeseados * tamIndice;
                // Compruebo si la cantidad de uA para indice que necesito entra en el ulimo indice
                // si es asi agrego cant_uaI a uABurocracia del ultimo indice y la tabla no se modifica
                if ((tamBloque - TablaBloques[(int)TablaIndices[ultimoIndice]].uABurocracia) >= cant_uaI)
                {
                    TablaBloques[(int)TablaIndices[ultimoIndice]].uABurocracia += cant_uaI;
                }
                else // Si no, tengo que agregar la diferencia y buscar un nuevo indice
                {
                    int diff = (tamBloque - TablaBloques[(int)TablaIndices[ultimoIndice]].uABurocracia);

                    // Asigno la cantidad de indices que entran en el ultimo indice
                    TablaBloques[(int)TablaIndices[ultimoIndice]].uABurocracia += diff;

                    // bloqueNecesitado en este caso es la cantidad de indices que voy a necesitar (no bloques, indices)
                    int bloqueNecesitado = (int)Math.Ceiling((decimal)(cant_uaI - diff) / (decimal)tamIndice);
                    
                    // En aux se van a guardar los nuevos bloques indices que se van a anexar a la tabla original
                    ArrayList aux = new ArrayList();

                    // Agrego la Tabla de indices original para no perderla
                    bloquesLibresIndices.AddRange(TablaIndices);
                    
                    try
                    {
                        // Agrego los nuevos bloques que necesito
                        bloquesLibresIndices.AddRange(getDireccionBloqueLibreIndice(bloqueNecesitado, aux));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e);
                    }

                }
            }

            return bloquesLibresIndices;

        }

        private ArrayList getDireccionBloqueLibre ( int bloquesDeseados ) // solo valido para indexada/ enlazada
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

        private ArrayList getDireccionBloqueContiguo(int bloquesDeseados)
        {
            ArrayList bloquesContiguos = new ArrayList();
            
            // Para verificar que existan la cantidad de bloques deseados contiguos
            int contiguos = 0;
            
            // Para recorrer la Tabla de Bloques
            int posBloque = 0;

            // Posicion donde comienzan los bloques contiguos
            int posInicial = -1;

            while ((posBloque <GetCantBloques()) && (contiguos < bloquesDeseados))
            {
                if (!TablaBloques[posBloque].estadoReserva)
                {
                    // Aumento el contador de bloques contiguos encontrados
                    contiguos++;
                    posInicial = posBloque;                
                }
                else
                {
                    // Si en algun momento se encuentra un bloque reservado sin cumplir la cantidad de bloques
                    // contiguos deseados, se vuelve a empezar de 0
                    contiguos = 0;
                    posInicial = -1;
                }
            }
            if (contiguos == bloquesDeseados)
            {
                for (int i=0; i<contiguos; i++)
                {
                    bloquesContiguos.Add(TablaBloques[posInicial + i]);
                    TablaBloques[posInicial + i].estadoReserva = true;
                }
            }
            else
            {
                throw new Exception("No hay suficiente espacio de almacenamiento para el archivo solicitado");
            }

            return bloquesContiguos;
        }
        
        private bool checkStorage(int bloquesdeseados, ArrayList TablaIndices)
        {
            int bloquesDisponibles = 0;
            int posBloque = GetCantBloques();
            int cant_uaI = 0;
            int cant_bloquesI = 0;
            if (TablaIndices.Count == 0)
            {
                //Se obtienen la cantidad de uA que ocupan los indices para los BloquesDeseados
                cant_uaI = bloquesdeseados * tamIndice;
                // Se divide la cantidad anterior por el tamaño de bloque para obtener cuantos bloques
                // son necesarios para almacenar todos los indices necesarios
                cant_bloquesI = (int)Math.Ceiling((decimal)cant_uaI / (decimal)tamBloque);
            }
            else
            {
                int ultimoIndice = TablaIndices.Count;
                //Se obtienen la cantidad de uA que ocupan los indices para los BloquesDeseados
                cant_uaI = bloquesdeseados * tamIndice;
                // Compruebo si la cantidad de uA para indice que necesito entra en el ulimo indice
                // si es asi agrego cant_uaI a uABurocracia del ultimo indice y la tabla no se modifica
                if ((tamBloque - TablaBloques[(int)TablaIndices[ultimoIndice]].uABurocracia) >= cant_uaI)
                {
                    TablaBloques[(int)TablaIndices[ultimoIndice]].uABurocracia += cant_uaI;
                }
                else // Busco la cantidad de bloques indice que necesito extra
                {
                    int diff = (tamBloque - TablaBloques[(int)TablaIndices[ultimoIndice]].uABurocracia);

                    // cant_bloquesI en este caso es la cantidad de indices que voy a necesitar (no bloques, indices)
                    cant_bloquesI = (int)Math.Ceiling((decimal)(cant_uaI - diff) / (decimal)tamIndice);
                }
            }

            while ((bloquesDisponibles < (cant_bloquesI + bloquesdeseados) && (posBloque >= 0)))
            {
                if (!TablaBloques[posBloque].estadoReserva)
                {
                    bloquesDisponibles++;
                }
            }
            if ((bloquesdeseados + cant_bloquesI) == bloquesDisponibles)
            {
                return true;
            }
            return false;
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

