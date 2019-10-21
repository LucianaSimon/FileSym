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


public class FileSim
{

    #region Attributes

    private int Tprocesamiento;


    private string OrganizacionFisica;


    private string AlgoritmoBusqueda;


    private string AdminEspacio;


    private int MetocoAcceso;


    private Array<Operacion> TablaOperaciones;


    private Dispositivo disp;


    private int ContadorOp;


    private map<string, Indicadores> resultados;



    #endregion


    #region Public methods

    /// <summary>
    /// En el constructor de filesim se crearia el array de operaciones (vacio)
    /// </summary>
    /// <returns></returns>
    public FileSim(char Se_pasaria_toda_la_config)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    /// <summary>
    /// aca se arma el mapa
    /// </summary>
    /// <returns></returns>
    public void CargarOperaciones(string ruta)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public int CantBloques()
    {
        disp.getcantbloques()
    }

    public int EstadoBloque()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public int GetCantidadOp()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public int GetContadorOp()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public Operacion GetOperacion()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    /// <summary>
    /// case --> CREATE (n)/ WRITE (w)/ READ(r)/ OPEN(o)/ CLOSE(c)/DELETE (d)
    /// </summary>
    /// <returns></returns>
    public void SimularSiguienteOp()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public void Create()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public void Write()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public void Read()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public void Delete()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public void Open()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public void Close()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    #endregion


}

