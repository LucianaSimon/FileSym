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

public class Indicadores
{

    #region Attributes

    private float tSatisfaccion;


    private float tLectura;


    private float tEscritura;


    private float tGestionTotal;


    private float tMax;


    private float tMin;



    #endregion


}

