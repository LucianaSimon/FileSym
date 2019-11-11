using LiveCharts;
using LiveCharts.Wpf;
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
    /// Lógica de interacción para UserControl3.xaml
    /// </summary>
    public partial class UserControl3 : UserControl
    {
        public UserControl3()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection
            {
                new StackedRowSeries
                {
                    Values = new ChartValues<double> {2},
                    Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff422c")),
                    StackMode = StackMode.Percentage,
                    DataLabels = true,
                    Title = "bloques de F.Externa",
                    LabelPoint = p => p.X.ToString()
                },

                new StackedRowSeries
                {
                    Values = new ChartValues<double> {0},
                    Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#0ddbac")),
                    StackMode = StackMode.Percentage,
                    DataLabels = true,
                    Title = "otros bloques",
                    LabelPoint = p => p.X.ToString()
                }
            };
            Labels = new[] { "Fragmentación externa" };
            Formatter = val => val.ToString("P");

            DataContext = this;
        }

        //frag1 = cantidad de bloques de frag externa
        //frag2 = cantBloques - fragExterna
        internal void RefreshData(double frag1, double frag2)
        {
            this.SeriesCollection[0].Values[0] = frag1;
            this.SeriesCollection[1].Values[0] = frag2;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

    }

}

