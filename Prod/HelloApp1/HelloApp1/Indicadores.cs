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