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
    public class HelpViewModel : ViewModel
    {
        private HelpView _viewInstance;

        public ICommand OkButton { get; private set; }

        private bool OkButtonPressed = false;

        public HelpViewModel(UserControl control) : base(control)
        {
            OkButton = new DelegateCommand(OkButtonHandler);
            control.DataContext = this;
            _viewInstance = (HelpView)control;
        }

        public void OkButtonHandler()
        {
            OkButtonPressed = true;
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {
            //Only update this view if visible

            if (VMReturnData.HelpWindow_Active)
            {
                this.View.Visibility = Visibility.Visible;
                VMReturnData.NavigationBar_Active = false;
                HelpText = VMReturnData.HelpMessage;
                if (OkButtonPressed)
                {
                    OkButtonPressed = false;
                    VMReturnData.HelpWindow_Active = false;
                }
            }
            else
                this.View.Visibility = Visibility.Collapsed;

        }

        private string _HelpText;

        public string HelpText
        {
            get { return _HelpText; }
            set { _HelpText = value; RaisePropertyChanged("HelpText"); }
        }


    }
}
