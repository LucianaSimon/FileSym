using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using FireSim;

namespace HelloApp1
{
    /// <summary>
    /// Pantalla principal de simulacion
    /// Lógica de interacción para Window1.xaml 
    /// </summary>
    public partial class Window1 : Window
    {
        FileSim simulador = new FileSim(Globales.tProcesamiento, Globales.orgFisica, Globales.admLibre, Globales.modoAcceso,
            Globales.tLectura, Globales.tEscritura, Globales.tSeek, Globales.tAcceso, Globales.tamBloque, Globales.tamDispositivo, Globales.rutaArchivo);

        int pagActual = 0;
        int opActual = 0; //este atributo quedara reemplazada x simulador.GetContadorOp

        DispatcherTimer timer = new DispatcherTimer(); //para poder ejecutar operaciones x tiempo
        
        public Window1()
        {
            InitializeComponent();
            //Cada cierto tiempo se presiona el boton siguiente operación
            timer.Tick += (s, ev) => btn_SiguientePaso.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            LEjecutando.Content = "Esperando usuario";
            Spinner.Spin = false;

            lvDataBinding.ItemsSource = simulador.getTablaOperaciones();

            dibujarBloques(pagActual); //por defecto al inicio dibuja la primer pagina (130 o menos bloques)

            //informacion adicional cambiante por cada operacion simulada
            info_adicional.Text = "A rellenar con indicadores!";
                
        }

        //@toDO - Revisar dibujado de bloques de manera dinámica.
        //Tambien se debe poder mostrar los bloques que estan siendo modificados por la operación actual.
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
                    int numBloque = j + i * 10 + pagina * 130;  //numero de bloque a dibujar

                    if (numBloque < simulador.GetDispositivo().GetCantBloques()) //si corresponde dibujar el bloque
                    {
                        Rectangle bloque = new Rectangle();
                        
                        if(simulador.GetDispositivo().estadoBloque(numBloque) == 0) bloque.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd433")); //LIBRE
                        else if(simulador.GetDispositivo().estadoBloque(numBloque) == 1) bloque.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF1181A1")); //OCUPADO
                        else bloque.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#0ddbac"));

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
        {
            //dibuja una pagina antes
            if (pagActual == (simulador.getTablaBloques().Length / 130))
            {

            }
            else
            {
                pagActual++;
                dibujarBloques(pagActual);
            }
        }
        

        private void Button_Click_mostrarConfig(object sender, RoutedEventArgs e)
        {
            //Mostrar las configuraciones seleccionadas
            MessageBoxResult result = System.Windows.MessageBox.Show("Las configuraciones para la simulacion son:\n\n" +
                "* Organización física: " + Globales.orgFisica + "\n" +
                "* Modo de acceso: " + Globales.modoAcceso + "\n" +
                "* Administración de espacios libres: " + Globales.admLibre + "\n" +
                "* Tiempo de acceso: " + Globales.tAcceso + "\n" +
                "* Tiempo de seek: " + Globales.tSeek + "\n" +
                "* Tiempo de lectura: " + Globales.tLectura + "\n" +
                "* Tiempo de escritura: " + Globales.tEscritura + "\n" +
                "* Tiempo de procesamiento: " + Globales.tProcesamiento + "\n" +
                "* Tamaño de bloque: " + Globales.tamBloque + " uA" + "\n" +
                "* Tamaño de dispositivo: " + Globales.tamDispositivo + " uA" + "\n" +
                "* Cantidad de bloques: " + simulador.GetDispositivo().GetCantBloques(),
                "* Configuraciones seleccionadas", MessageBoxButton.OK);
        }


        private void Button_Click_PasoAPaso(object sender, RoutedEventArgs e)
        {
            tTiempoS.IsEnabled = true;

            if (!btn_Todo.IsEnabled)
            {
                btn_Todo.IsEnabled = true;
            }
            btn_PasoAPaso.IsEnabled = false;
            btn_SiguientePaso.IsEnabled = true;
            btn_P.IsEnabled = false; //No se puede pausar ya que es paso a paso!

            LEjecutando.Content = "Presione el botón de siguiente operación";
        }

        private void Button_Click_Todo(object sender, RoutedEventArgs e)
        {
            Spinner.Spin = true;
            tTiempoS.IsEnabled = false;

            btn_P.IsEnabled = true;
            btn_PasoAPaso.IsEnabled = false;
            btn_Todo.IsEnabled = false;
            btn_SiguientePaso.IsEnabled = false;

            timer.Interval = TimeSpan.FromSeconds(Int32.Parse(tTiempoS.Text));
            timer.Start();

            //TextBox de tiempo de simulacion
            tTiempoS.IsEnabled = false;

        }

        //boton de Pausar y Continuar
        private void btn_P_Click(object sender, RoutedEventArgs e)
        {
            if (btn_P.Content.Equals("Pausar"))
            {
                //Se presiona el boton pausar!
                btn_P.Content = "Continuar";
                tTiempoS.IsEnabled = true;
                btn_SiguientePaso.IsEnabled = false;
                btn_Todo.IsEnabled = false;
                btn_PasoAPaso.IsEnabled = true;
                timer.Stop();
            }
            else
            {
                btn_P.Content = "Pausar";
                tTiempoS.IsEnabled = false;
                btn_SiguientePaso.IsEnabled = false;
                btn_Todo.IsEnabled = false;
                btn_PasoAPaso.IsEnabled = false;

                timer.Interval = TimeSpan.FromSeconds(Int32.Parse(tTiempoS.Text));
                timer.Start();
            }
        }

        //boton de Siguiente Paso
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (btn_SiguientePaso.Content.Equals("Resultados"))
            {
                /*  Para la ventana de resultados!*/
                Window2 subWindow = new Window2();
                //this.WindowState = WindowState.Minimized;
                this.Topmost = false;
                subWindow.Show();
                subWindow.Activate();
                this.Close();
            }

            if (opActual < simulador.GetCantidadOp())
            {
                ejecutarSiguienteOp();
                //Tambien voy a tener que dibujar (mostrar) los bloques que esten siendo modificados
                //en la operacion actual.
            }

            if(opActual == simulador.GetCantidadOp())
            {
                LEjecutando.Content = "Simulación completa";
                btn_SiguientePaso.Content = "Resultados";
                timer.Stop();

                tTiempoS.IsEnabled = false;
                btn_SiguientePaso.IsEnabled = true;
                btn_Todo.IsEnabled = false;
                btn_PasoAPaso.IsEnabled = false;
                btn_P.IsEnabled = false;
            }

        }

        private void ejecutarSiguienteOp()
        {
            //Esta metodo llama a algun metodo de la clase FileSim que realizara la operacion
            //correspondiente, almacenando los resultados.
            //Por ahora solo aumenta una variable global que indica en que operacion estamos.
            Spinner.Spin = true;
            LEjecutando.Content = "Ejecutando operación " + (opActual + 1) + " de " + simulador.GetCantidadOp();

            //Tambien en la tabla tiene que mostrarse la operacion actual
            if(opActual < simulador.GetCantidadOp())
            {
                lvDataBinding.SelectedItem = lvDataBinding.Items[opActual];
                lvDataBinding.UpdateLayout(); // Pre-generates item containers 

                var listBoxItem = (ListBoxItem)lvDataBinding
                                            .ItemContainerGenerator
                                            .ContainerFromItem(lvDataBinding.SelectedItem);

                listBoxItem.Focus();
            }
            
            opActual++;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
