using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mernok_RFID_Licence_Studio
{
    class NewMernokCardViewModel : ViewModel
    {

        private NewMernokCardView _viewInstance;
        public NewMernokCardViewModel(UserControl control) : base(control)
        {

            control.DataContext = this;
            _viewInstance = (NewMernokCardView)control;
        }


        public override void Update(ViewModelReturnData VMReturnData)
        {
            //Only update this view if visible

            if (VMReturnData.MernokCardPresent_active)
            {
                RFIDCardInfoRead rFIDCardInfoRead = new RFIDCardInfoRead();
                this.View.Visibility = Visibility.Visible;
                if (VMReturnData.CardInField)
                {
                    VMReturnData.NewMernokUID = VMReturnData.UID;
                    VMReturnData.NewMernokCard = true;
                    VMReturnData.NewCardWindow = 1;
                    VMReturnData.NavigationBar_Active = true;
                    VMReturnData.NewCardDetail_Active = true;
                    VMReturnData.MernokCardPresent_active = false;
                    VMReturnData.AdvancedMenu_Active = false;
                }                    
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
