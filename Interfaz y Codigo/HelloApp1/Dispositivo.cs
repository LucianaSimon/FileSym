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
        private int tAcceso;
        private int tProcesamiento;
        private int espacioLibre; //que haria?
        private int tamBloque;
        private int tamDispositivo;
        private int tamIndice = 1; //tamanio que ocupa un indice --> burocracia
        private int cantBloques;
        private Bloque[] TablaBloques; //arreglo fijo, dispositivo no puede crecer en tamaño fisico

        public Dispositivo(int tLectura, int tEscritura, int tSeek, int tAcceso, int tamBloques, int tamDispositivo, int tProcesamiento,
                          int espacioLibre)
        {
            //seteamos los parametros de entrada
            this.SetTlectura(tLectura);
            this.SetTescritura(tEscritura);
            this.SetTseek(tSeek);
            this.SetTAcceso(tAcceso);
            this.SetTamBloques(tamBloques);
            this.SetTamDispositivo(tamDispositivo);
            this.SetCantBloques((int)Math.Truncate((decimal)tamDispositivo / (decimal)tamBloques)); //DUDA @lu: estaria bien asi?
            this.SetTprocesamiento(tProcesamiento);
            //this.SetEspacioLibre(espacioLibre); Lo vamos a usar?

            //Creo el arreglo de bloques para almacenar los diferentes estados de cada bloque
            this.TablaBloques = new Bloque[this.GetCantBloques()];

        }

        public bool isBloqueReservado(int numBloque) //comprueba el estado del bloque
        {
            return this.TablaBloques[numBloque].estadoReserva;
        }

        public void CambiarEstadoReserva(int numBloque, bool reserva)
        {
            this.TablaBloques[numBloque].estadoReserva = reserva; 
        }

        public int estadoBloque(int numBloque)
        {
            //@AYRTON Con este numero en la interfaz se comprueba si el bloque esta completo o no, x la cantidad de uA
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

        ///devuelve el t q llevaria buscar bloques libres segun el metodo de Adminitracion elegido x usuario
        /////el segundo parametro solo se usa en el segundo metodo --> en los otros pasar cero
        ///@AYRTON fijate de pasar estos parametros x interfaz escritos igual! 
        public int TprocesamientoBloquesLibres(Libres AdminLibres, int uAdeseada) 
        {
            /*************** @ROCIO*****************/
            /// Chicos no se si esta bien esta función. El problema está en LISTA DE LIBRES. Especificamente la linea de codifo que haces:
            /// " int bloquesDeseados = (int)Math.Ceiling((decimal)uAdeseada / (decimal)tamBloque); "
            /// En el caso en que tengamos  enlazada, no estas considerando el tamaño que te ocupa el indice en cada bloque
            /// En el caso en que tengamos indexada, no estas considerando si necesitas buscar un bloque extra para el indice.
            /// Diganme si me equivoco.
            /// 
            // Claro pero aca lo unico que haces es ver el tiempo de administracion de espacios libre, lo que tardas en buscar
            // espacios libres, lo que vos decis ya es asignarle el espacio libre a un archivo, eso ya es parte del tiempo
            // de otra funcion (creo que va a ser del Create y Write)

            int tiempo = 0;

            switch(AdminLibres)
            {
                case Libres.MapadeBits:
                    tiempo = GetTprocesamient();
                    break;

                case Libres.ListadeLibres:
                    int bloquesDeseados = (int)Math.Ceiling((decimal)uAdeseada / (decimal)tamBloque); 
                    tiempo = (GetTseek() + GetTlectura()) * bloquesDeseados;
                    break;

                case Libres.ListadeLibresdePrincipioyCuenta:
                    tiempo = 2 * GetTprocesamient();
                    break;

            }

            return tiempo;
        }

        //Devuelve true si se pudo asignar el espacio necesitado (bloques libres) y false si no pudo
        public bool GetLibres(int uAdeseada, string OrgaFisica, ref Archivo arch)
        {
            bool ObtuveLibres = false;

            if (OrgaFisica.Equals("contigua"))
            { //////@AYRTON DESDE LA INTERFAZ MANDA LOS NOMBRES EN MINUSCULA
                int bloquesDeseados = (int)Math.Ceiling((decimal)uAdeseada / (decimal)tamBloque);

                List<int> bloquesLibres = new List<int>(bloquesDeseados);
                bloquesLibres.AddRange(getDireccionBloqueContiguo(bloquesDeseados));

                if (bloquesLibres.Count != 0)
                {
                    ObtuveLibres = true; //si obtuve libres devuelvo verdadero
                    arch.TablaDireccion_AddRange(bloquesLibres);
                }//sino devuelve false

            }
            else if (OrgaFisica.Equals("enlazada"))
            {

                //// ************************ @ROCIO ***************************////
                ///Chicos, aca vos le estas sumando, a las unidades de almacenamiento deseado totales, UNA sola vez el tamaño de UN indice. Me huele a que estamos haciendolo mal
                ////Posible solucion
                /// int tamBloqueEnlazada = tamBloque - tamIndice ----------->>> AGREGAR: normalizas los bloques, ajustandolos a enlazada

                // Rocio, si creo que tenes razon, deberia quedar algo asi me parece:
                // int bloquesDeseados = (int)Math.Ceiling((decimal) (uAdeseada) / (decimal)(tamBloque-tamIndice));
                // si estan de acuerdo, descomentamos la de arriba y borramos la de abajo
                int bloquesDeseados = (int)Math.Ceiling((decimal) (uAdeseada) / (decimal)tamBloque); ////----------------------> eliminar
                List<int> bloquesLibres = new List<int>(bloquesDeseados);
                bloquesLibres.AddRange(getDireccionBloqueLibre(bloquesDeseados));

             /// NO LEAN ESTE COMMENT ES PARA ROCIO NOMAS  ELIMINAR LLEGUE A NORMALIZAR LOS BLOQUES ME FALTA ANALIZAR LA ASIGNACION

                if (bloquesLibres.Count != 0)
                {
                    ObtuveLibres = true; //si obtuve libres devuelvo verdadero
                    arch.TablaDireccion_AddRange(bloquesLibres);

                    // Asigno el tamaño del indice a las uABurocracia de cada bloque asignado
                    for (int i = 0; i < bloquesDeseados; i++)
                    {
                        // Obtengo la posicion del bloque
                        int posBloque = (int)arch.getTablaDireccion()[i];
                        TablaBloques[posBloque].uABurocracia = tamIndice;
                    //TablaBloques[posBloque].uAOcupado = tamBloque - tamIndice; 
                    //DUDA: esto de arriba iria??? o cuando asignamos este espacio ocupado???
                    // No, se asigna el espacio ocupado cuando se va a escribir en el archivo(Fede)
                    }
                }//sino devuelve false 

            }


            ////*****************@ROCIO *************************/////////
            ///Cuando hacemos el check storage no chequeamos que haya espacio extra para los bloques indices.
            ///Posible solución: ver lineas comentadas con flecha nueva!!!!!!
            /// Busca lo que dice @RESPUESTA ROCIO en la funcion checkStorage, lo mismo que pusiste esta implementado ya
            else if (OrgaFisica.Equals("indexado"))
            {
                int bloquesDeseados = (int)Math.Ceiling((decimal)uAdeseada / (decimal)tamBloque);

                ///     int cantUAindice = bloquesDeseados * tamIndice; ------------------------------------------>nueva
                ///     bloquesDeseados += (int)Math.Ceiling((decimal)cantUAindice / (decimal)tamBloque); -----------> nueva

                if (checkStorage(bloquesDeseados, arch.getTablaIndices()))
                {
                    List<int> bloquesLibres = new List<int>(bloquesDeseados);
                    bloquesLibres.AddRange(getDireccionBloqueLibre(bloquesDeseados));

                    if (bloquesLibres.Count != 0) // DUDA: habria que comprobar tambien aca los indices??? VER COMENTARIO ROCIO
                    {                             // Ya se sabe que hay espacio para los bloques deseados del usuario y para los del indice (@RESPUESTA ROCIO)
                        ObtuveLibres = true; //si obtuve libres devuelvo verdadero
                        arch.TablaDireccion_AddRange(bloquesLibres);
                        arch.TablaIndice_AddRange(getDireccionBloqueLibreIndice(bloquesDeseados, arch.getTablaIndices()));
                    }//sino devuelve false
                }
            }

            return ObtuveLibres;
        }

        private List<int> getDireccionBloqueLibreIndice( int BloquesDeseados, List<int> TablaIndices )
        {
            List<int> bloquesLibresIndices = new List<int>();
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
                    List<int> aux = new List<int>();

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

        private List<int> getDireccionBloqueLibre ( int bloquesDeseados ) // solo valido para indexada/ enlazada
        {
            List<int> bloquesLibres = new List<int>(bloquesDeseados);
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

        private List<int> getDireccionBloqueContiguo(int bloquesDeseados)
        {
            List<int> bloquesContiguos = new List<int>();
            
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
                    bloquesContiguos.Add(posInicial + i);
                    TablaBloques[posInicial + i].estadoReserva = true;
                }
            }
            else
            {
                throw new Exception("No hay suficiente espacio de almacenamiento para el archivo solicitado");
            }

            return bloquesContiguos;
        }
        
        private bool checkStorage(int bloquesdeseados, List<int> TablaIndices)
        {
            int bloquesDisponibles = 0;
            int posBloque = GetCantBloques();
            int cant_uaI;
            int cant_bloquesI = 0;

            if (TablaIndices.Count == 0)
            {
                //Se obtienen la cantidad de uA que ocupan los indices para los BloquesDeseados
                cant_uaI = bloquesdeseados * tamIndice;
                // Se divide la cantidad anterior por el tamaño de bloque para obtener cuantos bloques
                // son necesarios para almacenar los indices 
                cant_bloquesI = (int)Math.Ceiling((decimal)cant_uaI / (decimal)tamBloque);
                // @RESPUESTA ROCIO
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

            while ((bloquesDisponibles < (cant_bloquesI + bloquesdeseados) && (posBloque >= 0))) // @RESPUESTA ROCIO
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
        
        public int GetTAcceso()
        {
            return tAcceso;
        }

        private void SetTAcceso(int value)
        {
            tAcceso = value;
        }
        
        public int GetEspacioLibre()
        {
            return espacioLibre;
        }

        private void SetEspacioLibre(int value)
        {
            espacioLibre = value;
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

