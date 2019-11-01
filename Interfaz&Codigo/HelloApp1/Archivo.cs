using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace FireSim
{
    public class Archivo
    {
        private string nombreArchivo;
        private int estado;
        private int cant_uA;
        private List<int> TablaDirecciones;
        private List<int> TablaDireccionesIndice; // Se utiliza en el caso de indexada unicamente

        public Archivo()
        {
            this.nombreArchivo = "";
            this.estado = -1;
            this.cant_uA = 0;
            this.TablaDirecciones = new List<int>();
            this.TablaDireccionesIndice = new List<int>();
        }

        public void TablaDireccion_AddRange(List<int> value)
        {
            this.TablaDirecciones.AddRange(value);
        }

        public void TablaIndice_AddRange(List<int> value)
        {
            this.TablaDireccionesIndice.AddRange(value);
        }

        public string getNombre()
        {
            return this.nombreArchivo;
        }

        public void setNombre(string value)
        {
            this.nombreArchivo = value;
        }

        public int getEstado()
        {
            return this.estado;
        }

        public void setEstado(int value)
        {
            this.estado = value;
        }
        
        public int getCant_uA()
        {
            return this.cant_uA;
        }

        public void setCant_uA(int value)
        {
            this.cant_uA = value;
        }

        public List<int> getTablaDireccion()
        {
            return this.TablaDirecciones;
        }

        public void setTablaDireccion(List<int> value)
        {
            this.TablaDirecciones = value;
        }

        public List<int> getTablaIndices()
        {
            return this.TablaDireccionesIndice;
        }

        public void setTablaIndices(List<int> value)
        {
            this.TablaDirecciones = value;
        }
    }
}
