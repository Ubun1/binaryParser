using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonesWPF
{
    //TODO подумать над именами
    enum SelectedReportInf
    {
        X,
        Y,
        Time,
        Temp,
        Density,
        Water,
        Viscosity,
        RelativeDeformation,
        RockType,
        Default
    }

    static class FileWriter
    {
        static string outFileName;
        static List<SelectedReportInf> Config;

        public static string SavingDirectory { get; set; }

        public static void ConfigurateReport(List<SelectedReportInf> list)
        {
            Config = list;
            Config.Sort();
        }

        public static void SetOutFileName(string directory)
        {
            outFileName = directory.Split(new char[] { '\\' }).Last();
        }

        public static void Write(List<DataPoint[]> result)
        {
            var dataPoints = result.SelectMany(r => r.ToList());
            var file = new StreamWriter($"{SavingDirectory}\\{outFileName}.csv");

            file.WriteLine(MakeHeaders());

            foreach (var dataPoint in dataPoints)
            {
                file.WriteLine(MakeLine(dataPoint));
            }

            file.WriteLine("endOfFile");
            file.Close();
        }

        private static string MakeHeaders()
        {
            string result = "id;";
            foreach (var columnHead in Config)
            {
                result += $"{columnHead};";
            }
            return result;
        }

        private static string MakeLine(DataPoint dataPoint)
        {
            string result = $"{dataPoint.Id};";
            foreach (var item in Config)
            {
                switch (item)
                {
                    case SelectedReportInf.X:
                        result += $"{dataPoint.X};";
                        break;
                    case SelectedReportInf.Y:
                        result += $"{dataPoint.Y};";
                        break;
                    case SelectedReportInf.Time:
                        result += $"{dataPoint.Time};";
                        break;
                    case SelectedReportInf.Temp:
                        result += $"{dataPoint.Temperature};";
                        break;
                    case SelectedReportInf.Density:
                        result += $"{dataPoint.Density};";
                        break;
                    case SelectedReportInf.Water:
                        result += $"{dataPoint.WaterContent};";
                        break;
                    case SelectedReportInf.Viscosity:
                        result += $"{dataPoint.Viscosity};";
                        break;
                    case SelectedReportInf.RelativeDeformation:
                        result += $"{dataPoint.RelativeDeformation};";
                        break;
                    case SelectedReportInf.RockType:
                        result += $"{dataPoint.RockType};";
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
}
