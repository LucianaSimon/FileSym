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
    /// Pantalla principal de simulacion
    /// Lógica de interacción para Window1.xaml 
    /// </summary>
    public partial class Window1 : Window
    {
        int cantBloques = 455; // (455/130) = 3.5 -> 3 [0,1,2,3]
        int pagActual = 0;

        public Window1()
        {
            InitializeComponent();
            
            
            //para la tabla de operaciones
            List<Operacion> operaciones = new List<Operacion>();
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 3, arribo = 12, offset = 50, archivo = "file.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 5, arribo = 0, offset = 50, archivo = "file1.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 6, arribo = 11, offset = 50, archivo = "file2.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 7, arribo = 50, offset = 50, archivo = "file3.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 13, arribo = 32, offset = 50, archivo = "file4.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 11, arribo = 20, offset = 50, archivo = "file5.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 2, arribo = 50, offset = 50, archivo = "file6.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 3, arribo = 12, offset = 50, archivo = "file.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 5, arribo = 0, offset = 50, archivo = "file1.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 6, arribo = 11, offset = 50, archivo = "file2.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 7, arribo = 50, offset = 50, archivo = "file3.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 13, arribo = 32, offset = 50, archivo = "file4.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 11, arribo = 20, offset = 50, archivo = "file5.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 2, arribo = 50, offset = 50, archivo = "file6.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 3, arribo = 12, offset = 50, archivo = "file.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 5, arribo = 0, offset = 50, archivo = "file1.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 6, arribo = 11, offset = 50, archivo = "file2.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 7, arribo = 50, offset = 50, archivo = "file3.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 13, arribo = 32, offset = 50, archivo = "file4.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 11, arribo = 20, offset = 50, archivo = "file5.txt" });
            operaciones.Add(new Operacion() { Nombre = "CREATE", n_proceso = 2, arribo = 50, offset = 50, archivo = "file6.txt" });
            lvDataBinding.ItemsSource = operaciones;

            dibujarBloques(pagActual); //por defecto al inicio dibuja la primer pagina (130 o menos bloques)

            //informacion adicional cambiante por cada operacion simulada
            info_adicional.Text = "prueba prueba" +
                "prueba pruebaprueba pruebaprueba pruebaprueba pruebaprueba pruebaprueba pruebaprueba pruebaprueba pruebaprueba pruebaprueba prueba" +
                "prueba pruebaprueba pruebaprueba pruebaprueba pruebaprueba pruebaprueba pruebaprueba pruebaprueba pruebaprueba pruebaprueba prueba";
        }

        public void dibujarBloques(int pagina)
        {
            //Antes de dibujar todos los cuadrados necesito poner en blanco canvas
            //Tamaño = 330 W x 350 H
            Rectangle rect = new Rectangle();
            rect.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF"));
            rect.Width = 330;
            rect.Height = 350;
            Canvas.SetTop(rect, 0);
            Canvas.SetLeft(rect, 0);
            miLienzo.Children.Add(rect);

            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Rectangle bloque = new Rectangle();
                    bloque.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd433")); //de que color? tengo que verificar con el bloque que corresponda
                    bloque.Width = 20;
                    bloque.Height = 20;
                    bloque.StrokeThickness = 1;
                    bloque.Stroke = Brushes.Black;
                    Canvas.SetTop(bloque, i * 25 + 25);
                    Canvas.SetLeft(bloque, j * 30 + 35);
                    miLienzo.Children.Add(bloque);

                    //Para dibujar los numeros del eje y
                    if (j == 0)
                    {
                        //Pagina = 9 bloques x 12 bloques = 117 bloques
                        TextBlock txt1 = new TextBlock();
                        txt1.FontSize = 14;
                        txt1.Text = Convert.ToString(i * 10 + pagina * 130);
                        Canvas.SetTop(txt1, i * 25 + 25);
                        Canvas.SetLeft(txt1, 0);
                        miLienzo.Children.Add(txt1);
                    }

                    //Para dibujar los numeros del eje x (0,1...,9)
                    if (i == 0)
                    {
                        TextBlock txt1 = new TextBlock();
                        txt1.FontSize = 14;
                        txt1.Text = Convert.ToString(j);
                        Canvas.SetTop(txt1, 0);
                        Canvas.SetLeft(txt1, j * 30 + 35);
                        miLienzo.Children.Add(txt1);
                    }
                }
            }
        }
            
	//Esta clase la habia hecho yo antes de que pensaramos en el diagrama de clase, la idea es usar FileSim (clase)
        public class Operacion
        {
            public string Nombre { get; set; }
            public int n_proceso { get; set; }
            public int arribo { get; set; }
            public int offset { get; set; }
            public string archivo { get; set; }

            public override string ToString()
            {
                return this.Nombre + ", " + this.n_proceso + " " + this.arribo + " " + this.offset + " Archivo: " + this.archivo;
            }
        }


        public class Bloque
        {
            public int numeroBloque { get; set; }
            public string estado { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window2 subWindow = new Window2();
            //this.WindowState = WindowState.Minimized;
            this.Topmost = false;
            subWindow.Show();
            subWindow.Activate();
            this.Close();
        }

        private void btn_P_Click(object sender, RoutedEventArgs e)
        {
            if (btn_P.Content.Equals("Pausar"))
            {
                btn_P.Content = "Continuar";
            }
            else
            {
                btn_P.Content = "Pausar";
            }
        }

        private void btn_izq(object sender, RoutedEventArgs e)
        {   //dibuja una pagina antes
            if(pagActual == 0)
            {

            }
            else
            {
                pagActual--;
                dibujarBloques(pagActual);
            }
            
        }

        private void btn_der(object sender, RoutedEventArgs e)
        {   //dibuja una pagina antes
            if (pagActual == (cantBloques/130))
            {

            }
            else
            {
                pagActual++;
                dibujarBloques(pagActual);
            }
        }
    }
}
