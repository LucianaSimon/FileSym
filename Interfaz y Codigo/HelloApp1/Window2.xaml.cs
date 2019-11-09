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
using System.Windows.Shapes;

namespace HelloApp1
{
    /// <summary>
    /// Lógica de interacción para Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
            //Prueba para cargar texto a info_resultados
            info_resultados.Text = "Informacion general final\n* Fragmentación interna total: 0\n" +
                "* Fragmentación externa: 0\n" + "* Nivel de aprovechamiento: % datos de usuario y % de gestion\n" +
                "* % tiempo consumido en gestion\n" +
                "* Otros tiempos";

            grafico.RefreshData(63.2); //para actualizar el grafico torta
            
        }

        //Exportar
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Con los resultados obtenidos de la simulacion se crea un File
            //con un nombre generico? y se almacenan los indicadores para cada operacion.
        }

        //Salir
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
