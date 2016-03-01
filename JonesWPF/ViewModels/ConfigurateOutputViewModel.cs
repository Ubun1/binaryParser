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
                    OnPropertyChanged(nameof(TimeIsChecked), SelectedReportInf.Time);
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
                    OnPropertyChanged(nameof(TempIsChecked), SelectedReportInf.Temp);
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
                    OnPropertyChanged(nameof(XIsChecked), SelectedReportInf.X);
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
                    OnPropertyChanged(nameof(YIsChecked), SelectedReportInf.Y);
                }
            }
        }

        private bool densityIsChecked;
        public bool DensityIsChecked
        {
            get { return densityIsChecked; }
            set {
                if (densityIsChecked != value)
                {
                    densityIsChecked = value;
                    OnPropertyChanged(nameof(DensityIsChecked), SelectedReportInf.Density);
                }
            }
        }

        private bool waterContentIsChecked;
        public bool WaterContentIsChecked
        {
            get { return waterContentIsChecked; }
            set {
                if (densityIsChecked != value)
                {
                    waterContentIsChecked = value;
                    OnPropertyChanged(nameof(WaterContentIsChecked), SelectedReportInf.Water);
                }
            }
        }

        private bool viscosityIsChecked;
        public bool ViscosityIsChecked
        {
            get { return viscosityIsChecked; }
            set
            {
                if (viscosityIsChecked != value)
                {
                    viscosityIsChecked = value;
                    OnPropertyChanged(nameof(ViscosityIsChecked), SelectedReportInf.Viscosity);
                }
            }
        }

        private bool relativeDefIsChecked;
        public bool RelativeDefIsChecked
        {
            get { return relativeDefIsChecked; }
            set
            {
                if (relativeDefIsChecked != value)
                {
                    relativeDefIsChecked = value;
                    OnPropertyChanged(nameof(RelativeDefIsChecked), SelectedReportInf.RelativeDeformation);
                }
            }
        }

        private bool rockTypeIsChecked;
        public bool RockTypeIsChecked
        {
            get { return rockTypeIsChecked; }
            set {
                if (rockTypeIsChecked != value)
                {
                    rockTypeIsChecked = value;
                    OnPropertyChanged(nameof(RockTypeIsChecked), SelectedReportInf.RockType);
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
                if (arr.Exists(arg => arg == selectedReprotInf))
                {
                    arr.Remove(selectedReprotInf);
                }
                else
                {
                    arr.Add(selectedReprotInf);
                }   
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
