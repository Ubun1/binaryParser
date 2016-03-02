using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace JonesWPF
{

    class OneFileReader
    {
        static int _startX, _endX, _startY, _endY;
        public static void SetBorders(int startX = 2300, int endX = 2900, int startY = 0, int endY = 75)
        {
            _startX = startX;
            _endX = endX;
            _startY = startY;
            _endY = endY;
            Debug.WriteLine($"{_startX},{_endX},{_startY},{_endY}");
        }
        private static bool IsInAnaliseWindow(int x, int y)
        {
            return x > _startX && x < _endX && y > _startY && y < _endY;
        }

        List<DataPoint> column;
        BinaryReader binReader;

        long marknum;
        int time;
        readonly int workerID;

        public OneFileReader(int id)
        {
            column = new List<DataPoint>();
            workerID = id;
        }

        public List<DataPoint> Read(dynamic obj)
        {      
            string path = obj.Path;
            CancellationToken token = obj.Token;

            ProgressStarted(workerID, path);

            var stream = new FileStream(path, FileMode.Open);
            binReader = new BinaryReader(stream);
            token.ThrowIfCancellationRequested();

            ReadInitialInf();
            ReadMarkersInf();

            ProgressEnded(workerID, path);

            binReader.Close();
            stream.Close();

            return column;
        }

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
                int temperature = buffer[2];
                int density = buffer[3];
                int waterContent = buffer[4];
                int viscosuty = buffer[7];
                int relativeDeformation = buffer[8];

                if (IsInAnaliseWindow(x, y))
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
                    int a = (int)(id / 28e6 * 100);
                    Debug.WriteLine($"{Task.CurrentId} id - {id}, {a}");
                    ProgressChanged((int)(id / 28e6 * 100), workerID);
                }
            }
        }

        public event Action<int, string> ProgressStarted;
        public event Action<int, int> ProgressChanged;
        public event Action<int, string> ProgressEnded;
    }
}
