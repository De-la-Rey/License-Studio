using RFID;
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
    class MenuViewModel : ViewModel
    {
        private MenuView _viewInstance;

        public ICommand AboutBtn { get; private set; }
   
        public ICommand IssuerCardBtn { get; private set; }

        public ICommand IssuerFileCardBtn { get; private set; }

        public ICommand Exitbtn { get; private set; }        

        public ICommand AdvancedSet { get; private set; }

        private bool AboutBtnPressed = false;

        private bool IssuerCardBtnPressed = false;

        private bool IssuerFileCardBtnPressed = false;

        private bool _AdvancedSet = false; // to do for creating mernok cards

        private bool _buttonExitPressed = false;


        public MenuViewModel(UserControl control) : base(control)
        {
            AboutBtn = new DelegateCommand(AboutBtnHandler);
            IssuerCardBtn = new DelegateCommand(IssuerCardBtnHandler);
            IssuerFileCardBtn = new DelegateCommand(IssuerFileCardBtnHandler);
            Exitbtn = new DelegateCommand(ExitBtnHandler);
            AdvancedSet = new DelegateCommand(AdvancedbtnHandler);
            control.DataContext = this;
            _viewInstance = (MenuView)control;
        }

        private void AdvancedbtnHandler()
        {
            _AdvancedSet = true;
        }

        private void IssuerFileCardBtnHandler()
        {
            IssuerFileCardBtnPressed = true;
        }

        public void AboutBtnHandler()
        {
            AboutBtnPressed = true;
        }

        public void IssuerCardBtnHandler()
        {
            IssuerCardBtnPressed = true;
        }

        private void ExitBtnHandler()
        {
            _buttonExitPressed = true;
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {
            //Only update this view if visible

            if (VMReturnData.MenuView_Active)
            {
                VMReturnData.OptionsPressed = true;
                this.View.Visibility = Visibility.Visible;
 //               VMReturnData.NavigationBar_Active = false;
                EditCardVis = VMReturnData.MenuEditBtnEnabled;
                IssueCardVis = VMReturnData.MenuIssueBtnEnabled;


                if (AboutBtnPressed)
                {
                    AboutBtnPressed = false;
                    VMReturnData.AboutWindow_Active = true;

                }



                if(_AdvancedSet)
                {
                    _AdvancedSet = false;
                    VMReturnData.AdvancedPWMenu_Active = true;

                }

                if (IssuerCardBtnPressed)
                {
                    IssuerCardBtnPressed = false;
                    VMReturnData.NewIssuerPresent_Active = true;
                    VMReturnData.NewIssuerCard = true;
                    VMReturnData.MenuView_Active = false;


                }

                if(IssuerFileCardBtnPressed)
                {
                    IssuerFileCardBtnPressed = false;
                    VMReturnData.NewCardIssuer_Active = true;
 //                   VMReturnData.MenuView_Active = false;
                }
                

                #region Exit button
                else if (_buttonExitPressed)
                {
                    _buttonExitPressed = false;
                    VMReturnData.MenuView_Active = false;
                }
                #endregion
            }
            else
            {
                VMReturnData.OptionsPressed = false;
                this.View.Visibility = Visibility.Collapsed;
            }
                

        }

        private Visibility _editCardVis;

        public Visibility EditCardVis
        {
            get { return _editCardVis; }
            set { _editCardVis = value; base.RaisePropertyChanged("EditCardVis"); }
        }

        private Visibility _issueCardVis;
        

        public Visibility IssueCardVis
        {
            get { return _issueCardVis; }
            set { _issueCardVis = value; base.RaisePropertyChanged("IssueCardVis"); }
        }

    }
}
