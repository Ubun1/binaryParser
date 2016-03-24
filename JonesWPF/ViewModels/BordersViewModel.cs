using System.ComponentModel;
using System;
using System.Windows.Input;

namespace JonesWPF.ViewModels
{
    class BordersViewModel : INotifyPropertyChanged
    {
        #region PropsForUI
        private int startX = 2300;
        public int StartX
        {
            get { return startX; }
            set
            {
                if (startX != value)
                {
                    startX = value;
                    OnPropertyChanged("StartX");
                }
            }
        }

        private int endX = 2800;
        public int EndX
        {
            get { return endX; }
            set {
                if (endX != value)
                {
                    endX = value;
                    OnPropertyChanged("EndX");
                }
            }
        }

        private int endY = 20;
        public int EndY
        {
            get { return endY; }
            set {
                if (endY != value)
                {
                    endY = value;
                    OnPropertyChanged("EndY");
                }
            }
        }
        #endregion

        public ICommand CloseCommand { get; set; }

        public BordersViewModel()
        {
            CloseCommand = new RelayCommand(arg => BordersChanged(startX, endX, endY));
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        public event Action<int, int, int> BordersChanged;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
