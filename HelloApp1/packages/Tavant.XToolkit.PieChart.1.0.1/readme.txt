
PieChart
=========

By Tavant Technologies Pvt Ltd.

PieChart works in two modes. First it can draw the pie chart using the data passed
in with ItemSource. Second, it can display a donut type chart. The donut width can 
be controlled as required.

If the pie chart does not reflect any modified data from the ItemSource, just hide and
show it so that it redraws itself. This could happen on Android.

NOTE:
======

You must call InitPieChart.Init() in AppDelegate and MainActivity.

USAGE:

PieChart pc = new PieChart();
List<PieChartData> pcd = new List<PieChartData>();
pcd.Add( new PieChartData() { Value=12, FillColor=Color.Blue } );
pcd.Add( new PieChartData() { Value=20, FillColor=Color.Green } );
pcd.Add( new PieChartData() { Value=34, FillColor=Color.Red } );

pc.ItemSource = pcd;

Alternatively, you can use PieChart in XAML with the ItemSource mapped to a property in your
view model.
