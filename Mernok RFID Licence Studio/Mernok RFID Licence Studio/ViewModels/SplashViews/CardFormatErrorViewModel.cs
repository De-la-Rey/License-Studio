using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mernok_RFID_Licence_Studio
{
    class CardFormatErrorViewModel : ViewModel
    {
        private CardFormatError _viewInstance;

        public ICommand OkButton { get; private set; }
        public ICommand RetryButton { get; private set; }

        private bool OkButtonPressed = false;
        private bool RetryButtonPressed = false;

        public CardFormatErrorViewModel(UserControl control) : base(control)
        {
            OkButton = new DelegateCommand(OkButtonHandler);
            RetryButton = new DelegateCommand(RetryButtonHandler);
            control.DataContext = this;
            _viewInstance = (CardFormatError)control;
        }


        public override void Update(ViewModelReturnData VMReturnData)
        {
            //Only update this view if visible

            if (VMReturnData.CardFormatError)
            {
                this.View.Visibility = Visibility.Visible;
                VMReturnData.NavigationBar_Active = false;

                if (OkButtonPressed)
                {
                    VMReturnData.NewCardAccess_Active = true;
                    OkButtonPressed = false;
                    VMReturnData.CardFormatError = false;
                }

                if(RetryButtonPressed)
                {
                    RetryButtonPressed = false;
                    VMReturnData.CardFormatError = false;
                }
            }
            else
                this.View.Visibility = Visibility.Collapsed;

        }
        public void OkButtonHandler()
        {
            OkButtonPressed = true;
        }

        public void RetryButtonHandler()
        {
            RetryButtonPressed = true;
        }
    }

}
