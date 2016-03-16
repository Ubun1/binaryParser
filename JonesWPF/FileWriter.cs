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
        VRMtotal,
        Default
    }

    public static class FileWriter
    {
        static string outFileName;
        static List<CheckBox> selectedCheckBoxes;

        private static string savingDirectory;

        public static string SavingDirectory
        {
            get { return savingDirectory; }
            set
            {
                savingDirectory = value;
                SomethingChanged($"Saving directory {savingDirectory}");
            }
        }


        public static void ConfigurateReport(List<CheckBox> _selectedCheckBoxes)
        {
            selectedCheckBoxes = _selectedCheckBoxes;
            selectedCheckBoxes.Sort();
            SomethingChanged($"Report configurated, selected {_selectedCheckBoxes.Count}");
        }

        public static void SetOutFileName(string directory)
        {
            outFileName = directory.Split(new char[] { '\\' }).Last();
            SomethingChanged($"OutFileName selected - {outFileName}");
        }

        public static void Write(IEnumerable<DataPoint> datapoints)
        {
            var file = new StreamWriter($"{SavingDirectory}\\{outFileName}.txt");

            file.WriteLine(MakeHeaders());

            foreach (var datapoint in datapoints)
            {
                file.WriteLine(MakeLine(datapoint));
            }

            SomethingChanged($"file write complite");
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
                    case CheckBox.VRMtotal:
                        result += $"{dataPoint.VRM};";
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        public static event Action<string> SomethingChanged;
    }
}
