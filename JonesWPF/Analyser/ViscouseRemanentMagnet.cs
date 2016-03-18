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
        List<int> times = new List<int>();

        //TODO костыли с VRM нужно понять что тут реализовывать. Мадиана или среднее? и тд...
        protected override bool doConcreteAnalyse(DataPoint curDataPoint)
        {
            times.Add(curDataPoint.Time);
            var time = times.Count > 1 ? times.Last() - times.ElementAt(times.Count - 2) : times.Last();
            totalSquare += curDataPoint.Temperature * Math.Log(10e10 * time * secInYear);
            curDataPoint.VRM = (int)(totalSquare / criticalSquare);

            return totalSquare / criticalSquare > 2;
        }

        protected override void SetConcreteDefaults()
        {
            totalSquare = 0;
            times = new List<int>();
        }
    }
}
