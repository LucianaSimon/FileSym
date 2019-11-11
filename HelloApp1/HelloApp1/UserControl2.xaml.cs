using System;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace HelloApp1
{
    /// <summary>
    /// Interaction logic for BasicStackedRowPercentageExample.xaml
    /// </summary>
    public partial class UserControl2 : UserControl
    {
        public UserControl2()
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
                    Title = "UA de Datos",
                    LabelPoint = p => p.X.ToString()
                },

                new StackedRowSeries
                {
                    Values = new ChartValues<double> {0},
                    Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#0ddbac")),
                    StackMode = StackMode.Percentage,
                    DataLabels = true,
                    Title = "UA de Metadatos",
                    LabelPoint = p => p.X.ToString()
                }
            };

            Labels = new[] { "Datos y metadatos"};
            Formatter = val => val.ToString("P");

            DataContext = this;
        }

        internal void RefreshData(double datos, double metadatos)
        {
            this.SeriesCollection[0].Values[0] = datos;
            this.SeriesCollection[1].Values[0] = metadatos;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

    }
}
