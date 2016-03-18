using System.ComponentModel;

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

        private int endX = 2900;
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

        private int startY = 0;
        public int StartY
        {
            get { return startY; }
            set {
                if (startY != value)
                {
                    startY = value;
                    OnPropertyChanged("StartY");
                }
            }
        }

        private int endY = 75;
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
