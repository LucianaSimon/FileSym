using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HelloApp1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         
        public MainWindow()
        {
            InitializeComponent();
            //Se inicializa las variables globales por defecto
            Globales.Inicializar();
        }

        //boton Archivo!
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (*.csv)|*.csv";

            Nullable<bool> result = dlg.ShowDialog();
            
            if (result == true)
            {
                string filename = dlg.FileName;
                textArchivoSelec.Content = textArchivoSelec.Content + filename;
                //Crear evento para habilitar el boton 'Comenzar
            }
        }

        //boton configuracion
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Config subWindow = new Config(configuracionActual);
            //this.WindowState = WindowState.Minimized;
            this.Topmost = false;
            subWindow.Show();
            subWindow.Activate();
            
        }

        //boton Comenzar
        private void btnComenzar_Click(object sender, RoutedEventArgs e)
        {
            Window1 subWindow = new Window1();
            //this.WindowState = WindowState.Minimized;
            this.Topmost = false;
            subWindow.Show();
            subWindow.Activate();
            this.Close();
        }
    }
}
