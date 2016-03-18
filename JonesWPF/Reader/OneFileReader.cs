using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace JonesWPF.Reader
{
    //TODO Баги - столбцы для вывода сохраняются только если вызывать соответствующее окно.
    class OneFileReader
    {
        //TODO Допилить выбор из этого окна. Он положил мне весь анализ.
        List<DataPoint> column;
        BinaryReader binReader;

        long marknum;
        int time;
        readonly int threadID;
        string path;

        public OneFileReader(int id, string path)
        {
            column = new List<DataPoint>();
            threadID = id;
            this.path = path;
        }

        public List<DataPoint> Read()
        {  
            ProgressStarted(threadID, path);

            var stream = new FileStream(path, FileMode.Open);
            binReader = new BinaryReader(stream);

            ReadInitialInf();
            ReadMarkersInf();

            ProgressEnded(threadID, path);

            binReader.Close();
            stream.Close();

            var rand = new Random();
            return column.OrderBy(arg => rand.Next()).Take(150000).ToList();
        }

        public event Action<int, string> ProgressStarted;
        public event Action<int, int> ProgressChanged;
        public event Action<int, string> ProgressEnded;

        private void ReadInitialInf()
        {
            var a = binReader.ReadInt32();
            var xnumx = binReader.ReadInt64();
            var ynumy =  binReader.ReadInt64();
            var mnumx = binReader.ReadInt64();
            var mnumy = binReader.ReadInt64();
            marknum = binReader.ReadInt64(); //количество маркеров
            var xsize = binReader.ReadDouble();
            var ysize =  binReader.ReadDouble();
            var pinit = new double[5] { binReader.ReadDouble(), binReader.ReadDouble(), binReader.ReadDouble(), binReader.ReadDouble(), binReader.ReadDouble() };
            var gxkoef =  binReader.ReadDouble();
            var gykoef = binReader.ReadDouble();
            var rocknum = binReader.ReadInt32();
            var bondnum = binReader.ReadInt64();
            var n1 = binReader.ReadInt32();
            time = (int)binReader.ReadDouble();//время модели
            var nodenum = xnumx * ynumy;
            binReader.BaseStream.Position = (int)(4 + 2 * 4 + 16 * 8 + rocknum * (8 * 24 + 4) + 15 * 8 * nodenum + 4 * (xnumx + ynumy) + (bondnum - 1) * (16 + 3 * 8));
        }
        private void ReadMarkersInf()
        {
            for (long id = 0; id < marknum; id++)
            {
                int[] buffer = new int[9];
                for (int innerIndex = 0; innerIndex < 9; innerIndex++)
                {
                    buffer[innerIndex] = (int)binReader.ReadSingle();
                }

                int rockType = binReader.ReadByte();
                int x = buffer[0];
                int y = buffer[1];
                int temperature = buffer[2] == 0 ? 273 : buffer[2];
                int density = buffer[3];
                int waterContent = buffer[4];
                int viscosuty = buffer[7];
                int relativeDeformation = buffer[8];

                if (IsSatisfyConditions(x, y, rockType))
                {
                    column.Add(new DataPoint()
                    {
                        Id = (int)id,
                        X = x,
                        Y = y,
                        Temperature = temperature,
                        Time = time,
                        Density = density,
                        RelativeDeformation = relativeDeformation,
                        RockType = rockType,
                        WaterContent = waterContent,
                        Viscosity = viscosuty
                    });
                }
                if (id % 1000000 == 0)
                {
                    Debug.WriteLine($"{x},{y}");
                    ProgressChanged((int)(id / 28e6 * 100), threadID);
                }
            } 
        }
        private bool IsSatisfyConditions(int x, int y, int rockType)
        {
            var coordinateConditions = x > 2300000 && x < 2800000 && y < 20000;
            //if (result == null)
            //{
            if (rockType > 1 && rockType < 7)
            {
                return coordinateConditions && true;
            }
            //    return false;
            //}
            return false;
        }
    }
}
