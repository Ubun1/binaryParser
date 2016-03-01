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
    //one file Reader
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

        List<DataPoint> column;
        
        int index = 4;
        byte[] bytes;
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
            //TODO вернуть рабочий код через binReader!
            FastRead(path);

            token.ThrowIfCancellationRequested();

            ReadInitialInf();
            ReadMarkersInf();

            ProgressEnded(workerID, path);
           
            return column;
        }

        private void FastRead(string path)
        {
            var file = new FileInfo(path);
            var stream = new FileStream(path, FileMode.Open);
            bytes = new byte[file.Length];
            long numBytesToRead = file.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                // Read may return anything from 0 to numBytesToRead.
                int n = stream.Read(bytes, numBytesRead, (int)numBytesToRead);

                // Break when the end of the file is reached.
                if (n == 0)
                    break;

                numBytesRead += n;
                numBytesToRead -= n;
            }
            stream.Close();
        }

        private void ReadInitialInf()
        {
            
            var xnumx = BitConverter.ToInt64(bytes, index);//  binReader.ReadInt64();
            index += 8;
            var ynumy = BitConverter.ToInt64(bytes, index);// binReader.ReadInt64();
            index += 8;
            var mnumx = BitConverter.ToInt64(bytes, index);// binReader.ReadInt64();
            index += 8;
            var mnumy = BitConverter.ToInt64(bytes, index); // binReader.ReadInt64();
            index += 8;
            marknum = BitConverter.ToInt64(bytes, index);// binReader.ReadInt64(); //количество маркеров
            index += 8;
            var xsize = BitConverter.ToDouble(bytes, index);// binReader.ReadDouble();
            index += 8;
            var ysize = BitConverter.ToDouble(bytes, index);// binReader.ReadDouble();
            index += 8;
            //var pinit = new double[5] { binReader.ReadDouble(), binReader.ReadDouble(), binReader.ReadDouble(), binReader.ReadDouble(), binReader.ReadDouble() };
            index += 8 * 5;
            var gxkoef = BitConverter.ToDouble(bytes, index);// binReader.ReadDouble();
            index += 8;
            var gykoef = BitConverter.ToDouble(bytes, index);// binReader.ReadDouble();
            index += 8;
            var rocknum = BitConverter.ToInt32(bytes, index);// binReader.ReadInt32();
            index += 4;
            var bondnum = BitConverter.ToInt64(bytes, index);// binReader.ReadInt64();
            index += 8;
            var n1 = BitConverter.ToInt32(bytes, index);// binReader.ReadInt32();
            index += 4;
            time = (int)BitConverter.ToDouble(bytes, index);// binReader.ReadDouble();//время модели
            index += 8;
            var nodenum = xnumx * ynumy;
            index = (int)(4 + 2 * 4 + 16 * 8 + rocknum * (8 * 24 + 4) + 15 * 8 * nodenum + 4 * (xnumx + ynumy) + (bondnum - 1) * (16 + 3 * 8));

        }
        /// <summary>
        ///     В .prn файле начиная с позиции curpus0 
        ///     считываются значения связанные с маркерами.
        /// </summary>
        private void ReadMarkersInf()
        {
            for (long id = 0; id < marknum; id++)
            {
                int[] buffer = new int[9];
                for (int innerIndex = 0; innerIndex < 9; innerIndex++)
                {
                    buffer[innerIndex] = (int)BitConverter.ToSingle(bytes, index);
                    index += 4;
                }
                index++;

                int x = buffer[0];
                int y = buffer[1];
                int temperature = buffer[2];

                if (x > _startX && x < _endX && y > _startY && y < _endY)
                {
                    column.Add(new DataPoint((int)id, temperature, (int)time, x, y));
                }
                if (id % 1000000 == 0)
                {
                    int a =(int)(id / 28e6 * 100);
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
