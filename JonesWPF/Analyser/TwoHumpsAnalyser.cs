using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonesWPF.Analyser
{
    public class TwoHumps : Analyser
    {
        #region SingletonRealisation
        static TwoHumps uniqueObj;

        public static TwoHumps Instance()
        {
            if (uniqueObj == null)
            {
                uniqueObj = new TwoHumps();
                return uniqueObj;
            }
            return uniqueObj;
        }

        private TwoHumps()
        { }
        #endregion

        bool firsthumpreached = false;
        bool firsttroughtreached = false;
        bool secondtroughtreached = false;
        bool secondhumpreached = false;
        bool thirdTroughtReached = false;
        bool firstIter = false;

        int tempCurie = 859;

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
                curDataPoint.Temperature < tempCurie)
            {
                firstIter = true;
                return false;
            }
            //первый +
            if (!firsthumpreached && firstIter &&
                curDataPoint.Temperature > tempCurie)
            {
                firsttroughtreached = true;
                return false;
            }
            //второй -
            if (!secondtroughtreached &&
                curDataPoint.Temperature < tempCurie)
            {
                firsthumpreached = true;
                return false;
            }
            //второй +
            if (!secondhumpreached &&
                curDataPoint.Temperature > tempCurie)
            {
                secondtroughtreached = true;
                return false;
            }
            //третий -
            if (curDataPoint.Temperature < tempCurie)
            {
                thirdTroughtReached = true;
                return true;
            }
            return false;
        }
    }

}
