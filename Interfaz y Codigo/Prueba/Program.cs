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
            /*
             * Prueba metodo CargarOperaciones, que tambien las ordena (Funciono)
             * string ruta = "../../../Datos.csv";
             * FileSim file = new FileSim(1, "indexada", "scan", "mapa de bits", 1, 1, 1, 1, 1, 1, 100, 100, ruta);
            */

            /*
             * Prueba Metodo GetDireccionBloqueLibreIndice (Funciono) (Hacer metodo publico siquieren probrarlo)

            Dispositivo disp = new Dispositivo(1, 1, 2, 2, 5, 100, 1, 100);
            // Caso con TablaIndice vacia
            // List<int> bloquesLibresIndices = disp.getDireccionBloqueLibreIndice(1, new List<int>());

            // Caso con TablaIndice ocupada
            List<int> TablaIndice = new List<int>();
            TablaIndice = disp.getDireccionBloqueLibreIndice(4, TablaIndice);
            List<int> bloquesLibresIndices = disp.getDireccionBloqueLibreIndice(1, TablaIndice);
            foreach (int i in bloquesLibresIndices)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            foreach (Bloque b in disp.getTablaBloques())
            {
                Console.WriteLine(b.uAOcupado + " " + b.uABurocracia + " " + b.estadoReserva);
            }
            */

            /*
             * Prueba Metodo getDireccionBloqueLibre (Funciona) (Hacer publico el metodo para probar)
             

            Dispositivo disp = new Dispositivo(1, 1, 2, 2, 5, 100, 1, 100);
            List<int> bloquesLibres = new List<int>();
            try
            {
                bloquesLibres = disp.getDireccionBloqueLibre(21);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }
            
            foreach (int i in bloquesLibres)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            foreach (Bloque b in disp.getTablaBloques())
            {
                Console.WriteLine(b.uAOcupado + " " + b.uABurocracia + " " + b.estadoReserva);
            }
            */

            /*
             * Prueba Metodo getDireccionBloqueContiguo (Funciono) (Hacer publico el metodo para probar)
             
            Dispositivo disp = new Dispositivo(1, 1, 2, 2, 5, 100, 1, 100);
            List<int> bloquesContiguos = new List<int>();

            try
            {
                bloquesContiguos = disp.getDireccionBloqueContiguo(21);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e);
            }

            foreach (int i in bloquesContiguos)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            foreach (Bloque b in disp.getTablaBloques())
            {
                Console.WriteLine(b.uAOcupado + " " + b.uABurocracia + " " + b.estadoReserva);
            }

            */


        }
    }
} 
