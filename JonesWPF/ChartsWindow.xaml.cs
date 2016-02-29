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
using Chart2DControl;
using LineCharts;


namespace JonesWPF
{
    /// <summary>
    /// Логика взаимодействия для Chart.xaml
    /// </summary>
    public partial class ChartsWindow : Window, IChart
    {
        ChartStyleGridlines cs;
        DataCollection dc;
        DataSeries ds;
        List<DataPoint> interpolatedDataForDisplay;

        public ChartsWindow()
        {
            InitializeComponent();
            Initialize();
        }
 
        public void SetDataToVisualise(List<DataPoint> dataToVisualize)
        {
            interpolatedDataForDisplay = dataToVisualize;
        }
        public void Add()
        {
            Initialize();
            SetGraphDesight();
            MakeLines();
            AddLinesToGraph();
        }
        public void Resize()
        {
            textCanvas.Width = chartGrid.ActualWidth;
            textCanvas.Height = chartGrid.ActualHeight;
            chartCanvas.Children.Clear();
            textCanvas.Children.RemoveRange(1, textCanvas.Children.Count - 1);
            Add();
        }
        private void Initialize()
        {
            cs = new ChartStyleGridlines();
            dc = new DataCollection();
            ds = new DataSeries();
        }
        private void SetGraphDesight()
        {
            cs.ChartCanvas = chartCanvas;
            cs.TextCanvas = textCanvas;
            
            SetLabelNames();
            CustomizeAxis();
            CustomizeGridLines();
            CustomizeLines();
            cs.AddChartStyle(tbTitle, tbXLabel, tbYLabel);
        }
        private void SetLabelNames()
        {
            cs.Title = "Chart";
            cs.XLabel = "Time";
            cs.YLabel = "Temperature";
        }
        private void CustomizeAxis()
        {
            cs.Xmin = interpolatedDataForDisplay[0].Time;
            cs.Xmax = interpolatedDataForDisplay[interpolatedDataForDisplay.Count - 1].Time;
            cs.Ymin = 0;
            cs.Ymax = 4500;
        }
        private void CustomizeGridLines()
        {
            cs.YTick = 600;
            cs.XTick = 10000000;
            cs.GridlinePattern = ChartStyleGridlines.GridlinePatternEnum.DashDot;
            cs.GridlineColor = Brushes.Black;

        }
        private void CustomizeLines()
        {
            ds.LineColor = Brushes.Red;
            ds.LinePattern = DataSeries.LinePatternEnum.Solid;
        }
        private void MakeLines()
        {
            foreach (var point in interpolatedDataForDisplay)
            {
                ds.LineSeries.Points.Add(new System.Windows.Point(point.Time, point.Temperature));
            }
            dc.DataList.Add(ds);

        }
        private void AddLinesToGraph()
        {
            dc.AddLines(cs);
        }

        private void chartGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Resize();
        }

    }
}
