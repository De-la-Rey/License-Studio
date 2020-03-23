using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using MernokAssets;

namespace Mernok_RFID_Licence_Studio
{
    public class NewCardTypeViewModel : ViewModel
    {
        public ICommand AddBtn { get; private set; }
        public ICommand RemoveBtn { get; private set; }

        private NewCardTypeView _viewInstance;
        private bool AddbtnPressed = false;
        private bool RemovebtnPressed = false;
        bool OneTimeRead;
        int index = 0;
        private ObservableCollection<string> _vehicletypeList = new ObservableCollection<string>();
        private ObservableCollection<string> _vehicletypeList2 = new ObservableCollection<string>();
        private ObservableCollection<string> _DisplayVehicleList = new ObservableCollection<string>();

        public NewCardTypeViewModel(UserControl control) : base(control)
        {

            AddBtn = new DelegateCommand(AddbtnHandler);
            RemoveBtn = new DelegateCommand(RemoveBtnHandler);
            control.DataContext = this;
            _viewInstance = (NewCardTypeView)control;
        }

        public void AddbtnHandler()
        {
            AddbtnPressed = true;
        }

        private void RemoveBtnHandler()
        {
            RemovebtnPressed = true;
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {

            if (VMReturnData.NewCardType_Active)
            {
                View.Visibility = Visibility.Visible;

                #region Navigation Bar details
                VMReturnData.ViewTitle = VMReturnData.EditCard ? "Edit Card" : "New Card";
                VMReturnData.SubTitle = "Vehicle Type details";
                VMReturnData.CurrentPageNumber = 3;
                VMReturnData.TotalPageNumber = 4;
                VMReturnData.MenuButtonEnabled = Visibility.Collapsed;
                VMReturnData.HelpButtonEnabled = Visibility.Visible;
                #endregion

                if (!OneTimeRead)
                {

                    VehicleTypeList = new ObservableCollection<string>();
                    VehicleTypeList2 = new ObservableCollection<string>();
                    DisplayVehicleList = new ObservableCollection<string>();
                    index = 0;

                    foreach (var item in TagTypesL.MernokAssetType)
                    {
                        if (item.IsLicensable)
                            VehicleTypeList.Add(item.TypeName);
                    }

                    OneTimeRead = true;
                    VehicleTypenum = 0;
                }

                if (Bypassed)
                {
                    DisplayVehicleList = new ObservableCollection<string> { "All Vehicle Types" };
                    ByEnabled = BtnAddEnabled = false;
                    VMReturnData.VMCardDetails.VehicleLicenceType = Enumerable.Repeat((uint)255, 30).ToArray();
                    VMReturnData.VMCardDetails.ByPassBits = VMReturnData.VMCardDetails.ByPassBits | 0x04;
                    VMReturnData.NextButtonEnabled = true;
                }
                else
                {
                    DisplayVehicleList = VehicleTypeList2;
                    for (int i = 0; i < index; i++)
                    {
                        VMReturnData.VMCardDetails.VehicleLicenceType[i] = (byte)(TagTypesL.MernokAssetType.IndexOf(TagTypesL.MernokAssetType.Where(p => p.TypeName == VehicleTypeList2[i]).FirstOrDefault()) + 1);
                    }
                    
                    ByEnabled = BtnAddEnabled = true;
                    VMReturnData.NextButtonEnabled = VehicleTypeList2.Count() > 0 ? true : false;
                    VMReturnData.VMCardDetails.ByPassBits = (uint)(VMReturnData.VMCardDetails.ByPassBits & ~0b00000100);
                }              

                if (AddbtnPressed)
                {
                    AddbtnPressed = false;
                    if (VehicleTypeList2.Count < 15)
                    {
                        VMReturnData.VMCardDetails.VehicleLicenceType[index] = (byte)(TagTypesL.MernokAssetType.IndexOf(TagTypesL.MernokAssetType.Where(p => p.TypeName == VehicleTypeList[VehicleTypenum]).FirstOrDefault()) + 1);
                        VehicleTypeList2.Add(VehicleTypeList[VehicleTypenum]);
                        index++;
                        VehicleTypeList.RemoveAt(VehicleTypenum);                        
                        VehicleTypenum = 0;
                    }
                    else
                        BtnAddEnabled = false;
                }

                if (RemovebtnPressed)
                {
                    RemovebtnPressed = false;
                    if (VehicleTypeList2.Count() > 0)
                    {
                        BtnremoveEnabled = true;
                        index--;
                        VMReturnData.VMCardDetails.VehicleLicenceType[index] = 0;
                        VehicleTypeList.Add(VehicleTypeList2.Last());
                        VehicleTypeList2.RemoveAt(VehicleTypeList2.Count - 1);
                    }

                }

                if (VehicleTypeList.Count > 0)
                {
                    ByEnabled = BtnAddEnabled = true;
                }
                else
                    BtnAddEnabled = false;

                if (VehicleTypeList2.Count() > 0)
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
                OneTimeRead = false;
                this.View.Visibility = Visibility.Collapsed;

            }
        }

        #region Binding properties
        public ObservableCollection<string> VehicleTypeList
        {
            get { return _vehicletypeList; }
            set { _vehicletypeList = value; RaisePropertyChanged("VehicleTypeList"); }
        }

        public ObservableCollection<string> VehicleTypeList2
        {
            get { return _vehicletypeList2; }
            set { _vehicletypeList2 = value; RaisePropertyChanged("VehicleTypeList2"); }
        }

        public ObservableCollection<string> DisplayVehicleList
        {
            get { return _DisplayVehicleList; }
            set { _DisplayVehicleList = value; RaisePropertyChanged("DisplayVehicleList"); }
        }

        private string _vehicleType;

        public string VehicleType
        {
            get { return _vehicleType; }
            set { _vehicleType = value; RaisePropertyChanged("VehicleType"); }
        }

        private int _vehicleTypenum;

        public int VehicleTypenum
        {
            get { return _vehicleTypenum; }
            set { _vehicleTypenum = value; RaisePropertyChanged("VehicleTypenum"); }
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

    }

}
