using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonesWPF
{
    public enum CheckBox
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

    public static class FileWriter
    {
        static string outFileName;
        static List<CheckBox> selectedCheckBoxes;

        public static string SavingDirectory { get; set; }

        public static void ConfigurateReport(List<CheckBox> _selectedCheckBoxes)
        {
            selectedCheckBoxes = _selectedCheckBoxes;
            selectedCheckBoxes.Sort();
        }

        public static void SetOutFileName(string directory)
        {
            outFileName = directory.Split(new char[] { '\\' }).Last();
        }

        public static void Write(List<DataPoint[]> result)
        {
            //var dataPoints = result.SelectMany(r => r.ToList());
            var file = new StreamWriter($"{SavingDirectory}\\{outFileName}.csv");

            file.WriteLine(MakeHeaders());

            foreach (var dataPoints in result)
            {
                foreach (var dataPoint in dataPoints)
                {
                    file.WriteLine(MakeLine(dataPoint));
                }
            }

            file.WriteLine("endOfFile");
            file.Close();
        }

        private static string MakeHeaders()
        {
            string result = "id;";
            foreach (var columnHead in selectedCheckBoxes)
            {
                result += $"{columnHead};";
            }
            return result;
        }

        private static string MakeLine(DataPoint dataPoint)
        {
            string result = $"{dataPoint.Id};";
            foreach (var item in selectedCheckBoxes)
            {
                switch (item)
                {
                    case CheckBox.X:
                        result += $"{dataPoint.X};";
                        break;
                    case CheckBox.Y:
                        result += $"{dataPoint.Y};";
                        break;
                    case CheckBox.Time:
                        result += $"{dataPoint.Time};";
                        break;
                    case CheckBox.Temp:
                        result += $"{dataPoint.Temperature};";
                        break;
                    case CheckBox.Density:
                        result += $"{dataPoint.Density};";
                        break;
                    case CheckBox.Water:
                        result += $"{dataPoint.WaterContent};";
                        break;
                    case CheckBox.Viscosity:
                        result += $"{dataPoint.Viscosity};";
                        break;
                    case CheckBox.RelativeDeformation:
                        result += $"{dataPoint.RelativeDeformation};";
                        break;
                    case CheckBox.RockType:
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
