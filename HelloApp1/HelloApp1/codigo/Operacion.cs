/*
 * Guarda todos los datos correspondientes a una operacion
 * La operacion puede encontrarse en 4 estados
 * Estado Listo: la operacion esta lista para ser ejecutada
 * Estado Espera: la operacion trato de ejecutarse y por algun motivo no pudo realizarse, queda en espera a que terminen el resto de las operaciones y luego vuelve a intentar
 * Estado Realizado: la operacion se ejecuto satisfactoriamente
 * Estado Error: la operacion no puede realizarse
 */

using System;

public enum EstadoOp { Listo, Espera, Realizado, Error}
public class Operacion
{
    public string NombreArchivo { get; set; }
    public string IdOperacion { get; set; }
    public int NumProceso { get; set; }
    public int Tarribo{ get; set; }
    public int Offset { get; set; }
    public int CantidadUA { get; set; }
    public EstadoOp estado { get; set; }

    public Operacion()
    {
        this.NombreArchivo = "";
        this.IdOperacion = "";
        this.NumProceso = 0;
        this.Tarribo = -1;
        this.Offset = -1;
        this.CantidadUA = -1;
        this.estado = EstadoOp.Error;
    }

    public Operacion(string name, string idOp, int idP, int tA, int offs, int cuA, EstadoOp e)
    {
        this.NombreArchivo = name;
        this.IdOperacion = idOp;
        this.NumProceso = idP;
        this.Tarribo = tA;
        this.Offset = offs;
        this.CantidadUA = cuA;
        this.estado = e;
    }
    public void setEstado(EstadoOp e)
    {
       estado = e;
    }
    
    // Solo para debug!!!!!
    public override string  ToString()
    {
        string res;
        res = this.NombreArchivo + "\t" + this.IdOperacion + "\t" + this.NumProceso + "\t" +
                        this.Tarribo + "\t" + this.Offset + "\t" + this.CantidadUA + "\t";
        switch(this.estado)
        {
            case EstadoOp.Error:
                {
                    res += "Error";
                    break;
                }
            case EstadoOp.Espera:
                {
                    res += "Espera";
                    break;
                }
            case EstadoOp.Listo:
                {
                    res += "Listo";
                    break;
                }
            case EstadoOp.Realizado:
                {
                    res += "Realizado";
                    break;
                }
        }

        return res;
    }
}

