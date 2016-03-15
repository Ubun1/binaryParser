using System.Windows;
using System;
using System.Linq;
using System.Collections.Generic;

namespace JonesWPF.Analiser
{
    interface IAnalisible
    {
        IEnumerable<DataPoint> Analise(List<DataPoint> datapoints);
    }

    public class Analyzer
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

    public class ViscouseRemanentMagnet : IAnalisible
    {
        #region SingletonRealisation
        static ViscouseRemanentMagnet uniqueObj;

        public static ViscouseRemanentMagnet Instance()
        {
            if (uniqueObj == null)
            {
                uniqueObj = new ViscouseRemanentMagnet();
                return uniqueObj;
            }
            return uniqueObj;
        }

        private ViscouseRemanentMagnet()
        { }
        #endregion

        double totalSquare = 0;
        bool IsAnaliseComplite = false;

        List<DataPoint> output;
        List<DataPoint> dpsWithSameIds;

        public IEnumerable<DataPoint> Analise(List<DataPoint> datapoints)
        {
            output = new List<DataPoint>();
            for (int curIndex = 1; curIndex < datapoints.Count; curIndex++)
            {
                var curDataPoint = datapoints[curIndex];
                var prevDatapoint = datapoints[curIndex - 1];

                if (curDataPoint.Id != prevDatapoint.Id)
                {
                    if (IsAnaliseComplite)
                    {
                        output.AddRange(dpsWithSameIds);
                    }
                    SetDefaults();
                    curIndex++;
                }
                dpsWithSameIds.Add(curDataPoint);
                IsAnaliseComplite = ViscouseRemamentAnalise(out totalSquare, curDataPoint, prevDatapoint);
            }
            return output as IEnumerable<DataPoint>;
        }

        private void SetDefaults()
        {
            totalSquare = 0;
            IsAnaliseComplite = false;
            dpsWithSameIds = new List<DataPoint>();
        }

        private bool ViscouseRemamentAnalise(out double totalSquare, DataPoint curDatapoint, DataPoint prevDataPoint)
        {
            //critcalSquare = TempCur * Lm (10^10 * (timeAtTempCur = 1))
            double criticalSquare = 19780;
            long timeInterval = curDatapoint.Time - prevDataPoint.Time;
            totalSquare = curDatapoint.Temperature * Math.Log(10e10 * timeInterval);
            return totalSquare / criticalSquare > 2;
        }
    }
}
