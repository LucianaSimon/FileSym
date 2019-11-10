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

        public char tipoOp { get; set; } //se usaria para hacer la comparacion de tmax y tmin!!!! 
                                        //almacena el tipo de operacion realizada relacionada con los tiempos 

        /*
         * Lista con los tiempos que lleva una operacion, el tamaño maximo debe ser 6 siempre!
         * Si la operacion es un create ese tiempo se debe introducir en la posicion 0 y de la posicion 1 a la 5 ponerlos en 0
         * Si la operacion es un open ese tiempo se debe introducir en la posicion 1 y la 0 y de la 2 a la 5 ponerlos en 0
         * Si la operacion es un close ese tiempo se debe introducir en la posicion 2 y la 0, 1 y de la 3 a la 5 ponerlos en 0
         * Si la operacion es un delete ese tiempo se debe introducir en la posicion 3 y la 0, 1, 2 y de la 4 a la 5 ponerlos en 0
         * Si la operacion es un read ese tiempo se debe introducir en la posicion 4 y de la 0 a la 3  y la 5 ponerlos en 0
         * Si la operacion es un write ese tiempo se debe introducir en la posicion 5 y de la 0 a la 4 ponerlos en 0
         */
        //public List<int> tOperaciones { get; set; } //se va a ir guardando el tSatisfaccion de cada operacion 

    }
}

/*
 * Tiempo de simulacion actual
 * Tiempo de satisfaccion todas las solicitudas (tiempo solicitud final)
 * Tiempo de lecto/escritura (efectivo)
 * tMax y tMin de cada operacion (C, N, D, W, R, O) del sistema!, no por archivo
 * Tiempo de espera promedio del sistema! 
 * Tiempo open, close, delete = tProcesamiento (siempre)
 */