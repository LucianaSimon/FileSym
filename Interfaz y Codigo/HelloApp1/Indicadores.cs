using System.Collections.Generic;

namespace FireSim
{
    // Estos son por operacion!!!
    // Son los que se exportan
    public struct Indicadores
    {
        public int tSatisfaccion { get; set; }
        public int tLectoEscritura { get; set; }
        public int tGestionTotal { get; set; }
        public int tEspera { get; set; }
<<<<<<< HEAD
        /*
         * Lista con los timpos que lleva una operacion, el tamaño maximo debe ser 6 siempre!
         * Si la operacion es un create ese tiempo se debe introducir en la posicion 0 y de la posicion 1 a la 5 ponerlos en 0
         * Si la operacion es un open ese tiempo se debe introducir en la posicion 1 y la 0 y de la 2 a la 5 ponerlos en 0
         * Si la operacion es un close ese tiempo se debe introducir en la posicion 2 y la 0, 1 y de la 3 a la 5 ponerlos en 0
         * Si la operacion es un delete ese tiempo se debe introducir en la posicion 3 y la 0, 1, 2 y de la 4 a la 5 ponerlos en 0
         * Si la operacion es un read ese tiempo se debe introducir en la posicion 4 y de la 0 a la 3  y la 5 ponerlos en 0
         * Si la operacion es un write ese tiempo se debe introducir en la posicion 5 y de la 0 a la 4 ponerlos en 0
         */
        public List<int> tOperaciones { get; set; }
    
}
=======
     //   public int tEscritura { get; set; }

//        public int tLectura { get; set; }

     ///   public float tMax { get; set; } //DUDA @lu irian aca estos tiempos?? no se refiere al tmax o min total??? NOOOOOO!!!!

      //  public float tMin { get; set; }
    }
>>>>>>> e9f1bce8826e8ca09a02b1805499d7894d3bd39d
}


/*
 * Tiempo de simulacion actual
 * Tiempo de satisfaccion todas las solicitudas (tiempo solicitud final)
 * Tiempo de lecto/escritura (efectivo)
 * tMax y tMin de cada operacion (C, N, D, W, R, O) del sistema!, no por archivo
 * Tiempo de espera promedio del sistema!
 * 
 * Tiempo de open y close es solo un t de procesamiento
 * 
 * Tiempo open, close, delete = tProcesamiento (siempre)
 */