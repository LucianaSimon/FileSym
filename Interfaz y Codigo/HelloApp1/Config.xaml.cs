using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System;
using System.Windows.Controls;

namespace HelloApp1
{
    static class Globales
    {
        public static string orgFisica;
        public static string admLibre;
        public static string modoAcceso;
        public static string algBusqueda;

        public static int tAcceso;
        public static int tSeek;
        public static int tLectura;
        public static int tEscritura;
        public static int tProcesamiento;
        public static int espacioInicial;
        public static int tamBloque;
        public static int tamDispositivo;


        public static void Inicializar() //valores x defecto
        {
            orgFisica = "";
            admLibre = "";
            modoAcceso = "";
            algBusqueda = "";

            tAcceso = 10;
            tSeek = 10;
            tLectura = 10;
            tEscritura = 10;
            tProcesamiento = 10;
            espacioInicial = 0;
            tamBloque = 10;      //uA
            tamDispositivo = 1000; //uA

        }
    }


    /// <summary>
    /// Lógica de interacción para Config.xaml
    /// </summary>
    public partial class Config : Window
    {
        private Label configuracionActual;

        public Config()
        {
            InitializeComponent();

            //Cuando se abre la pantalla config se mantiene la ultima
            //configuracion seleccionada
        }

        public Config(Label configuracionActual)
        {
            InitializeComponent();
            this.configuracionActual = configuracionActual; //para poder escribir en la pantalla principal 
            //(no se usara  en la version finalmente)

            //Cada vez que inicia la pantalla configuracion carga los valores globales, la primera vez va a cargar los valores por defecto
            //Luego a medida que el usuario realice cambios estos se veran persistidos.
            //Sliders (s)
            stAcceso.Value = Globales.tAcceso;
            stLectura.Value = Globales.tLectura;
            stEscritura.Value = Globales.tEscritura;
            stSeek.Value = Globales.tSeek;
            stProcesamiento.Value = Globales.tProcesamiento;

            tEspacioInicial.Text = Globales.espacioInicial.ToString();
            ttamBloque.Text = Globales.tamBloque.ToString();
            ttamDisp.Text = Globales.tamDispositivo.ToString();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Cuando le doy a aceptar deberia poder almacenar toda los datos de configuracion
            //en variables globales

            //Accedo a los ComboBox -> selectedItem
            Globales.orgFisica = cOrgFisica.Text.ToLower();
            Globales.admLibre = cAdmLibre.Text.ToLower();
            Globales.modoAcceso = cModoAcceso.Text.ToLower();
            Globales.algBusqueda = cAlgBusqueda.Text.ToLower();

            //Con los TextBox (t)
            Globales.tAcceso = Int32.Parse(ttAcceso.Text);
            Globales.tEscritura = Int32.Parse(ttEscritura.Text);
            Globales.tLectura = Int32.Parse(ttLectura.Text);
            Globales.tProcesamiento = Int32.Parse(ttProcesamiento.Text);
            Globales.tSeek = Int32.Parse(ttSeek.Text);

            Globales.espacioInicial = Int32.Parse(tEspacioInicial.Text);
            Globales.tamBloque = Int32.Parse(ttamBloque.Text);
            Globales.tamDispositivo = Int32.Parse(ttamDisp.Text);

            this.Close();

            //Muestro en la pantalla principal utilizando Label la config seleccionada
            configuracionActual.Content = Globales.orgFisica + "\n" +
                Globales.admLibre + "\n" + Globales.orgFisica + "\n" + Globales.modoAcceso;
            //Esto en la version final no va a estar

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Boton cancelar cierro la ventana de config
            this.Close();
        }
    }
}
