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
        List<CheckBox> selectedCheckBoxes;

        public ICommand CloseCommand { get; set; }
        public ICommand LoadedCommand { get; set; }

        public ConfigurateOutputViewModel()
        {
            Initialize();
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
                    OnPropertyChanged(nameof(TimeIsChecked), CheckBox.Time);
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
                    OnPropertyChanged(nameof(TempIsChecked), CheckBox.Temp);
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
                    OnPropertyChanged(nameof(XIsChecked), CheckBox.X);
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
                    OnPropertyChanged(nameof(YIsChecked), CheckBox.Y);
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
                    OnPropertyChanged(nameof(DensityIsChecked), CheckBox.Density);
                }
            }
        }

        private bool waterContentIsChecked;
        public bool WaterContentIsChecked
        {
            get { return waterContentIsChecked; }
            set {
                if (waterContentIsChecked != value)
                {
                    waterContentIsChecked = value;
                    OnPropertyChanged(nameof(WaterContentIsChecked), CheckBox.Water);
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
                    OnPropertyChanged(nameof(ViscosityIsChecked), CheckBox.Viscosity);
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
                    OnPropertyChanged(nameof(RelativeDefIsChecked), CheckBox.RelativeDeformation);
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
                    OnPropertyChanged(nameof(RockTypeIsChecked), CheckBox.RockType);
                }
            }
        }

        private bool vrmIsChecked;
        public bool VRMisChecked
        {
            get { return vrmIsChecked; }
            set {
                if (vrmIsChecked != value)
                {
                    vrmIsChecked = value;
                    OnPropertyChanged(nameof(VRMisChecked), CheckBox.VRMtotal);
                }
            }
        }


        #endregion

        private void CloseMethod()
        {
            FileWriter.ConfigurateReport(selectedCheckBoxes);
            XmlConfigManger.AddInfToConfig(selectedCheckBoxes);
        }

        private void Initialize()
        {
            selectedCheckBoxes = XmlConfigManger.ReadInitialCheckBoxes();

            timeIsChecked = selectedCheckBoxes.Exists(arg => arg == CheckBox.Time);
            tempIsChecked = selectedCheckBoxes.Exists(arg => arg == CheckBox.Temp);
            xIsChecked = selectedCheckBoxes.Exists(arg => arg == CheckBox.X);
            yIsChecked = selectedCheckBoxes.Exists(arg => arg == CheckBox.Y);
            waterContentIsChecked = selectedCheckBoxes.Exists(arg => arg == CheckBox.Water);
            viscosityIsChecked = selectedCheckBoxes.Exists(arg => arg == CheckBox.Viscosity);
            densityIsChecked = selectedCheckBoxes.Exists(arg => arg == CheckBox.Density);
            rockTypeIsChecked = selectedCheckBoxes.Exists(arg => arg == CheckBox.RockType);
            relativeDefIsChecked = selectedCheckBoxes.Exists(arg => arg == CheckBox.RelativeDeformation);
            vrmIsChecked = selectedCheckBoxes.Exists(arg => arg == CheckBox.VRMtotal);
        }

        private void OnPropertyChanged(string propertyName, CheckBox checkBox)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                if (selectedCheckBoxes.Exists(arg => arg == checkBox))
                {
                    selectedCheckBoxes.Remove(checkBox);
                }
                else
                {
                    selectedCheckBoxes.Add(checkBox);
                }   
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
