using Microsoft.Win32;
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
    class RuleTemplateViewModel : ViewModel
    {
        private RuleTemplateView _viewInstance;
        #region Properties
        private String _ruleTitle;
        public String RuleTitle
        {
            get { return _ruleTitle; }
            set
            {
                _ruleTitle = value;
                RaisePropertyChanged("RuleTitle");
            }
        }

        private String _viewTitle;
        public String ViewTitle
        {
            get { return _viewTitle; }
            set
            {
                _viewTitle = value;
                RaisePropertyChanged("ViewTitle");
            }
        }

        private String _nextTitle;
        public String NextTitle
        {
            get { return _nextTitle; }
            set
            {
                _nextTitle = value;
                RaisePropertyChanged("NextTitle");
            }
        }

        private String _previousTitle;
        public String PreviousTitle
        {
            get { return _previousTitle; }
            set
            {
                _previousTitle = value;
                RaisePropertyChanged("PreviousTitle");
            }
        }

        private int _pageProgress;
        public int PageProgress
        {
            get { return _pageProgress; }
            set
            {
                _pageProgress = value;
                RaisePropertyChanged("PageProgress");
            }
        }

        private int _currentPageNumber;
        public int CurrentPageNumber
        {
            get { return _currentPageNumber; }
            set
            {
                _currentPageNumber = value;
                RaisePropertyChanged("CurrentPageNumber");
            }
        }

        private int _totalPageNumber;
        public int TotalPageNumber
        {
            get { return _totalPageNumber; }
            set
            {
                _totalPageNumber = value;
                RaisePropertyChanged("TotalPageNumber");
            }
        }

        Visibility _NextVisible = Visibility.Hidden;
        public Visibility NextVisible
        {
            get
            {
                return _NextVisible;
            }
            set { _NextVisible = value; base.RaisePropertyChanged("NextVisible"); }

        }

        Visibility _PreviousVisible = Visibility.Hidden;
        public Visibility PreviousVisible
        {
            get
            {
                return _PreviousVisible;
            }
            set { _PreviousVisible = value; base.RaisePropertyChanged("PreviousVisible"); }
        }

        Visibility _MenuVisable = Visibility.Hidden;
        public Visibility MenuVisable
        {
            get
            {
                return _MenuVisable;
            }
            set { _MenuVisable = value; base.RaisePropertyChanged("MenuVisable"); }
        }

        Visibility _HelpVisable = Visibility.Hidden;
        public Visibility HelpVisable
        {
            get
            {
                return _HelpVisable;
            }
            set { _HelpVisable = value; base.RaisePropertyChanged("HelpVisable"); }
        }

        #endregion
        #region Button Init
        public ICommand ButtonNextPage { get; private set; }
        public ICommand ButtonPreviousPage { get; private set; }
        public ICommand ButtonBack { get; private set; }
        public ICommand ButtonRuleMenu { get; private set; }
        public ICommand ButtonHelp { get; private set; }

        private bool _buttonNextPagePressed = false;
        private bool _buttonPreviousPagePressed = false;
        private bool _buttonBackPressed = false;
        private bool _buttonRuleMenuPressed = false;
        private bool _buttonHelpPressed = false;
        #endregion

        public RuleTemplateViewModel(UserControl control) : base(control)
        {
            ButtonNextPage = new DelegateCommand(Button_Next_Page_Handler);
            ButtonPreviousPage = new DelegateCommand(Button_Previous_Page_Handler);
            ButtonBack = new DelegateCommand(Button_Back_Handler);
            ButtonRuleMenu = new DelegateCommand(Button_Rule_Menu_Handler);
            ButtonHelp = new DelegateCommand(Button_Help_Handler);

            control.DataContext = this;
            _viewInstance = (RuleTemplateView)control;
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {
            if(VMReturnData.NavigationBar_Active)
            {
                View.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                View.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (this.View.Visibility == System.Windows.Visibility.Visible)
            {

                //RuleTitle = VMReturnData.Rule_Managers.Complete_Group_info_List.ElementAt(_currentPageNumber).Group_Name;
                //TotalPageNumber = VMReturnData.Rule_Managers.PD_Rule_List.Count;

                ViewTitle = VMReturnData.ViewTitle;
                RuleTitle = VMReturnData.SubTitle;
                PageProgress = VMReturnData.NavBarProgress;
                CurrentPageNumber = VMReturnData.CurrentPageNumber;
                TotalPageNumber = VMReturnData.TotalPageNumber;
                PageProgress = (int)(((float)CurrentPageNumber / (float)(TotalPageNumber)) * 100);

                NextVisible = (Visibility)Convert.ToInt16(!VMReturnData.NextButtonEnabled);
                PreviousVisible = (Visibility)Convert.ToInt16(!VMReturnData.BackButtonEnabled);
                MenuVisable = VMReturnData.MenuButtonEnabled;
                HelpVisable = VMReturnData.HelpButtonEnabled;

                if (_buttonBackPressed)
                {
                    _buttonBackPressed = false;
                    VMReturnData.BackApp();
                   
                    //VMReturnData.Temp_Page_Number = 0;
                    //VMReturnData.View_Changed = true;
                    //VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.Main_Menu_View_Selected] = true;
                    //VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.PDRules_View_Selected] = false;
                    //VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.System_Specific_View_Selected] = false;
                    //VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.Rule_Template_View_Selected] = false;
                }

                else if (_buttonNextPagePressed && VMReturnData.NextButtonEnabled)
                {
                    _buttonNextPagePressed = false;
                    VMReturnData.NextWindow();
                    //VMReturnData.Temp_Page_Number++;
                    //VMReturnData.View_Changed = true;

                    //if (VMReturnData.Temp_Page_Number == VMReturnData.Rule_Managers.PD_Rule_List.Count && VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.PDRules_View_Selected])
                    //{
                    //    VMReturnData.Temp_Page_Number = 0;
                    //    VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.PDRules_View_Selected] = false;
                    //    VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.System_Specific_View_Selected] = true;
                    //}
                    //else if (VMReturnData.Temp_Page_Number == VMReturnData.Rule_Managers.PD_Rule_List.Count && VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.System_Specific_View_Selected])
                    //{
                    //    VMReturnData.Temp_Page_Number = 0;
                    //    VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.PDRules_View_Selected] = true;
                    //    VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.System_Specific_View_Selected] = false;
                    //}
                }

                else if (_buttonPreviousPagePressed && VMReturnData.BackButtonEnabled)
                {
                    _buttonPreviousPagePressed = false;
                    VMReturnData.BackWindow();
                    //VMReturnData.Temp_Page_Number--;
                    //VMReturnData.View_Changed = true;

                    //if (VMReturnData.Temp_Page_Number < 0 && VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.PDRules_View_Selected])
                    //{
                    //    VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.PDRules_View_Selected] = false;
                    //    VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.System_Specific_View_Selected] = true;
                    //    VMReturnData.Temp_Page_Number = VMReturnData.Rule_Managers.PD_Rule_List.Count - 1;
                    //}
                    //else if (VMReturnData.Temp_Page_Number < 0 && VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.System_Specific_View_Selected])
                    //{
                    //    VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.PDRules_View_Selected] = true;
                    //    VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.System_Specific_View_Selected] = false;
                    //    VMReturnData.Temp_Page_Number = VMReturnData.Rule_Managers.PD_Rule_List.Count - 1;
                    //}

                }

                else if (_buttonRuleMenuPressed)
                {
                    _buttonRuleMenuPressed = false;
                    VMReturnData.MenuButton();
                    //VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.Rule_Menu_View_Selected] = true;

                }
                else if(_buttonHelpPressed)
                {
                    _buttonHelpPressed = false;
                    VMReturnData.HelpButton();
                }

                //if (VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.PDRules_View_Selected])
                //{
                //    ViewTitle = "Vehicle to Vehicle Rules";

                //}
                //if (VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.System_Specific_View_Selected])
                //{
                //    ViewTitle = "Vehicle to Static Rules";
                //}


                //    if (VMReturnData.Temp_Page_Number == 0)
                //    {
                //        if (VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.PDRules_View_Selected])
                //        {
                //            PreviousTitle = "Vehicle to Vehicle Rules";
                //            NextTitle = VMReturnData.Rule_Managers.Complete_Group_info_List.ElementAt(VMReturnData.Temp_Page_Number + 2).Group_Name;

                //        }
                //        if (VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.System_Specific_View_Selected])
                //        {
                //            PreviousTitle = "Vehicle to Static Rules";
                //            NextTitle = VMReturnData.Rule_Managers.Complete_Group_info_List.ElementAt(VMReturnData.Temp_Page_Number + 2).Group_Name;
                //        }
                //    }
                //    else if (VMReturnData.Temp_Page_Number == VMReturnData.Rule_Managers.PD_Rule_List.Count)
                //    {
                //        if (VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.PDRules_View_Selected])
                //        {
                //            NextTitle = "Vehicle to Vehicle Rules";
                //            PreviousTitle = VMReturnData.Rule_Managers.Complete_Group_info_List.ElementAt(VMReturnData.Temp_Page_Number).Group_Name;

                //        }
                //        if (VMReturnData.View_Selected[(int)ViewModelReturnData.View_Selected_Enum.System_Specific_View_Selected])
                //        {
                //            NextTitle = "Vehicle to Static Rules";
                //            PreviousTitle = VMReturnData.Rule_Managers.Complete_Group_info_List.ElementAt(VMReturnData.Temp_Page_Number).Group_Name;
                //        }
                //    }
                //    else
                //    {
                //        NextTitle = VMReturnData.Rule_Managers.Complete_Group_info_List.ElementAt(VMReturnData.Temp_Page_Number + 2).Group_Name;
                //        PreviousTitle = VMReturnData.Rule_Managers.Complete_Group_info_List.ElementAt(VMReturnData.Temp_Page_Number).Group_Name;
                //    }

                //    CurrentPageNumber = VMReturnData.Temp_Page_Number + 1;
                //    PageProgress = (int)(((float)CurrentPageNumber / (float)(VMReturnData.Rule_Managers.PD_Rule_List.Count)) * 100);

                }
                else
                {
                    //View is not visible, do not update
                    //Stop any animations on this vieModel
                }
            }

            #region Button Handlers
            private void Button_Next_Page_Handler()
        {
            _buttonNextPagePressed = true;
        }

        private void Button_Previous_Page_Handler()
        {
            _buttonPreviousPagePressed = true;
        }


        private void Button_Back_Handler()
        {
            _buttonBackPressed = true;
        }

        private void Button_Rule_Menu_Handler()
        {
            _buttonRuleMenuPressed = true;
        }

        private void Button_Help_Handler()
        {
            _buttonHelpPressed = true;
        }
        #endregion
    }
}
