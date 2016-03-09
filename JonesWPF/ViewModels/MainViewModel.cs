using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Forms;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using JonesWPF.View;

namespace JonesWPF.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public ICommand StartCommand { get; set; }
        public ICommand OpenCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SaveToCommand { get; set; }
        public ICommand SelectBoundaries { get; set; }
        public ICommand ConfigOutCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        #region PropsForUI
        private string mainTblk;
        public string MainTblk
        {
            get { return mainTblk; }
            set {
                if (mainTblk != value)
                {
                    mainTblk = value;
                    OnPropertyChanged("MainTblk");
                }
            }
        }

        private bool startButtEnable = false;

        private int totalCount;
        public int TotalCount
        {
            get { return totalCount; }
            set
            {
                if (totalCount != value)
                {
                    totalCount = value;
                    OnPropertyChanged("TotalCount");
                }
            }
        }
        

        private void logTextEventHandler(string message)
        {
            LogText += message;
        }
        private string logText;
        public string LogText
        {
            get { return logText; }
            set
            {
                if (logText != value)
                {
                    logText = value + "\n";
                    OnPropertyChanged("LogText");
                }
            }
        }


        private int firstThrdCount;
        public int FirstThrdCount
        {
            get { return firstThrdCount; }
            set
            {
                if (firstThrdCount != value)
                {
                    firstThrdCount = value;
                    OnPropertyChanged("FirstThrdCount");
                }
            }
        }

        private int secondThrdCount;
        public int SecondThrdCount
        {
            get { return secondThrdCount; }
            set
            {
                if (secondThrdCount != value)
                {
                    secondThrdCount = value;
                    OnPropertyChanged("SecondThrdCount");
                }
            }
        }

        private int thirdThrdCount;
        public int ThirdThrdCount
        {
            get { return thirdThrdCount; }
            set
            {
                if (thirdThrdCount != value)
                {
                    thirdThrdCount = value;
                    OnPropertyChanged("ThirdThrdCount");
                }
            }
        }

        private int fourthThrdCount;
        public int FourthThrdCount
        {
            get { return fourthThrdCount; }
            set
            {
                if (fourthThrdCount != value)
                {
                    fourthThrdCount = value;
                    OnPropertyChanged("FourthThrdCount");
                }
            }
        }

        private string firstTblk;
        public string FirstTblk
        {
            get { return firstTblk; }
            set
            {
                if (firstTblk != value)
                {
                    firstTblk = value;
                    OnPropertyChanged("FirstTblk");
                }
            }
        }

        private string secondTblk;
        public string SecondTblk
        {
            get { return secondTblk; }
            set
            {
                if (secondTblk != value)
                {
                    secondTblk = value;
                    OnPropertyChanged("SecondTblk");
                }
            }
        }

        private string thirdTblk;
        public string ThirdTblk
        {
            get { return thirdTblk; }
            set
            {
                if (thirdTblk != value)
                {
                    thirdTblk = value;
                    OnPropertyChanged("ThirdTblk");
                }
            }
        }

        private string fouthTblk;
        public string FouthTblk
        {
            get { return fouthTblk; }
            set
            {
                if (fouthTblk != value)
                {
                    fouthTblk = value;
                    OnPropertyChanged("FouthTblk");
                }
            }
        }

        #endregion

        FolderBrowserDialog loadFolderBrowser, saveFolderBrowser;
        CancellationTokenSource tokenSource;
        List<DataPoint> Column;
        List<string> directories;

        public MainViewModel()
        {
            Initialize();

            StartCommand = new RelayCommand(arg => StartMethod(), arg => true);
            OpenCommand = new RelayCommand(arg => OpenMethod());
            CancelCommand = new RelayCommand(arg => CancelMethod());
            SaveToCommand = new RelayCommand(arg => SaveToMethod(), arg => directories == null ? false : true);
            SelectBoundaries = new RelayCommand(arg => SelectBoundariesMethod());
            ConfigOutCommand = new RelayCommand(arg => ConfigOutMethod());
            CloseCommand = new RelayCommand(arg => CloseMethod());
        }

        private void Initialize()
        {
            //loadFolderBrowser = new FolderBrowserDialog();
            Column = new List<DataPoint>();

            FileWriter.SomethingChanged += logTextEventHandler;
            Analyzer.SomethingChanged += logTextEventHandler;
            XmlConfigManger.SomethingChanged += logTextEventHandler;
        }

        private void OpenMethod()
        {
            loadFolderBrowser = new FolderBrowserDialog();
            loadFolderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
            if (loadFolderBrowser.ShowDialog().ToString() == "OK")
            {
                directories = FolderManager.GetFilesPaths(loadFolderBrowser.SelectedPath);
                LogText += "Selected directories";
                foreach (var directory in directories)
                {
                    LogText += directory.ToString();
                } 
            }
        }

        private void CancelMethod()
        {
                TotalCount = 0;
                tokenSource.Cancel();
            startButtEnable = true;
        }

        private void SaveToMethod()
        {
            saveFolderBrowser = new FolderBrowserDialog();
            saveFolderBrowser.RootFolder = Environment.SpecialFolder.Desktop;
            if (saveFolderBrowser.ShowDialog().ToString() == "OK")
            {
                FileWriter.SavingDirectory = saveFolderBrowser.SelectedPath;
                LogText += $"{FileWriter.SavingDirectory} folder is choozed for saving..";
                startButtEnable = true;
            }
        }

        private void SelectBoundariesMethod()
        {
            var selectBoundarWindow = new BordersForAnalyseView();
            selectBoundarWindow.ShowDialog();
        }

        private void ConfigOutMethod()
        {
            var configOutWindow = new ConfigurateOutputView();
            configOutWindow.ShowDialog();
        }

        private async void StartMethod()
        {
            startButtEnable = false;

            LogText = "Starting operation";



            tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            foreach (var directory in directories)
            {
                MainTblk = directory.Split(new char[] { '\\' }).Last();
                TotalCount = 0;

                FileWriter.SetOutFileName(directory);
                var filePaths = FolderManager.ChoozeFilesFrom(directory);

                for (int outerPathCount = 0; outerPathCount < filePaths.Count; outerPathCount += 1)
                {
                    var tasks = new List<Task<List<DataPoint>>>();
                    for (int innerPathCount = 0; innerPathCount < 1; innerPathCount++)
                    {
                        if (outerPathCount + innerPathCount >= filePaths.Count)
                        {
                            break;
                        }
                        string path = filePaths[outerPathCount + innerPathCount];
                        
                        var oneFileReader = new OneFileReader(innerPathCount);
                        //Подписка на события
                        oneFileReader.ProgressStarted += ProgressStarted;
                        oneFileReader.ProgressChanged += ProgressChanged;
                        oneFileReader.ProgressEnded += ProgressEnded;

                        var mTask = Task<List<DataPoint>>.Factory.StartNew(oneFileReader.Read, new { Path = path, Token = token }, token);
                        tasks.Add(mTask);
                    }

                    try
                    {
                        var result = await Task.WhenAll(tasks);

                        foreach (var item in result)
                        {
                            Column = Column.Concat(item).ToList();
                        }

                        TotalCount = (outerPathCount + 4) * 100 / filePaths.Count;
                    }
                    catch (OperationCanceledException)
                    {
                        LogText += "\nCancelled";
                    }
                    catch (Exception ex)
                    {
                        LogText += "\n" + ex.Message;
                    }
                }
                Column = Column.OrderBy(x => x.Id).ThenBy(x => x.Time).ToList();
                FileWriter.Write(Analyzer.TwoHumps(Column));
            }
            WorkComplited(true);
        }

        private void CloseMethod()
        {
            XmlConfigManger.SaveConfig();
        }

        private void ProgressChanged(int progress, int id)
        {
            switch (id)
            {
                case 0:
                    DispatchService.Invoke(() => FirstThrdCount = progress);
                    break;
                case 1:
                    DispatchService.Invoke(() => SecondThrdCount = progress);
                    break;
                case 2:
                    DispatchService.Invoke(() => ThirdThrdCount = progress);
                    break;
                case 3:
                    DispatchService.Invoke(() => FourthThrdCount = progress);
                    break;
                default:
                    break;
            }
        }

        private void ProgressStarted(int id, string path)
        {
            var name = path.Split(new char[] { '\\' }).Last();
            switch (id)
            {
                case 0:
                    DispatchService.Invoke(() => FirstTblk = name);
                    LogText += $"Started {name} ...";
                    break;
                case 1:
                    DispatchService.Invoke(() => SecondTblk = name);
                    LogText += $"Started {name} ..."; 
                    break;
                case 2:
                    DispatchService.Invoke(() => ThirdTblk = name);
                    LogText += $"Started {name} ...";
                    break;
                case 3:
                    DispatchService.Invoke(() => FouthTblk = name);
                    LogText += $"Started {name} ..."; 
                    break;
                default:
                    break;
            }
        }

        private void ProgressEnded(int id, string path)
        {
            var name = path.Split(new char[] { '\\' }).Last() + " complited...";
            switch (id)
            {
                case 0:
                    DispatchService.Invoke(() => FirstTblk = name);
                    LogText += $"{name}";
                    break;
                case 1:
                    DispatchService.Invoke(() => SecondTblk = name);
                    LogText += $"{name}";
                    break;
                case 2:
                    DispatchService.Invoke(() => ThirdTblk = name);
                    LogText += $"{name}";
                    break;
                case 3:
                    DispatchService.Invoke(() => FouthTblk = name);
                    LogText += $"{name}";
                    break;
                default:
                    break;
            }
        }

        private void WorkComplited(bool canseled)
        {
            //Action action = () =>
            //{
            //    string message = canseled ? "\n:(" : "\n:)";
            //    LogText += message;
            //};
            //DispatchService.Invoke(action);

            startButtEnable = true;

            TotalCount = 0;
            FirstThrdCount = 0;
            SecondThrdCount = 0;
            ThirdThrdCount = 0;
            FourthThrdCount = 0;

            MainTblk = "Operation complited. Select new model.";
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
