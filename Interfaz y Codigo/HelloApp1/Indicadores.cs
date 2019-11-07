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

        //public float tMax { get; set; } //DUDA @lu irian aca estos tiempos?? no se refiere al tmax o min total??? NOOOOOO!!!!

        //public float tMin { get; set; }
    }
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