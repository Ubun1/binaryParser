using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonesWPF.Analyser
{
    public abstract class Analyser
    {
        protected List<DataPoint> output;
        protected List<DataPoint> dpsWithSameIds;
        protected bool IsAnalyseComplite = false;

        public IEnumerable<DataPoint> doAnalyse(IEnumerable<DataPoint> datapoints)
        {
            var innerCollection = datapoints.OrderBy(x => x.Id).ThenBy(x => x.Time).ToList();

            output = new List<DataPoint>();
            SetDefauls();

            for (int curIndex = 0; curIndex < innerCollection.Count - 1; curIndex++)
            {
                var curDataPoint = innerCollection[curIndex];
                var nextDataPoint = innerCollection[curIndex + 1];

                dpsWithSameIds.Add(curDataPoint);
                IsAnalyseComplite = doConcreteAnalyse(curDataPoint);

                if (curDataPoint.Id != nextDataPoint.Id)
                {
                    if (IsAnalyseComplite)
                    {
                        output.AddRange(dpsWithSameIds);
                    }
                    SetDefauls();
                    continue;
                }
            }

            return output;
        }
        protected event Action<string> SomethingChanged;

        protected abstract void SetConcreteDefaults();

        protected abstract bool doConcreteAnalyse(DataPoint curDataPoint);

        private void SetDefauls()
        {
            SetConcreteDefaults();
            IsAnalyseComplite = false;
            dpsWithSameIds = new List<DataPoint>();
        }
    }
}
