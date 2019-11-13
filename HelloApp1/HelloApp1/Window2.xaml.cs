using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using FireSim;

namespace HelloApp1
{
    /// <summary>
    /// Lógica de interacción para Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        Dictionary<string, Object> res = new Dictionary<string, object>();

        public Window2(Dictionary<string, Object> res)
        {
            InitializeComponent();
            this.res = res;
            //Prueba para cargar texto a info_resultados REVISAR!!!
            info_resultados.Text = Environment.NewLine +
                "* Fragmentación interna total: " + res["fragInt"].ToString() + " %" + Environment.NewLine +
                "* Fragmentación externa total: " + res["fragExt"].ToString() + " %" + Environment.NewLine +
                "* Tiempo de gestion: " + res["tGestion"].ToString() + " %" + Environment.NewLine +
                "* Tiempo de espera: " + res["tEspera"].ToString() + " UT" + Environment.NewLine +
                "* Tiempo de simulación: " + res["tSimulacion"].ToString() + " UT" + Environment.NewLine;

            if ((int)res["tMaxN"] > 0) info_resultados.Text = info_resultados.Text.ToString() + "* Tiempo máximo y mínimo de CREATE: " + res["tMaxN"].ToString() + " y " + res["tMinN"].ToString() + " UT\n";
            if ((int)res["tMaxD"] > 0) info_resultados.Text = info_resultados.Text.ToString() + "* Tiempo máximo y mínimo de DELETE: " + res["tMaxD"].ToString() + " y " + res["tMinD"].ToString() + " UT\n";
            if ((int)res["tMaxR"] > 0) info_resultados.Text = info_resultados.Text.ToString() + "* Tiempo máximo y mínimo de READ: " + res["tMaxR"].ToString() + " y " + res["tMinR"].ToString() + " UT\n";
            if ((int)res["tMaxW"] > 0) info_resultados.Text = info_resultados.Text.ToString() + "* Tiempo máximo y mínimo de WRITE: " + res["tMaxW"].ToString() + " y " + res["tMinW"].ToString() + " UT\n";
            if ((int)res["tMaxO"] > 0) info_resultados.Text = info_resultados.Text.ToString() + "* Tiempo máximo y mínimo de OPEN: " + res["tMaxO"].ToString() + " y " + res["tMinO"].ToString() + " UT\n";
            if ((int)res["tMaxC"] > 0) info_resultados.Text = info_resultados.Text.ToString() + "* Tiempo máximo y mínimo de CLOSE: " + res["tMaxC"].ToString() + " y " + res["tMinC"].ToString() + " UT\n";

            //Grafico de TORTA
            int dato = (int)res["datos"];
            int meta = (int)res["metadatos"];
            grafico.RefreshData(dato, meta);
        }

        //Exportar

        public bool esValido(int tiempo)
        {
            if(tiempo > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Con los resultados obtenidos de la simulacion se crea un File 
            // y se almacenan los indicadores para cada operacion.
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string fileText = "Indicadores del sistema:" + Environment.NewLine +
                "* Fragmentación interna total: " + res["fragInt"].ToString() + " %" + Environment.NewLine +
                "* Fragmentación externa total: " + res["fragExt"].ToString() + " %" + Environment.NewLine +
                "* Tiempo de gestion: " + res["tGestion"].ToString() + " UT" + Environment.NewLine +
                "* Tiempo de espera: " + res["tEspera"].ToString() + " UT" + Environment.NewLine +
                "* Tiempo de simulación: " + res["tSimulacion"].ToString() + " UT" + Environment.NewLine;

            if ((int)res["tMaxN"] > 0) fileText = fileText + "* Tiempo máximo y mínimo de CREATE: " + res["tMaxN"].ToString() + " y " + res["tMinN"].ToString() + " UT" + Environment.NewLine;
            if ((int)res["tMaxD"] > 0) fileText = fileText + "* Tiempo máximo y mínimo de DELETE: " + res["tMaxD"].ToString() + " y " + res["tMinD"].ToString() + " UT" + Environment.NewLine;
            if ((int)res["tMaxR"] > 0) fileText = fileText + "* Tiempo máximo y mínimo de READ: " + res["tMaxR"].ToString() + " y " + res["tMinR"].ToString() + " UT" + Environment.NewLine;
            if ((int)res["tMaxW"] > 0) fileText = fileText + "* Tiempo máximo y mínimo de WRITE: " + res["tMaxW"].ToString() + " y " + res["tMinW"].ToString() + " UT" + Environment.NewLine;
            if ((int)res["tMaxO"] > 0) fileText = fileText + "* Tiempo máximo y mínimo de OPEN: " + res["tMaxO"].ToString() + " y " + res["tMinO"].ToString() + " UT" + Environment.NewLine;
            if ((int)res["tMaxC"] > 0) fileText = fileText + "* Tiempo máximo y mínimo de CLOSE: " + res["tMaxC"].ToString() + " y " + res["tMinC"].ToString() + " UT" + Environment.NewLine;

            String textoOperacion = "";
            List<Indicadores> indicadores = (List<Indicadores>)res["IndicadoresOP"];

            for (int i=0; i<indicadores.Count; i++)
            {
                textoOperacion = textoOperacion +
                    Environment.NewLine + "Operación " + i + ":" + Environment.NewLine +
                    "* Tiempo de satisfaccion: " + indicadores[i].tSatisfaccion.ToString() + " UT" + Environment.NewLine +
                    "* Tiempo de lectura/escritura: " + indicadores[i].tLectoEscritura.ToString() + " UT" + Environment.NewLine +
                    "* Tiempo de espera: " + indicadores[i].tEspera.ToString() + " UT" + Environment.NewLine +
                    "* Tiempo de gestion total: " + indicadores[i].tGestionTotal.ToString() + " UT" + Environment.NewLine;
            }

            String config= "Las configuraciones para la simulacion son:" + Environment.NewLine + 
                "* Organización física: " + Globales.orgFisica + Environment.NewLine +
                "* Modo de acceso: " + Globales.modoAcceso + Environment.NewLine +
                "* Administración de espacios libres: " + Globales.admLibre + Environment.NewLine +
                "* Tiempo de acceso: " + Globales.tAcceso + Environment.NewLine +
                "* Tiempo de seek: " + Globales.tSeek + Environment.NewLine +
                "* Tiempo de lectura: " + Globales.tLectura + Environment.NewLine +
                "* Tiempo de escritura: " + Globales.tEscritura + Environment.NewLine +
                "* Tiempo de procesamiento: " + Globales.tProcesamiento + Environment.NewLine +
                "* Tamaño de bloque: " + Globales.tamBloque + " uA" + Environment.NewLine +
                "* Tamaño de dispositivo: " + Globales.tamDispositivo + " uA" + Environment.NewLine +
                "* Cantidad de bloques: " + (int)Math.Truncate((decimal)Globales.tamDispositivo / (decimal)Globales.tamBloque) + Environment.NewLine + Environment.NewLine;

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Archivo de texto(*.txt)|*.txt"
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, (config + fileText + textoOperacion));
            }
        }

        //Salir
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
