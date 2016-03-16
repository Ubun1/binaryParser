using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonesWPF.Analyser
{
    public class ViscouseRemanentMagnet : Analyser
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

        const double secInYear = 3.154e+7;
        const double criticalSquare = 19780;

        double totalSquare = 0;
        int time = 1;
        List<int> temps;
        //TODO костыли с VRM нужно понять что тут реализовывать. Мадиана или среднее? и тд...
        protected override bool doConcreteAnalyse(DataPoint curDataPoint)
        {
            if (curDataPoint.Temperature < 859)
            {
                temps.Add(curDataPoint.Temperature);
                temps.Sort();

                var tempMedian = temps.ElementAt(temps.Count / 2);

                time += curDataPoint.Time;
                //critcalSquare = TempCur * Ln (10^10 * (timeAtTempCur = 1))
                totalSquare = tempMedian * Math.Log(10e10 * time * secInYear);
                curDataPoint.VRM = (int)(totalSquare / criticalSquare);
                return temps.Count > 2 ? totalSquare / criticalSquare > 2 : false;
            }
            else
            {
                return false;
            }

        }

        protected override void SetConcreteDefaults()
        {
            totalSquare = 0;
            time = 1;
            temps = new List<int>();
        }
   }
}
