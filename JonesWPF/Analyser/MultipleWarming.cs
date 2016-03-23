using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonesWPF.Analyser
{
    public class MultipleWarming : Analyser
    {
        #region SingletonRealisation
        static MultipleWarming uniqueObj;

        public static MultipleWarming Instance()
        {
            if (uniqueObj == null)
            {
                uniqueObj = new MultipleWarming();
                return uniqueObj;
            }
            return uniqueObj;
        }

        private MultipleWarming()
        {        }
        #endregion

        bool firsthumpreached = false;
        bool firsttroughtreached = false;
        bool secondtroughtreached = false;
        bool secondhumpreached = false;
        bool thirdTroughtReached = false;
        bool firstIter = false;

        public int TempCurie { get; set; } = 859;

        protected override void SetConcreteDefaults()
        {
            firsthumpreached = false;
            firsttroughtreached = false;
            secondhumpreached = false;
            secondtroughtreached = false;
            thirdTroughtReached = false;
            firstIter = false;
        }

        protected override bool doConcreteAnalyse(DataPoint curDataPoint)
        {
            if (thirdTroughtReached)
            {
                return true;
            }
            //первый -
            if (!firsttroughtreached &&
                curDataPoint.Temperature < TempCurie)
            {
                firstIter = true;
                return false;
            }
            //первый +
            if (!firsthumpreached && firstIter &&
                curDataPoint.Temperature > TempCurie)
            {
                firsttroughtreached = true;
                return false;
            }
            //второй -
            if (!secondtroughtreached &&
                curDataPoint.Temperature < TempCurie)
            {
                firsthumpreached = true;
                return false;
            }
            //второй +
            if (!secondhumpreached &&
                curDataPoint.Temperature > TempCurie)
            {
                secondtroughtreached = true;
                return false;
            }
            //третий -
            if (curDataPoint.Temperature < TempCurie)
            {
                thirdTroughtReached = true;
                return true;
            }
            return false;
        }
    }

}
