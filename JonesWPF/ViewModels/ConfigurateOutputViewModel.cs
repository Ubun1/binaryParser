using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;

namespace JonesWPF.ViewModels
{
    class ConfigurateOutputViewModel : INotifyPropertyChanged
    {
        //TODO подумать над именем этого массива!
        List<SelectedReportInf> arr;

        public ICommand CloseCommand { get; set; }

        public ConfigurateOutputViewModel()
        {
            arr = new List<SelectedReportInf>();

            CloseCommand = new RelayCommand(arg => CloseMethod());
        }

        #region PropsForUI

        private bool timeIsChecked;
        public bool TimeIsChecked
        {
            get { return timeIsChecked; }
            set {
                if (timeIsChecked != value)
                {
                    timeIsChecked = value;
                    OnPropertyChanged(nameof(TimeIsChecked), timeIsChecked ? SelectedReportInf.Time : SelectedReportInf.Default);
                }
            }
        }

        private bool tempIsChecked;
        public bool TempIsChecked
        {
            get { return tempIsChecked; }
            set {
                if (tempIsChecked != value)
                {
                    tempIsChecked = value;
                    OnPropertyChanged(nameof(TempIsChecked), tempIsChecked ? SelectedReportInf.Temp : SelectedReportInf.Default);
                }
            }
        }

        private bool xIsChecked;
        public bool XIsChecked
        {
            get { return xIsChecked; }
            set {
                if (xIsChecked != value)
                {
                    xIsChecked = value;
                    OnPropertyChanged(nameof(XIsChecked), xIsChecked ? SelectedReportInf.X : SelectedReportInf.Default);
                }
            }
        }

        private bool yIsChecked;
        public bool YIsChecked
        {
            get { return yIsChecked; }
            set
            {
                if (yIsChecked != value)
                {
                    yIsChecked = value;
                    OnPropertyChanged(nameof(YIsChecked), yIsChecked ? SelectedReportInf.Y : SelectedReportInf.Default);
                }
            }
        }

        #endregion

        private void CloseMethod()
        {
            FileWriter.ConfigurateReport(arr);
        }

        private void OnPropertyChanged(string propertyName, SelectedReportInf selectedReprotInf)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                arr.Add(selectedReprotInf);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
