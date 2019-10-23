using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public enum estadoOp
{
    Espera = 0,
    Ejecutando = 1,
    Finalizado = 2
}

public class Operacion
{ 
    private string NombreArchivo;
    private char IdOperacion;
    private int NumProceso;
    private int Tarribo;
    private int Offset;
    private int CantidadUA;
    private estadoOp Estado;    //Espera, Ejecutando, Finalizado
    

    public Operacion(string nombreArchivo, char idOperacion, int numProceso, int tArribo, int offset, int cantidadUA, estadoOp estado)
    {
        this.NombreArchivo = nombreArchivo;
        this.IdOperacion = idOperacion;
        this.NumProceso = numProceso;
        this.Tarribo = tArribo;
        this.Offset = offset;
        this.CantidadUA = cantidadUA;
        this.Estado = estado;
    }

}

