using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;

namespace Mernok_RFID_Licence_Studio
{

    public abstract class ViewModel : INotifyPropertyChanged
    {
        public ContentControl View { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ViewModel(ContentControl view)
        {
            this.View = view;
        }

        public abstract void Update(ViewModelReturnData VMReturnData);

    }
}