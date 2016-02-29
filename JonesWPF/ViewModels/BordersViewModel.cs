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
                    OneFileReader.SetBorders(startX, endX, startY, endY);
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
                    OneFileReader.SetBorders(startX, endX, startY, endY);
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
                    OneFileReader.SetBorders(startX, endX, startY, endY);
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
                    OneFileReader.SetBorders(startX, endX, startY, endY);
                }
            }
        }
        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            //Debug.WriteLine($"{StartX},{EndX},{StartY},{EndY}");
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
