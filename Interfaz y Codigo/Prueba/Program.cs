using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


namespace FireSim
{
    class Program
    {
        static void Main(string[] args)
        {
            string ruta = "../../../Datos.csv";
            FileSim file = new FileSim(1, "indexada", "scan", "mapa de bits", 1, 1, 1, 1, 1, 1, 100, 100, ruta);
        }
    }
} 
