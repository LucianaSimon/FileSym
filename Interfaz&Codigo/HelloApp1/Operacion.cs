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

public struct Operacion
{
    public string NombreArchivo;
    public char IdOperacion;
    public int NumProceso;
    public int Tarribo;
    public int Offset;
    public int CantidadUA;
    public estadoOp Estado;
}

