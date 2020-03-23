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
    class IssuerCardPrompViewModel : ViewModel
    {
        private IssuerCardPrompView _viewInstance;

        public ICommand OkButton { get; private set; }
        public ICommand RetryButton { get; private set; }

        private bool OkButtonPressed = false;
        private bool RetryButtonPressed = false;

        public IssuerCardPrompViewModel(UserControl control) : base(control)
        {
            OkButton = new DelegateCommand(OkButtonHandler);
            RetryButton = new DelegateCommand(RetryButtonHandler);
            control.DataContext = this;
            _viewInstance = (IssuerCardPrompView)control;
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {
            //Only update this view if visible

            if (VMReturnData.CardStillIssuer_Active)
            {
                this.View.Visibility = Visibility.Visible;
                VMReturnData.NavigationBar_Active = false;

                if (OkButtonPressed)
                {

                    OkButtonPressed = false;
                    VMReturnData.CardStillIssuer_Active = false;

                    if (VMReturnData.CardInfoWrite.WriteInfoToCard(VMReturnData.VMCardDetails, VMReturnData.CardType) == 100)
                    {
                        VMReturnData.CardProramed_done = true;
                        VMReturnData.App_datareset();
                    }
                    else
                    {
                        VMReturnData.CardProgramFail = true;
                    }
                }
                if (RetryButtonPressed)
                {
                    RetryButtonPressed = false;
                    VMReturnData.CardStillIssuer_Active = false;
 //                   VMReturnData.App_datareset();
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
