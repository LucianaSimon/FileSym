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
using LiveCharts;
using LiveCharts.Wpf;

namespace HelloApp1
{
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();

            myPieChart.Series.Add(new PieSeries { Title = "Dato [UA]", Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff422c")), StrokeThickness = 0, Values = new ChartValues<double> { 100.0 } });
            myPieChart.Series.Add(new PieSeries { Title = "Metadato [UA]", Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd433")), StrokeThickness = 0, Values = new ChartValues<double> { 0.0 } });

            DataContext = this;

        }

        internal void RefreshData(double dato, double metadato)
        {
            myPieChart.Series[0].Values[0] = dato;
            myPieChart.Series[1].Values[0] = metadato;
        }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}
