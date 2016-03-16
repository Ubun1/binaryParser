using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JonesWPF.Analyser;
using System.Linq;
using JonesWPF;
namespace UnitTests
{
    [TestClass]
    public class AnalyserTester
    {
        [TestMethod]
        public void TwoHumpsTest()
        {
            var dataPoints = MakeTestCollectionForTH();
            Analyser analyser = TwoHumps.Instance();
            var expected = dataPoints.Take(dataPoints.Count - 1).ToList().Last();
            var real = analyser.doAnalyse(dataPoints).ToList().Last();
            Assert.AreEqual(expected, real);
        }

        [TestMethod]
        public void VRMtest()
        {
            var dataPoints = MakecollcectionForVRM();
            Analyser analyser = ViscouseRemanentMagnet.Instance();
            var real = analyser.doAnalyse(dataPoints).ToList().Last();
            var expected = dataPoints.Take(dataPoints.Count - 1).ToList().Last();
            Assert.AreEqual(real, expected);
        }

        private List<DataPoint> MakeTestCollectionForTH()
        {
            var testList = new List<DataPoint>();
            for (int i = 0; i < 10; i++)
            {
                var dp = new DataPoint();
                dp.Id = 0;
                dp.Density = i;
                dp.Time = i * 100;
                dp.Temperature = i % 2 == 0 ? 800 : 900;

                testList.Add(dp);
            }
            testList.Add(new DataPoint() { Id = 3 });
            return testList;
        }

        private List<DataPoint> MakecollcectionForVRM()
        {
            var testList = new List<DataPoint>();
            for (int i = 0; i < 10; i++)
            {
                var dp = new DataPoint();
                dp.Id = 0;
                dp.Time = (int)(i * 2e5);
                dp.Temperature = 750;
                testList.Add(dp);
            }
            testList.Add(new DataPoint() { Id = 2 });
            return testList;
        }
    }
}
