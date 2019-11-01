using System;

namespace FireSim
{
    class Program
    {
        static void Main(string[] args)
        {
            // Cambien la ruta!
            string ruta = "C:\\Users\\Federico\\Documents\\Proyectos\\FileSym\\Interfaz&Codigo\\Prueba\\Datos.csv";
            FileSim file = new FileSim(1, "contigua", "scan", "mapa de bits", 2, 2, 2, 1, 1, 4, 200, 200, ruta);
            
        }
    }
}
