using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JonesWPF.ViewModels;

namespace JonesWPF.Reader
{
    public static class Manager
    {
        static double MaxWidth, MaxDepth, MinWidth;

        public static event Action<int> TotalProgressChanhed;

        public static void SetBorders(int maxWidth, int minWidth, int maxDepth)
        {
            var metersPerKm = 10e3;
            MaxDepth = maxDepth * metersPerKm;
            MinWidth = minWidth * metersPerKm;
            MaxWidth = maxWidth * metersPerKm;
        }

        public static async Task<List<DataPoint>> StartRead(List<string> filePaths, int threadsCount)
        {
            int totalProgress = 0;
            var datapoints = new List<DataPoint>();

            for (totalProgress = 0; totalProgress < filePaths.Count; totalProgress += threadsCount)
            {
                var pathsForThreads = filePaths.Skip(totalProgress).Take(threadsCount).ToList();
                var tasks = ConfigurateReaders(pathsForThreads);
                var taskskResult = await Task.WhenAll(tasks);

                datapoints.AddRange(taskskResult.SelectMany(arg => arg.Select(dp => dp)).ToList());

                TotalProgressChanhed(totalProgress * 100 / filePaths.Count);
            }

            return datapoints;
        }

        private static List<Task<List<DataPoint>>> ConfigurateReaders(List<string> pathsForThreads)
        {
            var threadsCount = pathsForThreads.Count;
            if (threadsCount > 5)
            {
                throw new ArgumentOutOfRangeException("threadsCount", "не может быть больше 4");
            }

            var tasks = new List<Task<List<DataPoint>>>(threadsCount);

            for (int id = 0; id < threadsCount; id++)
            {
                var fileReader = new OneFileReader(id, pathsForThreads[id]);
                fileReader.SetBorders(MaxWidth, MinWidth, MaxDepth);

                var task = new Task<List<DataPoint>>(fileReader.Read);
                task.Start();
                tasks.Add(task);

                fileReader.ProgressStarted += (threadId, path) => ProgressStarted(threadId, path);
                fileReader.ProgressChanged += (threadId, path) => ProgressChanged(threadId, path);
                fileReader.ProgressEnded += (threadId, path) => ProgressEnded(threadId, path);
            }

            return tasks;
        }
        public static event Action<int, string> ProgressStarted;
        public static event Action<int, int> ProgressChanged;
        public static event Action<int, string> ProgressEnded;
    }
}
