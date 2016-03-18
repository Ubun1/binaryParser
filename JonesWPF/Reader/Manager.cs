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
        static List<Task<List<DataPoint>>> tasks;

        public static event Action<int> TotalProgressChanhed;

        public static async Task<List<DataPoint>> StartRead(List<string> filePaths, int threadsCount)
        {
            int totalProgress = 0;
            for (totalProgress = 0; totalProgress < filePaths.Count; totalProgress += threadsCount)
            {
                var pathsForThreads = filePaths.Skip(totalProgress).Take(threadsCount).ToList();
                ConfigurateReaders(pathsForThreads);
            }
            var result = await Task.WhenAll(tasks);

            TotalProgressChanhed(totalProgress * 100 / filePaths.Count);

            return result.SelectMany(arg => arg.Select(dp => dp)).ToList();
        }

        //TODO передать сюда ссылку на класс обработчик главного окна и внем подписать события чтения на методы обработчики переданного класа.
        private static void ConfigurateReaders(List<string> pathsForThreads)
        {
            var threadsCount = pathsForThreads.Count;
            if (threadsCount > 5)
            {
                throw new ArgumentOutOfRangeException("threadsCount", "не может быть больше 4");
            }

            tasks = new List<Task<List<DataPoint>>>(threadsCount);

            for (int id = 0; id < threadsCount; id++)
            {
                var fileReader = new OneFileReader(id, pathsForThreads[id]);

                var task = new Task<List<DataPoint>>(fileReader.Read);
                task.Start();
                tasks.Add(task);

                //fileReader.ProgressStarted += viewModel.ProgressStarted;
                //fileReader.ProgressChanged += viewModel.ProgressChanged;
                //fileReader.ProgressEnded += viewModel.ProgressEnded;
            }
        }
    }
}
