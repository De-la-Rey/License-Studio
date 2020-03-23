using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using MernokAssets;
using System.Collections.ObjectModel;

namespace Mernok_RFID_Licence_Studio
{
    public class NewCardGroupViewModel : ViewModel
    {
        private NewCardGroupView _viewInstance;

        public ICommand AddBtn { get; private set; }
        public ICommand RemoveBtn { get; private set; }

        private bool AddbtnPressed = false;
        bool OneTimeRead;
        int index = 0;
        private ObservableCollection<string> _vehicleGroupList = new ObservableCollection<string>();
        private ObservableCollection<string> _vehicleGroupList2 = new ObservableCollection<string>();
        private ObservableCollection<string> _DisplayVehicleList = new ObservableCollection<string>();

        public NewCardGroupViewModel(UserControl control) : base(control)
        {
            AddBtn = new DelegateCommand(AddbtnHandler);
            RemoveBtn = new DelegateCommand(RemoveBtnHandler);
            control.DataContext = this;
            _viewInstance = (NewCardGroupView)control;
        }



        public override void Update(ViewModelReturnData VMReturnData)
        {
   
            if (VMReturnData.NewCardGroup_Active)
            {
                this.View.Visibility = Visibility.Visible;
                //Only update this viewModel when this view is visible

                #region Navigationbar details
                VMReturnData.ViewTitle = "New Card";
                VMReturnData.SubTitle = "Group details";
                VMReturnData.CurrentPageNumber = 3;
                VMReturnData.TotalPageNumber = 4;
                VMReturnData.MenuButtonEnabled = Visibility.Collapsed;
                VMReturnData.HelpButtonEnabled = Visibility.Visible;
                #endregion

                if (!OneTimeRead)
                {
                    VehicleGroupList = new ObservableCollection<string>();
                    VehiclegroupList2 = new ObservableCollection<string>();
                    DisplayVehicleList = new ObservableCollection<string>();
                    foreach (var item in TagTypesL.MernokAssetGroups)
                    {
                        VehicleGroupList.Add(item.GroupName);
                    }
                    OneTimeRead = true;
                    VehicleGroupnum = 0;
                    index = 0;
                }

                if (Bypassed)
                {
                    DisplayVehicleList = new ObservableCollection<string> { "All Vehicle Groups" };
                    ByEnabled = BtnAddEnabled = false;
                    VMReturnData.VMCardDetails.VehicleGroup = Enumerable.Repeat((byte)255, 30).ToArray();
                    VMReturnData.VMCardDetails.ByPassBits = VMReturnData.VMCardDetails.ByPassBits | 0x04;
                    VMReturnData.NextButtonEnabled = true;
                }
                else
                {
                    DisplayVehicleList = VehiclegroupList2;
                    for (int i = 0; i < index; i++)
                    {
                        VMReturnData.VMCardDetails.VehicleGroup[i] = (byte)(TagTypesL.MernokAssetGroups.IndexOf(TagTypesL.MernokAssetGroups.Where(p => p.GroupName == VehiclegroupList2[i]).FirstOrDefault()) + 1);
                    }                    
                    VMReturnData.NextButtonEnabled = VehiclegroupList2.Count() > 0 ? true : false;
                    VMReturnData.VMCardDetails.ByPassBits = (uint)(VMReturnData.VMCardDetails.ByPassBits & ~0b00000100);

                }

                if (AddbtnPressed)
                {
                    if (VehiclegroupList2.Count() < 15 && VehicleGroupList.Count>0)
                    {
                        VehiclegroupList2.Add(VehicleGroupList[VehicleGroupnum]);
                        VMReturnData.VMCardDetails.VehicleGroup[index] = (byte)(TagTypesL.MernokAssetGroups.IndexOf(TagTypesL.MernokAssetGroups.Where(p => p.GroupName == VehicleGroupList[VehicleGroupnum]).FirstOrDefault()) + 1);
                        index++;
                        AddbtnPressed = false;
                        VehicleGroupList.RemoveAt(VehicleGroupnum);
                        VehicleGroupnum = 0;

                    }
                                                                                  
                }

                if(RemovebtnPressed)
                {
                    RemovebtnPressed = false;
                    if (VehiclegroupList2.Count() > 0)
                    {
                        BtnremoveEnabled = true;
                        index--;
                        VMReturnData.VMCardDetails.VehicleGroup[index] = 0;
                        VehicleGroupList.Add(VehiclegroupList2.Last());
                        VehiclegroupList2.RemoveAt(VehiclegroupList2.Count-1);                      
                    }

                }

                if(VehicleGroupList.Count > 0)
                {
                    ByEnabled = BtnAddEnabled = true;
                }
                else
                    BtnAddEnabled = false;

                if (VehiclegroupList2.Count()>0)
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
        public ObservableCollection<string> VehicleGroupList
        {
            get { return _vehicleGroupList; }
            set { _vehicleGroupList = value; RaisePropertyChanged("VehicleGroupList"); }
        }

        public ObservableCollection<string> VehiclegroupList2
        {
            get { return _vehicleGroupList2; }
            set { _vehicleGroupList2 = value; RaisePropertyChanged("VehiclegroupList2"); }
        }

        public ObservableCollection<string> DisplayVehicleList
        {
            get { return _DisplayVehicleList; }
            set { _DisplayVehicleList = value; RaisePropertyChanged("DisplayVehicleList"); }
        }

        private string _vehiclegroup;

        public string VehicleGroup
        {
            get { return _vehiclegroup; }
            set { _vehiclegroup = value; RaisePropertyChanged("VehicleGroup"); }
        }

        private int _vehiclegroupnum;

        public int VehicleGroupnum
        {
            get { return _vehiclegroupnum; }
            set { _vehiclegroupnum = value; RaisePropertyChanged("VehicleGroupnum"); }
        }

        public void AddbtnHandler()
        {
            AddbtnPressed = true;
        }

        private void RemoveBtnHandler()
        {
            RemovebtnPressed = true;
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
            set { _ByEnabled = value; RaisePropertyChanged("VehicleGroupnum"); }
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
    }

}

