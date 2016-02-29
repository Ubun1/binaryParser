using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonesWPF
{
    static class FileWriter
    {
        static string outFileName;

        public static string SavingDirectory { get; set; }
        
        public static void SetOutFileName(string directory)
        {
            outFileName = directory.Split(new char[] { '\\' }).Last();
        }
        public static void Write(List<DataPoint[]> result)
        {
            StreamWriter file = new StreamWriter($"{SavingDirectory}\\{outFileName}.txt");

            foreach (var column in result)
            {
                file.WriteLine("Time\tTemp\tX\tY");
                foreach (var dataPoint in column)
                {
                    file.WriteLine(string.Format("{0},{1},{2},{3}", dataPoint.Time, dataPoint.Temperature, dataPoint.X, dataPoint.Y));
                }
            }
            file.WriteLine("endOfFile");
            file.Close();
        }
    }
}
