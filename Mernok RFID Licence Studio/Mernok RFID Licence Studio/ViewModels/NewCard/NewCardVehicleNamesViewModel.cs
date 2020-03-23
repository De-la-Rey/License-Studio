using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace Mernok_RFID_Licence_Studio
{
    public class NewCardVehicleNamesViewModel : ViewModel
    {
        private NewCardVehicleNamesView _viewInstance;
        bool OneTimeRead;
        private ObservableCollection<string> _vehicleList = new ObservableCollection<string>();
        private ObservableCollection<string> _DisplayVehicleList = new ObservableCollection<string>();
        public ICommand AddBtn { get; private set; }
        public ICommand RemoveBtn { get; private set; }      
        private bool AddbtnPressed = false;
        int index = 0;
        GeneralFunctions generalFunctions = new GeneralFunctions();

        public NewCardVehicleNamesViewModel(UserControl control) : base(control)
        {
            AddBtn = new DelegateCommand(AddbtnHandler);
            RemoveBtn = new DelegateCommand(RemoveBtnHandler);
            control.DataContext = this;
            _viewInstance = (NewCardVehicleNamesView)control;
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {

            if (VMReturnData.NewCardVNames_Active)
            {
                View.Visibility = Visibility.Visible;
                #region Navigationbar Details
                VMReturnData.ViewTitle = "New Card";
                VMReturnData.SubTitle = "Vehicle Name details";
                VMReturnData.CurrentPageNumber = 3;
                VMReturnData.TotalPageNumber = 4;
                VMReturnData.MenuButtonEnabled = Visibility.Collapsed;
                VMReturnData.HelpButtonEnabled = Visibility.Visible;
                //Only update this viewModel when this view is visible
                #endregion

                if (!OneTimeRead)
                {
                    VehicleInfoList = new ObservableCollection<string>();
                    DisplayVehicleList = new ObservableCollection<string>();
                    OneTimeRead = true;
                    VehicleName = "";
                    index = 0;
                }

                if (Bypassed)
                {
                    DisplayVehicleList = new ObservableCollection<string> { "All Vehicle Serials" };
                    ByEnabled = BtnAddEnabled = false;
                    VMReturnData.VMCardDetails.VehicleNames = Enumerable.Repeat("", 15).ToArray();
                    VMReturnData.VMCardDetails.ByPassBits = VMReturnData.VMCardDetails.ByPassBits | 0x04;
                    VMReturnData.NextButtonEnabled = true;
                }
                else
                {
                    DisplayVehicleList = VehicleInfoList;
                    for (int i = 0; i < index; i++)
                    {
                        VMReturnData.VMCardDetails.VehicleNames[i] = VehicleInfoList[i];
                    }
                    if(VehicleInfoList.Count<15 && VehicleName != "")
                    {
                        ByEnabled = BtnAddEnabled = true;
                    }
                    else
                    {
                        ByEnabled = BtnAddEnabled = false;
                    }
                    VMReturnData.NextButtonEnabled = VehicleInfoList.Count() > 0 ? true : false;
                    VMReturnData.VMCardDetails.ByPassBits = (uint)(VMReturnData.VMCardDetails.ByPassBits & ~0b00000100);
                }


                if (AddbtnPressed)
                {
                    if (index < 15 && VehicleName != "")
                    {
                        VMReturnData.VMCardDetails.VehicleNames[index] = VehicleName;
                        VehicleInfoList.Add(VehicleName);                        
                        index++;
                    }
                    else
                        BtnAddEnabled = false;

                    for (int i = index; i < 15; i++)
                    {
                        VMReturnData.VMCardDetails.VehicleNames[index] = null;
                    }

                    VehicleName = "";
                    AddbtnPressed = false;
                }

                if (RemovebtnPressed)
                {
                    RemovebtnPressed = false;
                    if (VehicleInfoList.Count() > 0)
                    {
                        BtnremoveEnabled = true;
                        index--;
                        VMReturnData.VMCardDetails.VehicleNames[index] = null;
                        VehicleInfoList.RemoveAt(VehicleInfoList.Count - 1);

                    }

                }

                if (VehicleInfoList.Count() > 0)
                {
                    BtnremoveEnabled = true;
                }
                else
                {
                    BtnremoveEnabled = false;
                }

            }
            else
            {
                //View is not visible, do not update
                //Stop any animations on this vieModel
                View.Visibility = Visibility.Collapsed;
                OneTimeRead = false;
            }
        }

        #region Binding properties
        public ObservableCollection<string> VehicleInfoList
        {
            get { return _vehicleList; }
            set { _vehicleList = value; RaisePropertyChanged("VehicleInfoList"); }
        }

        public ObservableCollection<string> DisplayVehicleList
        {
            get { return _DisplayVehicleList; }
            set { _DisplayVehicleList = value; RaisePropertyChanged("DisplayVehicleList"); }
        }

        private string _vehicleName;

        public string VehicleName
        {
            get { return _vehicleName; }
            set { _vehicleName = generalFunctions.StringConditioner(value); RaisePropertyChanged("VehicleName"); }
        }

        private bool _Bypassed;

        public bool Bypassed
        {
            get { return _Bypassed; }
            set { _Bypassed = value; RaisePropertyChanged("Bypassed"); }
        }

        private bool _ByEnabled;
        private bool _btnAddEnabled = true;
        private bool _BtnremoveEnabled;
        private bool RemovebtnPressed;

        public bool ByEnabled
        {
            get { return _ByEnabled; }
            set { _ByEnabled = value; RaisePropertyChanged("VehicleTypenum"); }
        }

        public bool BtnAddEnabled
        {
            get
            {
                return _btnAddEnabled;
            }
            private set
            {
                _btnAddEnabled = value;
                RaisePropertyChanged("BtnAddEnabled");
            }
        }

        public bool BtnremoveEnabled
        {
            get
            {
                return _BtnremoveEnabled;
            }
            private set
            {
                _BtnremoveEnabled = value;
                RaisePropertyChanged("BtnremoveEnabled");
            }
        }
        #endregion

        public void AddbtnHandler()
        {
            AddbtnPressed = true;
        }

        private void RemoveBtnHandler()
        {
            RemovebtnPressed = true;
        }

        
    }



    public class VehicleNameInfoClass
    {
        private string _vehicleInfo;

        public string VehicleInfo
        {
            get { return _vehicleInfo; }
            set { _vehicleInfo = value; }
        }

    }
}
