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
            info_resultados.Text = "\n" +
                "* Fragmentación interna total: " + res["fragInt"].ToString() + " UA\n" +
                "* Fragmentación externa total: " + res["fragExt"].ToString() + " UA\n" +
                "* Tiempo de gestion: " + res["tGestion"].ToString() + " UT\n" +
                "* Tiempo de espera: " + res["tEspera"].ToString() + " UT\n" +
                "* Tiempo de simulación: " + res["tSimulacion"].ToString() + "UT\n";

            if ((int)res["tMaxN"] > 0) info_resultados.Text = info_resultados.Text + "* Tiempo máximo y mínimo de CREATE: " + res["tMaxN"].ToString() + " y " + res["tMinN"].ToString() + " UT\n";
            if ((int)res["tMaxD"] > 0) info_resultados.Text = info_resultados.Text + "* Tiempo máximo y mínimo de DELETE: " + res["tMaxD"].ToString() + " y " + res["tMinD"].ToString() + " UT\n";
            if ((int)res["tMaxR"] > 0) info_resultados.Text = info_resultados.Text + "* Tiempo máximo y mínimo de READ: " + res["tMaxR"].ToString() + " y " + res["tMinR"].ToString() + " UT\n";
            if ((int)res["tMaxW"] > 0) info_resultados.Text = info_resultados.Text + "* Tiempo máximo y mínimo de WRITE: " + res["tMaxW"].ToString() + " y " + res["tMinW"].ToString() + " UT\n";
            if ((int)res["tMaxO"] > 0) info_resultados.Text = info_resultados.Text + "* Tiempo máximo y mínimo de OPEN: " + res["tMaxO"].ToString() + " y " + res["tMinO"].ToString() + " UT\n";
            if ((int)res["tMaxC"] > 0) info_resultados.Text = info_resultados.Text + "* Tiempo máximo y mínimo de CLOSE: " + res["tMaxC"].ToString() + " y " + res["tMinC"].ToString() + " UT\n";

            //Grafico de TORTA
            int dato = (int)res["datos"];
            int meta = (int)res["metadatos"];
            grafico.RefreshData(dato, meta);
        }

        //Exportar
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Con los resultados obtenidos de la simulacion se crea un File 
            // y se almacenan los indicadores para cada operacion.
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string fileText = "\n Indicadores del sistema: \n\n" +
                "* Fragmentación interna total: " + res["fragInt"].ToString() + " UA\n" +
                "* Fragmentación externa total: " + res["fragExt"].ToString() + " UA\n" +
                "* Datos: " + res["datos"].ToString() + " UA\n" +
                "* Metadatos: " + res["metadatos"].ToString() + " UA\n" +
                "* Tiempo de gestion: " + res["tGestion"].ToString() + " UT\n" +
                "* Tiempo de espera: " + res["tEspera"].ToString() + " UT\n" +
                "* Tiempo de simulación: " + res["tSimulacion"].ToString() + "UT\n";

            if ((int)res["tMaxN"] > 0) info_resultados.Text = info_resultados.Text + "* Tiempo máximo y mínimo de CREATE: " + res["tMaxN"].ToString() + " y " + res["tMinN"].ToString() + " UT\n";
            if ((int)res["tMaxD"] > 0) info_resultados.Text = info_resultados.Text + "* Tiempo máximo y mínimo de DELETE: " + res["tMaxD"].ToString() + " y " + res["tMinD"].ToString() + " UT\n";
            if ((int)res["tMaxR"] > 0) info_resultados.Text = info_resultados.Text + "* Tiempo máximo y mínimo de READ: " + res["tMaxR"].ToString() + " y " + res["tMinR"].ToString() + " UT\n";
            if ((int)res["tMaxW"] > 0) info_resultados.Text = info_resultados.Text + "* Tiempo máximo y mínimo de WRITE: " + res["tMaxW"].ToString() + " y " + res["tMinW"].ToString() + " UT\n";
            if ((int)res["tMaxO"] > 0) info_resultados.Text = info_resultados.Text + "* Tiempo máximo y mínimo de OPEN: " + res["tMaxO"].ToString() + " y " + res["tMinO"].ToString() + " UT\n";
            if ((int)res["tMaxC"] > 0) info_resultados.Text = info_resultados.Text + "* Tiempo máximo y mínimo de CLOSE: " + res["tMaxC"].ToString() + " y " + res["tMinC"].ToString() + " UT\n";

            String textoOperacion = "";
            List<Indicadores> indicadores = (List<Indicadores>)res["IndicadoresOP"];

            
            for (int i=0; i<indicadores.Count; i++)
            {
                textoOperacion = textoOperacion +
                    "\nOperación" + i + ":\n" +
                    "* Tiempo de satisfaccion: " + indicadores[i].tSatisfaccion.ToString() + " UT\n" +
                    "* Tiempo de lectura/escritura: " + indicadores[i].tLectoEscritura.ToString() + " UT\n" +
                    "* Tiempo de espera: " + indicadores[i].tEspera.ToString() + " UT\n" +
                    "* Tiempo de gestion total: " + indicadores[i].tGestionTotal.ToString() + " UT\n\n";
            }

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Archivo de texto(*.txt)|*.txt"
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, (fileText + textoOperacion));
            }
        }

        //Salir
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
