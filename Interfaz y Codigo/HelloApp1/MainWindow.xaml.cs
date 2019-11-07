using System;
using System.Windows;
using FireSim;


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
                Globales.rutaArchivo = dlg.FileName;
                //textArchivoSelec.Content = textArchivoSelec.Content + Globales.rutaArchivo;

                //Crear evento para habilitar el boton 'Comenzar
                btnComenzar.IsEnabled = true;
            }
        }

        //boton configuracion
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Config subWindow = new Config();
            //this.WindowState = WindowState.Minimized;
            this.Topmost = false;
            subWindow.Show();
            subWindow.Activate();
            
        }

        //boton Comenzar
        private void btnComenzar_Click(object sender, RoutedEventArgs e)
        {
            if(Globales.orgFisica == Org.Vacio || Globales.admLibre == Libres.Vacio 
                || Globales.modoAcceso == Acceso.Vacio)
            {
                //MessageBox.Show("Selecione las configuraciones necesarias para la simulación");
                MessageBoxResult result = System.Windows.MessageBox.Show("Selecione las configuraciones necesarias para la simulación", 
                    "Error de configuración", MessageBoxButton.OK);
            }
            else
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
}
