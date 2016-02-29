using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using Chart2DControl;
using LineCharts;

namespace JonesWPF
{
    interface IChart
    {
        void Show();
        void SetDataToVisualise(List<DataPoint> dataToVisualize);
        void Add();
        void Resize();
    }

}
