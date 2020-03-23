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
    class ExitPromptViewModel : ViewModel
    {
        private ExitPromptView _viewInstance;

        public ICommand OkButton { get; private set; }

        public ICommand NoButton { get; private set; }

        private bool OkButtonPressed = false;

        private bool NoButtonPressed = false;

        public ExitPromptViewModel(UserControl control) : base(control)
        {
            OkButton = new DelegateCommand(OkButtonHandler);
            NoButton = new DelegateCommand(NoButtonHandler);
            control.DataContext = this;
            _viewInstance = (ExitPromptView)control;
        }

        public void OkButtonHandler()
        {
            OkButtonPressed = true;
        }

        public void NoButtonHandler()
        {
            NoButtonPressed = true;
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {
            //Only update this view if visible

            if (VMReturnData.ExitPromtView_Active)
            {
                this.View.Visibility = Visibility.Visible;
                VMReturnData.NavigationBar_Active = false;

                if (OkButtonPressed)
                {
                    OkButtonPressed = false;
                    Application.Current.Shutdown();
                }

                if (NoButtonPressed)
                {
                    NoButtonPressed = false;
                    VMReturnData.ExitPromtView_Active = false;
                }
            }
            else
                this.View.Visibility = Visibility.Collapsed;

        }


    }
}
