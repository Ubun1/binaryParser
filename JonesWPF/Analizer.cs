using System.Windows;
using System;
using System.Collections.Generic;

namespace JonesWPF
{
    public static class Analyzer
    {
        public static List<DataPoint[]> TwoHumps(List<DataPoint> column)
        {
            int tempCurie = 859;

            DataPoint[] result;
            List<DataPoint[]> output = new List<DataPoint[]>();
            bool firsthumpreached = false;
            bool firsttroughtreached = false;
            bool secondtroughtreached = false;
            bool secondhumpreached = false;
            bool thirdTroughtReached = false;
            bool firstIter = false;
            int startMarker = 0;
            int endMarker = 0;

            for (int i = 1; i < column.Count; i++)
            {
                if (column[i - 1].Id != column[i].Id)
                {
                    if (thirdTroughtReached)
                    {
                        result = new DataPoint[endMarker - startMarker + 1];
                        column.CopyTo(startMarker, result, 0, result.Length);
                        output.Add(result);
                    }
                    firsthumpreached = false;
                    firsttroughtreached = false;
                    secondhumpreached = false;
                    secondtroughtreached = false;
                    thirdTroughtReached = false;
                    firstIter = false;
                    startMarker = i;
                    endMarker = i;
                }
                var point = column[i];
                //первый -
                if (!firsttroughtreached &&
                    point.Temperature < tempCurie)
                {
                    firstIter = true;
                    continue;
                }
                //первый +
                if (!firsthumpreached && firstIter &&
                    point.Temperature > tempCurie)
                {
                    firsttroughtreached = true;
                    continue;
                }
                //второй -
                if (!secondtroughtreached &&
                    point.Temperature < tempCurie)
                {
                    firsthumpreached = true;
                    continue;
                }
                //второй +
                if (!secondhumpreached && point.Temperature > tempCurie)
                {
                    secondtroughtreached = true;
                    continue;
                }
                //третий -
                if (point.Temperature < tempCurie)
                {
                    endMarker = i;
                    thirdTroughtReached = true;
                }
            }
            SomethingChanged($"Analize complite, result count = {output.Count}");
            return output;
        }

        public static event Action<string> SomethingChanged;
    }
}
