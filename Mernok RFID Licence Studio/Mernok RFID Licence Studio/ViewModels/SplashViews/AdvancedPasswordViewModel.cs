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
    class AdvancedPasswordViewModel : ViewModel
    {
        private AdvancedPasswordView _viewInstance;
        public ICommand OkButton { get; private set; }
        public ICommand RetryButton { get; private set; }

        private bool OkButtonPressed = false;
        private bool RetryButtonPressed = false;

        public AdvancedPasswordViewModel(UserControl control) : base(control)
        {
            OkButton = new DelegateCommand(OkButtonHandler);
            RetryButton = new DelegateCommand(RetryButtonHandler);
            control.DataContext = this;
            _viewInstance = (AdvancedPasswordView)control;
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {
            //Only update this view if visible

            if (VMReturnData.AdvancedPWMenu_Active)
            {
                this.View.Visibility = Visibility.Visible;
                VMReturnData.NavigationBar_Active = false;

                if (OkButtonPressed)
                {
                    OkButtonPressed = false;
                    if (MernokPW == "Mp123456")
                    {
                        VMReturnData.AdvancedMenu_Active = true;                       
                        VMReturnData.AdvancedPWMenu_Active = false;
                    }
                    else
                        MernokPW = "Wrong password, try again!";
                }

                if (RetryButtonPressed)
                {
                    RetryButtonPressed = false;
                    VMReturnData.AdvancedPWMenu_Active = false;
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

        private string _MernokPW;

        public string MernokPW
        {
            get { return _MernokPW; }
            set { _MernokPW = value; RaisePropertyChanged("MernokPW"); }
        }

    }
}
