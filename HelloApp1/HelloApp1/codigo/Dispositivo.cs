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
        private int tProcesamiento;        
        private int tamBloque;
        private int tamDispositivo;
        private int tamIndice = 1; //tamanio que ocupa un indice --> burocracia
        private int cantBloques;
        private Bloque[] TablaBloques; //arreglo fijo, dispositivo no puede crecer en tamaño fisico

        public Dispositivo(int tLectura, int tEscritura, int tSeek, int tamBloques, int tamDispositivo, int tProcesamiento)
        {
            //seteamos los parametros de entrada
            this.SetTlectura(tLectura);
            this.SetTescritura(tEscritura);
            this.SetTamBloques(tamBloques);
            this.SetTamDispositivo(tamDispositivo);
            this.SetCantBloques((int)Math.Truncate((decimal)tamDispositivo / (decimal)tamBloques)); //windows no considera fragmentacion externa si no es entera la division del dispositivo en bloques 
            this.SetTprocesamiento(tProcesamiento);

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
        public int TprocesamientoBloquesLibres(Libres AdminLibres, int uAdeseada) 
        {
            int tiempo = 0;

            switch(AdminLibres)
            {
                case Libres.MapadeBits:
                    tiempo = GetTseek() + GetTlectura() + GetTprocesamient();
                    break;

                case Libres.ListadeLibres:
                    int bloquesDeseados = (int)Math.Ceiling((decimal)uAdeseada / (decimal)tamBloque); 
                    tiempo = (GetTseek() + GetTlectura()) * bloquesDeseados + GetTprocesamient();
                    break;

                case Libres.ListadeLibresdePrincipioyCuenta:
                    tiempo = GetTseek()  + 2*(GetTlectura() + GetTprocesamient());
                    break;
            }

            return tiempo;
        }

        //Devuelve true si se pudo asignar el espacio necesitado (bloques libres) y false si no pudo
        public bool GetLibres(int uAdeseada, Org OrgaFisica, ref Archivo arch)
        {
            bool ObtuveLibres = false;

            if (OrgaFisica == Org.Contigua)
            {
                int bloquesDeseados = (int)Math.Ceiling((decimal)uAdeseada / (decimal)tamBloque);

                List<int> bloquesLibres;
                
               if (arch.getTablaDireccion().Count > 0)
                {
                    bloquesLibres = arch.getTablaDireccion();
                    try
                    {
                        bloquesLibres.AddRange(Realloc(uAdeseada, arch.getTablaDireccion()));
                        ObtuveLibres = true; //si obtuve libres devuelvo verdadero
                        arch.setTablaDireccion(bloquesLibres);  // Se mueve todo siempre!
                    }
                    catch
                    {
                        // Si llega aca es que no hay los bloques libres necesarios a continuacion
                        List<int> dir = arch.getTablaDireccion();
                        for (int i=0; i<dir.Count; i++)
                        {
                            TablaBloques[dir[i]].estadoReserva = false;
                        }

                        try
                        {
                            bloquesLibres = getBloquesLibresContiguos(bloquesDeseados + dir.Count);
                        }
                        catch
                        {
                            for (int i = 0; i < dir.Count; i++)
                            {
                                TablaBloques[dir[i]].estadoReserva = true;
                            }
                            throw new Exception("No hay mas espacio");
                        }
                        

                        for (int j=0; j<dir.Count; j++)
                        {
                            TablaBloques[bloquesLibres[j]].uAOcupado = TablaBloques[dir[j]].uAOcupado;
                            TablaBloques[bloquesLibres[j]].uABurocracia = TablaBloques[dir[j]].uABurocracia;
                            TablaBloques[dir[j]].uAOcupado = 0;
                            TablaBloques[dir[j]].uABurocracia = 0;
                        }
                        ObtuveLibres = true; //si obtuve libres devuelvo verdadero
                        arch.setTablaDireccion(bloquesLibres);  // Se mueve todo siempre!
                    }
                    
                }
               else
                {
                    try
                    {
                        bloquesLibres = getBloquesLibresContiguos(bloquesDeseados);
                        ObtuveLibres = true; //si obtuve libres devuelvo verdadero
                        arch.setTablaDireccion(bloquesLibres);  // Se mueve todo siempre!
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }

            }
            else if (OrgaFisica == Org.Enlazada)
            {

                 int bloquesDeseados = (int)Math.Ceiling((decimal) (uAdeseada) / (decimal)(tamBloque-tamIndice));

                List<int> bloquesLibres = new List<int>(bloquesDeseados);

                try
                {
                    bloquesLibres.AddRange(getDireccionBloqueLibre(bloquesDeseados));
                }
                catch (Exception e)
                {
                    throw new Exception("No hay mas espacio de almacenamiento");
                    // Console.WriteLine("Error: " + e);
                }

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
                    }
                }//sino devuelve false 

            }
            else if (OrgaFisica == Org.Indexada)
            {
                int bloquesDeseados = (int)Math.Ceiling((decimal)uAdeseada / (decimal)tamBloque);


                if (checkStorage(bloquesDeseados, arch.getTablaIndices()))
                {
                    List<int> bloquesLibres = new List<int>(bloquesDeseados);

                    try
                    {
                        bloquesLibres.AddRange(getDireccionBloqueLibre(bloquesDeseados));
                    }
                    catch (Exception e)
                    {
                        throw new Exception("No hay mas espacio de almacenamiento");
                        // Console.WriteLine("Error: " + e);
                    }

                    if (bloquesLibres.Count != 0) { 
                        ObtuveLibres = true; //si obtuve libres devuelvo verdadero
                        arch.TablaDireccion_AddRange(bloquesLibres);

                        try
                        {
                            arch.TablaIndice_AddRange(getDireccionBloqueLibreIndice(bloquesDeseados, arch.getTablaIndices()));
                        }
                        catch (Exception e)
                        {
                            throw new Exception("No hay mas espacio de almacenamiento");
                            // Console.WriteLine("Error: " + e);
                        }
                    }//sino devuelve false
                }
                else
                {
                    throw new Exception("No hay mas espacio de almacenamiento");
                }
            }

            return ObtuveLibres;
        }

        public List<int> getDireccionBloqueLibreIndice( int BloquesDeseados, List<int> TablaIndices )
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

                int posBloque = 0; //arranca desde el principio a almacenar los bloques indices --> first fit
                int posIndice = 0;

                while ((posIndice < cant_bloquesI) && (posBloque < GetCantBloques() ))
                {
                    if (!TablaBloques[posBloque].estadoReserva)
                    {
                        bloquesLibresIndices.Add(posBloque);
                        posIndice++;
                    }
                    posBloque++;
                }
                if (posIndice == cant_bloquesI)
                {
                    for (int i=0; i<posIndice; i++)
                    {
                        int numBloque = (int)bloquesLibresIndices[i];
                        TablaBloques[numBloque].estadoReserva = true;
                        // Voy agregando "indices" al bloque indice hasta llenarlo o ya no necesitar guardar indices
                        while((TablaBloques[numBloque].uABurocracia < tamBloque) && (cant_uaI > 0))
                        {
                            TablaBloques[numBloque].uABurocracia += tamIndice;
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
                int ultimoIndice = TablaIndices.Count - 1;
                //Se obtienen la cantidad de uA que ocupan los indices para los BloquesDeseados
                int cant_uaI = BloquesDeseados * tamIndice;
                // Compruebo si la cantidad de uA para indice que necesito entra en el ulimo indice
                // si es asi agrego cant_uaI a uABurocracia del ultimo indice y la tabla no se modifica
                if ((tamBloque - TablaBloques[(int)TablaIndices[ultimoIndice]].uABurocracia) >= cant_uaI)
                {
                    TablaBloques[(int)TablaIndices[ultimoIndice]].uABurocracia += cant_uaI;
                    bloquesLibresIndices = TablaIndices;
                }
                else // Si no, tengo que agregar la diferencia y buscar un nuevo indice
                {
                    int diff = (tamBloque - TablaBloques[(int)TablaIndices[ultimoIndice]].uABurocracia);

                    // Asigno la cantidad de indices que entran en el ultimo indice
                    TablaBloques[(int)TablaIndices[ultimoIndice]].uABurocracia += diff;

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

            while ((aux < bloquesDeseados) && (contaTabla < cantBloques))
            {
               if (!TablaBloques[contaTabla].estadoReserva) // este valor devuelve true si no esta reservado
                {
                    bloquesLibres.Add(contaTabla);
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

        private List<int> getBloquesLibresContiguos(int bloquesDeseados)
        {
            List<int> bloquesContiguos = new List<int>();
            
            // Para verificar que existan la cantidad de bloques deseados contiguos
            int contiguos = 0;

            // Para recorrer la Tabla de Bloques
            int posBloque = 0;

            // Posicion donde comienzan los bloques contiguos
            int posInicial = -1;

            while ((posBloque < GetCantBloques()) && (contiguos < bloquesDeseados))
            {
                if (!TablaBloques[posBloque].estadoReserva)
                {
                    // Aumento el contador de bloques contiguos encontrados
                    contiguos++;
                    if (posInicial == -1)
                    {
                        posInicial = posBloque;
                    }
                }
                else
                {
                    // Si en algun momento se encuentra un bloque reservado sin cumplir la cantidad de bloques
                    // contiguos deseados, se deja de buscar y se lanza la excepcion
                    contiguos = 0;
                    posInicial = -1;
                }
                posBloque++;
            }
            if (contiguos == bloquesDeseados)
            {
                for (int i = 0; i < contiguos; i++)
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

        private List<int> Realloc(int bloquesDeseados, List<int> dir)
        {
            List<int> bloquesContiguos = new List<int>();

            // Para verificar que existan la cantidad de bloques deseados contiguos
            int contiguos = 0;

            // Para recorrer la Tabla de Bloques
            int posBloque = dir[dir.Count - 1] + 1; // El bloque siguiente al que ya esta asignado para este archivo;

            // Posicion donde comienzan los bloques contiguos
            int posInicial = posBloque; ;

            while ((posBloque <GetCantBloques()) && (contiguos < bloquesDeseados) && posInicial == dir[dir.Count -1])
            {
                if (!TablaBloques[posBloque].estadoReserva)
                {
                    // Aumento el contador de bloques contiguos encontrados
                    contiguos++;
                }
                else
                {
                    // Si en algun momento se encuentra un bloque reservado sin cumplir la cantidad de bloques
                    // contiguos deseados, se deja de buscar y se lanza la excepcion
                    contiguos = 0;
                    posInicial = -1;
                }
                posBloque++;
            }
            if (contiguos == bloquesDeseados)
            {
                bloquesContiguos = dir;
                for (int i = 0; i < contiguos; i++)
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
        
        public bool checkStorage(int bloquesdeseados, List<int> TablaIndices)
        {
            
            int bloquesDisponibles = 0;
            int posBloque = 0; //arranca desde el principio a modificar --> para respetar first fit
            int cant_uaI;
            int cant_bloquesI = 0;

            if (TablaIndices.Count == 0)
            {
                //Se obtienen la cantidad de uA que ocupan los indices para los BloquesDeseados
                cant_uaI = bloquesdeseados * tamIndice;
                // Se divide la cantidad anterior por el tamaño de bloque para obtener cuantos bloques
                // son necesarios para almacenar los indices 
                cant_bloquesI = (int)Math.Ceiling((decimal)cant_uaI / (decimal)tamBloque);
            }
            else
            {
                int ultimoIndice = TablaIndices.Count - 1;
                //Se obtienen la cantidad de uA que ocupan los indices para los BloquesDeseados
                cant_uaI = bloquesdeseados * tamIndice;

                // Compruebo si la cantidad de uA para indice que necesito entra en el ulimo indice
                // si es asi agrego cant_uaI a uABurocracia del ultimo indice y la tabla no se modifica
                if ((tamBloque - TablaBloques[(int)TablaIndices[ultimoIndice]].uABurocracia) >= cant_uaI)
                {
                    // No necesito ningun bloque extra para indices
                    cant_bloquesI = 0;
                }
                else // Busco la cantidad de bloques indice que necesito extra
                {
                    int diff = (tamBloque - TablaBloques[(int)TablaIndices[ultimoIndice]].uABurocracia);

                    cant_bloquesI = (int)Math.Ceiling((decimal)(cant_uaI - diff) / (decimal)tamBloque);
                }
            }

            while ((bloquesDisponibles < (cant_bloquesI + bloquesdeseados) && (posBloque < GetCantBloques() ))) 
            {
                if (!TablaBloques[posBloque].estadoReserva)
                {
                    bloquesDisponibles++;
                }
                posBloque++;
            }
            if ((bloquesdeseados + cant_bloquesI) == bloquesDisponibles)
            {
                return true;
            }
            return false;
        }

        public void Write(int inicio, int cant_uA, List<int> direcciones, Org orga)
        {          
            int i = inicio;  // Para recorrer la tabla de direcciones
            
            while (cant_uA > 0 && i < direcciones.Count)
            {
                int espacioDisponibleBloque = GetTamBloques() - getTablaBloques()[direcciones[i]].uABurocracia - getTablaBloques()[direcciones[i]].uAOcupado;
                if (espacioDisponibleBloque >= cant_uA) // Si se puede escribir todo en ese bloque lo escribo
                {
                    getTablaBloques()[direcciones[i]].uAOcupado += cant_uA;
                    cant_uA = 0;
                }
                else
                {
                    getTablaBloques()[direcciones[i]].uAOcupado += espacioDisponibleBloque;
                    cant_uA = cant_uA - espacioDisponibleBloque;
                }
                i++;
            }
        }
        public int getFragExt()
        {
            int cnt = 0;

            for (int i = 0; i < GetCantBloques(); i++)
            {
                if(!TablaBloques[i].estadoReserva)
                {
                    cnt++;
                }
            }

            return cnt;
        }

        public void datosMetadatos(ref int datos, ref int metadatos)
        {
            for (int i=0; i<GetCantBloques(); i++)
            {
                if (TablaBloques[i].uAOcupado > 0 || TablaBloques[i].uABurocracia > 0)
                {
                    datos += TablaBloques[i].uAOcupado;
                    metadatos += TablaBloques[i].uABurocracia;
                }
            }
        }

        public void getFragInt(ref int fragint, ref int cnt)
        {

            for (int i = 0; i < GetCantBloques(); i++)
            {
                if (TablaBloques[i].estadoReserva)
                {
                    if ((TablaBloques[i].uAOcupado + TablaBloques[i].uABurocracia) < GetTamBloques())
                    {
                        fragint += GetTamBloques() - (TablaBloques[i].uAOcupado + TablaBloques[i].uABurocracia);
                    }
                    cnt++;
                }
            }
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

        public Bloque[] getTablaBloques()
        {
            return TablaBloques;
        }
    }
}

