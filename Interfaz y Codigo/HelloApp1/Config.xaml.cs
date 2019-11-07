using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System;
using FireSim;

namespace HelloApp1
{
    static class Globales
    {
        public static Org orgFisica;
        public static Libres admLibre;
        public static Acceso modoAcceso;

        public static int tAcceso;
        public static int tSeek;
        public static int tLectura;
        public static int tEscritura;
        public static int tProcesamiento;
        
        public static int tamBloque;
        public static int tamDispositivo;

        public static string rutaArchivo;


        public static void Inicializar() //valores x defecto
        {
            orgFisica = Org.Vacio;
            admLibre = Libres.Vacio;
            modoAcceso = Acceso.Vacio;

            tAcceso = 10;
            tSeek = 10;
            tLectura = 10;
            tEscritura = 10;
            tProcesamiento = 10;

            tamBloque = 10;         //uA
            tamDispositivo = 1000;  //uA

            rutaArchivo = "";
        }
    }


    /// <summary>
    /// Lógica de interacción para Config.xaml
    /// </summary>
    public partial class Config : Window
    {
        public Config()
        {
            InitializeComponent();

            //Cada vez que se inicia la pantalla de configuracion se cargan los valores globales
            //La primera vez seran los valores por defecto. A medida que el usuario realice cambios 
            //estos serán persistidos.

            //Para los RadioButtons!
            if (Globales.orgFisica == Org.Contigua) Org_Contigua.IsChecked = true;
            else if (Globales.orgFisica == Org.Enlazada) Org_Enlazada.IsChecked = true;
            else if (Globales.orgFisica == Org.Indexada) Org_Indexada.IsChecked = true;
            else //No checkeo nada;

            if  (Globales.admLibre == Libres.ListadeLibres) Adm_Lista.IsChecked = true;
            else if (Globales.admLibre == Libres.ListadeLibresdePrincipioyCuenta) Adm_Cuenta.IsChecked = true ;
            else if (Globales.admLibre == Libres.MapadeBits) Adm_Mapa.IsChecked = true ;
            else //;

            if (Globales.modoAcceso == Acceso.Directo) Acceso_Directo.IsChecked = true ;
            else if (Globales.modoAcceso == Acceso.Indexado) Acceso_Indexado.IsChecked = true ;
            else if (Globales.modoAcceso == Acceso.Secuencial) Acceso_Secuencial.IsChecked = true ;
            else //;


            //Sliders (s)
            stAcceso.Value = Globales.tAcceso;
            stLectura.Value = Globales.tLectura;
            stEscritura.Value = Globales.tEscritura;
            stSeek.Value = Globales.tSeek;
            stProcesamiento.Value = Globales.tProcesamiento;

            ttamBloque.Text = Globales.tamBloque.ToString();
            ttamDisp.Text = Globales.tamDispositivo.ToString();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //Evento boton aceptar
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Se almacena la configuracion el las variables globales

            //Accedo a los RadioButtons
            //@toDO Mapeo para el constructor de FileSim

            if (Org_Contigua.IsChecked == true) Globales.orgFisica = Org.Contigua;
            else if (Org_Enlazada.IsChecked == true) Globales.orgFisica = Org.Enlazada;
            else if (Org_Indexada.IsChecked == true) Globales.orgFisica = Org.Indexada;
            else Globales.orgFisica = Org.Vacio;

            if (Adm_Lista.IsChecked == true) Globales.admLibre = Libres.ListadeLibres;
            else if (Adm_Cuenta.IsChecked == true) Globales.admLibre = Libres.ListadeLibresdePrincipioyCuenta;
            else if (Adm_Mapa.IsChecked == true) Globales.admLibre = Libres.MapadeBits;
            else Globales.admLibre = Libres.Vacio;

            if (Acceso_Directo.IsChecked == true) Globales.modoAcceso = Acceso.Directo;
            else if (Acceso_Indexado.IsChecked == true) Globales.modoAcceso = Acceso.Indexado;
            else if (Acceso_Secuencial.IsChecked == true) Globales.modoAcceso = Acceso.Secuencial;
            else Globales.modoAcceso = Acceso.Vacio;

            //Con los TextBox (t)
            Globales.tAcceso = Int32.Parse(ttAcceso.Text);
            Globales.tEscritura = Int32.Parse(ttEscritura.Text);
            Globales.tLectura = Int32.Parse(ttLectura.Text);
            Globales.tProcesamiento = Int32.Parse(ttProcesamiento.Text);
            Globales.tSeek = Int32.Parse(ttSeek.Text);

            Globales.tamBloque = Int32.Parse(ttamBloque.Text);
            Globales.tamDispositivo = Int32.Parse(ttamDisp.Text);

            this.Close();
        }

        //Boton cancelar
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Boton cancelar cierro la ventana de config
            this.Close();
        }

        //Manejo de RadioButton
        private void Org_Contigua_Checked(object sender, RoutedEventArgs e)
        {
            HabilitarRadioButtons();
            Acceso_Indexado.IsEnabled = false;
            Adm_Lista.IsEnabled = false;
        }

        private void Org_Enlazada_Checked(object sender, RoutedEventArgs e)
        {
            HabilitarRadioButtons();
            Acceso_Directo.IsEnabled = false;
            Acceso_Indexado.IsEnabled = false;
        }

        private void Org_Indexada_Checked(object sender, RoutedEventArgs e)
        {
            HabilitarRadioButtons();
            Acceso_Directo.IsEnabled = false;
            Acceso_Secuencial.IsEnabled = false;
        }

        private void HabilitarRadioButtons()
        {
            //Habilito los paneles de opciones de modo de acceso y adm de espacios libres
            PanelAcceso.IsEnabled = true;
            PanelAdm.IsEnabled = true;

            Acceso_Directo.IsChecked = false;
            Acceso_Directo.IsEnabled = true;
            Acceso_Secuencial.IsEnabled = true;
            Acceso_Secuencial.IsChecked = false;
            Acceso_Indexado.IsEnabled = true;
            Acceso_Indexado.IsChecked = false;

            Adm_Lista.IsEnabled = true;
            Adm_Lista.IsChecked = false;
            Adm_Cuenta.IsEnabled = true;
            Adm_Cuenta.IsChecked = false;
            Adm_Mapa.IsEnabled = true;
            Adm_Mapa.IsChecked = false;
        }
    }
}
