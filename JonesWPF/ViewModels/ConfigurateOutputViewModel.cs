using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace JonesWPF.ViewModels
{
    class ConfigurateOutputViewModel : INotifyPropertyChanged
    {
        #region PropsForUI

        private bool timeIsChecked = true;
        public bool TimeIsChecked
        {
            get { return timeIsChecked; }
            set {
                if (timeIsChecked != value)
                {
                    timeIsChecked = value;
                    OnPropertyChanged(nameof(TimeIsChecked));
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
