using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mernok_RFID_Licence_Studio
{
    public class NewCardWarningViewModel : ViewModel
    {

        private NewCardWarningView _viewInstance;


        public NewCardWarningViewModel(UserControl control) : base(control)
        {

            control.DataContext = this;
            _viewInstance = (NewCardWarningView)control;
        }


        public override void Update(ViewModelReturnData VMReturnData)
        {
            //Only update this view if visible

            if (VMReturnData.NewCardWarn_Active)
            {
                RFIDCardInfoRead rFIDCardInfoRead = new RFIDCardInfoRead();
                this.View.Visibility = Visibility.Visible;
                if (VMReturnData.NewIssuerCard)
                    NewcardUID = rFIDCardInfoRead.UIDtoString(VMReturnData.NewIssuerUID);
                else
                    NewcardUID = rFIDCardInfoRead.UIDtoString(VMReturnData.NewCardUID);
            }
            else
                this.View.Visibility = Visibility.Collapsed;

        }

        private string _NewcardUID;

        public string NewcardUID
        {
            get { return _NewcardUID; }
            set { _NewcardUID = value; RaisePropertyChanged("NewcardUID"); }
        }



    }
}
