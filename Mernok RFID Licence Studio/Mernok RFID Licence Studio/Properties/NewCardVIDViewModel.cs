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
    public class NewCardVIDViewModel : ViewModel
    {
        private NewCardVIDView _viewInstance;

        private ObservableCollection<string> _vehicleList = new ObservableCollection<string>();
        private ObservableCollection<string> _DisplayVehicleList = new ObservableCollection<string>();

        GeneralFunctions generalFunctions = new GeneralFunctions();

        public ICommand AddBtn { get; private set; }
        public ICommand RemoveBtn { get; private set; }

        private bool AddbtnPressed = false;
        bool OneTimeRead;

        int index = 0;

        public NewCardVIDViewModel(UserControl control) : base(control)
        {
            AddBtn = new DelegateCommand(AddbtnHandler);
            RemoveBtn = new DelegateCommand(RemoveBtnHandler);
            VehicleID = "";
            control.DataContext = this;
            _viewInstance = (NewCardVIDView)control;
        }

        private void RemoveBtnHandler()
        {
            RemovebtnPressed = true;
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {

            if (VMReturnData.NewCardVID_Active)
            {
                this.View.Visibility = Visibility.Visible;
                //Only update this viewModel when this view is visible
                #region Navigation Bar details
                VMReturnData.ViewTitle = "New Card";
                VMReturnData.SubTitle = "Vehicle ID details";
                VMReturnData.CurrentPageNumber = 3;
                VMReturnData.TotalPageNumber = 4;
                VMReturnData.MenuButtonEnabled = Visibility.Collapsed;
                VMReturnData.HelpButtonEnabled = Visibility.Visible;
                #endregion

                if (!OneTimeRead)
                {
                    DisplayVehicleList = new ObservableCollection<string>();
                    VehicleIDInfoList = new ObservableCollection<string>();
                    OneTimeRead = true;
                    index = 0;
                }

                if (Bypassed)
                {
                    DisplayVehicleList = new ObservableCollection<string> { "All Vehicle Serials" };
                    ByEnabled = BtnAddEnabled = false;
                    VMReturnData.VMCardDetails.VID = Enumerable.Repeat(UInt16.MaxValue, 30).ToArray();
                    VMReturnData.VMCardDetails.ByPassBits = VMReturnData.VMCardDetails.ByPassBits | 0x04;
                    VMReturnData.NextButtonEnabled = true;
                }
                else
                {
                    DisplayVehicleList = VehicleIDInfoList;
                    for (int i = 0; i < index; i++)
                    {
                        VMReturnData.VMCardDetails.VID[i] = Convert.ToUInt16(VehicleIDInfoList[i]);
                    }

                    if (VehicleIDInfoList.Count < 15 && VehicleID != "")
                    {
                        ByEnabled = BtnAddEnabled = true;
                    }
                    else
                    {
                        ByEnabled = BtnAddEnabled = false;
                    }

                    VMReturnData.NextButtonEnabled = VehicleIDInfoList.Count() > 0 ? true : false;
                    VMReturnData.VMCardDetails.ByPassBits = (uint)(VMReturnData.VMCardDetails.ByPassBits & ~0b00000100);
                }

                if (AddbtnPressed)
                {
                    AddbtnPressed = false;
                    if ((index < 15) && (VehicleID != "") && int.TryParse(VehicleID,out int i))
                    {
                        VMReturnData.VMCardDetails.VID[index] = Convert.ToUInt16(VehicleID);
                        VehicleIDInfoList.Add(VehicleID);
                        VehicleID = "";
                        index++;
                    }
                    else
                        BtnAddEnabled = false;

                    VehicleID = "";
                    
                    
                }

                if (RemovebtnPressed)
                {
                    RemovebtnPressed = false;
                    if (VehicleIDInfoList.Count() > 0)
                    {
                        BtnremoveEnabled = true;
                        index--;
                        VMReturnData.VMCardDetails.VID[index] = 0;
                        VehicleIDInfoList.RemoveAt(VehicleIDInfoList.Count - 1);

                    }

                }

                if (VehicleIDInfoList.Count() > 0)
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
                this.View.Visibility = Visibility.Collapsed;
                OneTimeRead = false;
            }

        }

        public ObservableCollection<string> DisplayVehicleList
        {
            get { return _DisplayVehicleList; }
            set { _DisplayVehicleList = value; RaisePropertyChanged("DisplayVehicleList"); }
        }

        public ObservableCollection<string> VehicleIDInfoList
        {
            get { return _vehicleList; }
            set { _vehicleList = value; RaisePropertyChanged("VehicleIDInfoList"); }
        }

        private string _vehicleID;

        public string VehicleID
        {
            get { return _vehicleID; }
            set { _vehicleID = generalFunctions.StringNumConditioner(value); RaisePropertyChanged("VehicleID"); }
        }

        public void AddbtnHandler()
        {
            AddbtnPressed = true;
        }

        private bool _Bypassed;

        public bool Bypassed
        {
            get { return _Bypassed; }
            set { _Bypassed = value; RaisePropertyChanged("Bypassed"); }
        }

        private bool _ByEnabled;
        private bool _btnAddEnabled = true;
        private bool RemovebtnPressed = false;
        private bool _BtnremoveEnabled;

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
    }

}
