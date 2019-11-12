using System;
using System.Collections.Generic;

namespace FireSim
{
    class Program
    {
        static void Main(string[] args)
        {
            FileSim simulador = new FileSim(2, Org.Contigua, Libres.MapadeBits, Acceso.Directo, 2, 3, 2, 5, 200, "../../../../Datos.csv");
            
            int simulaciones = 0;

            while(simulador.getOpActual() != -1)
            {
                simulador.SimularSiguienteOp();
                simulaciones++;
            }
            Console.WriteLine("Cantidad Operaciones Simuladas: " + simulaciones + "\r\n");

            Console.WriteLine("Tabla Operaciones Final");
            foreach(var op in simulador.getTablaOperaciones())
            {
                Console.WriteLine(op.ToString());
            }

            
            Console.WriteLine("Resultados");
            foreach (var i in simulador.Resultados())
            {
                if (i.Key.Equals("IndicadoresOP"))
                {
                    Console.WriteLine("Indicadores");
                    foreach(var indicador in (List<Indicadores>)i.Value)
                    {
                        Console.WriteLine("Op: " + indicador.tipoOp + "\ttSatisfaccion: " + indicador.tSatisfaccion + "\ttGestion: " + indicador.tGestionTotal +
                            "\ttLectoEscritura: " + indicador.tLectoEscritura + "\ttEspera: " + indicador.tEspera);
                    }
                }
                else
                {
                    Console.WriteLine(i.Key + " " + i.Value);
                }
                
            }
            
        }
    }
}
