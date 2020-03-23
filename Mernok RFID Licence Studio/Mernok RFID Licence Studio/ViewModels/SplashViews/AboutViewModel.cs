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
    class AboutViewModel : ViewModel
    {

        private AboutView _viewInstance;

        public ICommand OkButton { get; private set; }

        private bool OkButtonPressed = false;

        public AboutViewModel(UserControl control) : base(control)
        {
            OkButton = new DelegateCommand(OkButtonHandler);
            control.DataContext = this;
            _viewInstance = (AboutView)control;
        }

        public void OkButtonHandler()
        {
            OkButtonPressed = true;
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {
            //Only update this view if visible

            if (VMReturnData.AboutWindow_Active)
            {
                this.View.Visibility = Visibility.Visible;
                VMReturnData.NavigationBar_Active = false;

                if (OkButtonPressed)
                {
                    OkButtonPressed = false;
                    VMReturnData.AboutWindow_Active = false;
                }
            }
            else
                this.View.Visibility = Visibility.Collapsed;

        }
    }
}
