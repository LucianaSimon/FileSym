using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;


public class Dispositivo
{

    #region Attributes

    private int Tlectura;


    private int Tescritura;


    private int Tseek;


    private int TamBloques;


    private int TamDispositivo;


    private int CantBloques;


    private Array<float> TablaBloques;



    #endregion


    #region Public methods

    public Dispositivo()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public int estadoBloque(int numBloque)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public void CambiarEstado(int numBloque, float Estado)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    #endregion


}


using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;


public class Operacion
{

    #region Attributes

    private string NombreArchivo;


    private char IdOperacion;


    private int NumProceso;


    private int Tarribo;


    private int Offset;


    private int CantidadUA;


    private estadoOp Estado;



    #endregion


    #region Public methods

    public void Operacion_string_char_int_int_int_int_()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    #endregion


}

