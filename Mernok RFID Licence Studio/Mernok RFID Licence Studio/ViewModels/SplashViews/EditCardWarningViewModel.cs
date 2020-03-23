using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mernok_RFID_Licence_Studio
{
    public class EditCardWarningViewModel : ViewModel
    {
        private EditCardWarningView _viewInstance;



        public EditCardWarningViewModel(UserControl control) : base(control)
        {

            control.DataContext = this;
            _viewInstance = (EditCardWarningView)control;
        }


        public override void Update(ViewModelReturnData VMReturnData)
        {
            //Only update this view if visible

            if (VMReturnData.EditCardWarn_Active)
            {
                RFIDCardInfoRead rFIDCardInfoRead = new RFIDCardInfoRead();
                this.View.Visibility = Visibility.Visible;
                EditUID = rFIDCardInfoRead.UIDtoString(VMReturnData.EditCardUID);
            }
            else
                this.View.Visibility = Visibility.Collapsed;

        }

        private string _EditUID;

        public string EditUID
        {
            get { return _EditUID; }
            set { _EditUID = value; RaisePropertyChanged("EditUID"); }
        }

    }
}
