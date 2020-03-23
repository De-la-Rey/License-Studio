using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using static Mernok_RFID_Licence_Studio.RFIDCardInfoRead;
using System.Collections.ObjectModel;
using MernokAssets;
using System.Windows.Media;
using MernokClients;
using MernokProducts;

namespace Mernok_RFID_Licence_Studio
{
    public class LicenceViewModel : ViewModel
    {
        #region ViewModel global variables
        private LicenceView _viewInstance;
        private bool CardRead_Done = false;
        private ObservableCollection<VehicleInfoClass> _vehicleList = new ObservableCollection<VehicleInfoClass>();
        public MernokClientFile mernokClientFile = MernokClientManager.ReadMernokClientFile();
        public MernokProductFile mernokProductFile = MernokProductManager.ReadMernokProductFile();
        #endregion

        #region ViewModel Constructor
        public LicenceViewModel(UserControl control) : base(control)
        {
            control.DataContext = this;
             _viewInstance = (LicenceView)control;
        }
        #endregion

        public override void Update(ViewModelReturnData VMReturnData)
        {
                         

            if (VMReturnData.LicenceView_Active)
            {
                View.Visibility = Visibility.Visible;

                #region NavigationBar details
                //               VMReturnData.ViewTitle = "Licence Details";
                VMReturnData.ViewTitle = VMReturnData.cardInfoRead.UIDtoString(VMReturnData.prevUID);
                VMReturnData.SubTitle = "Licence Details";
                VMReturnData.CurrentPageNumber = 1;
                VMReturnData.TotalPageNumber = 1;
                VMReturnData.MenuButtonEnabled = Visibility.Visible;
                VMReturnData.HelpButtonEnabled = Visibility.Hidden;
                VMReturnData.NextButtonEnabled = false;
                VMReturnData.BackButtonEnabled = false;
                #endregion

                #region menu buttons
                VMReturnData.MenuEditBtnEnabled = Visibility.Visible;
                VMReturnData.MenuIssueBtnEnabled = Visibility.Collapsed;
                #endregion

                //Only update this viewModel when this view is visible

                #region populate view
                if (!CardRead_Done)
                {
                    CardRead_Done = true;
                    VehicleInfoList.Clear();
                    switch (VMReturnData.cardInfoRead.cardDetails.Options)
                    {
                        case 3:
                            foreach (byte item in VMReturnData.cardInfoRead.cardDetails.VehicleGroup)
                            {
                                AccessType = "Group";
                                if (item!=0 && item != 255)
                                {
                                    VehicleInfoList.Add(new VehicleInfoClass { VehicleInfo = TagTypesL.MernokAssetGroups[item-1].GroupName, ImagePath = "/Resources/Images/53272.jpg" });                                   
                                }
                                if (item == 255 && VehicleInfoList.Count == 0)
                                {
                                    VehicleInfoList.Add(new VehicleInfoClass
                                    {
                                        VehicleInfo = "All Vehicle Groups",
                                        ImagePath = ("/Resources/Images/TagTypes/Fleet.jpg")
                                    });
                                }

                            }
                            break;

                        case 1:
                            foreach (uint item in VMReturnData.cardInfoRead.cardDetails.VID)
                            {
                                if(item!=0 && item != UInt16.MaxValue) VehicleInfoList.Add(new VehicleInfoClass{ VehicleInfo = item.ToString("X"), ImagePath = "/Resources/Images/53272.jpg" });
                                if (item == UInt16.MaxValue && VehicleInfoList.Count == 0)
                                {
                                    VehicleInfoList.Add(new VehicleInfoClass
                                    {
                                        VehicleInfo = "All Vehicle Serails",
                                        ImagePath = ("/Resources/Images/TagTypes/Fleet.jpg")
                                    });
                                }
                                AccessType = "Serial";
                            }
                            break;

                        case 2:
                            foreach (string item in VMReturnData.cardInfoRead.cardDetails.VehicleNames)
                            {
                                if (item != null && item != "                " && item != "") VehicleInfoList.Add(new VehicleInfoClass{ VehicleInfo = item, ImagePath = " / Resources/Images/53272.jpg" });
                                if (item == "                " && VehicleInfoList.Count == 0)
                                {
                                    VehicleInfoList.Add(new VehicleInfoClass
                                    {
                                        VehicleInfo = "All Vehicle Names",
                                        ImagePath = ("/Resources/Images/TagTypes/Fleet.jpg")
                                    });
                                }
                                AccessType = "Name";
                            }
                            break;

                        case 0:
                            foreach (uint item in VMReturnData.cardInfoRead.cardDetails.VehicleLicenceType)
                            {
                                if(item!=0 && item!=255) VehicleInfoList.Add(new VehicleInfoClass
                                {
                                    VehicleInfo = TagTypesL.MernokAssetType[(int)item-1].TypeName,
                                    ImagePath = ("/Resources/Images/TagTypes/" + TagTypesL.MernokAssetType[(int)item - 1].Type.ToString() + ".png")
                                });
                                if(item==255 && VehicleInfoList.Count==0)
                                {
                                    VehicleInfoList.Add(new VehicleInfoClass
                                    {
                                        VehicleInfo = "All Vehicle Types",
                                        ImagePath = ("/Resources/Images/TagTypes/Fleet.jpg")
                                    });
                                }

                                AccessType = "Type";
                            }
                            break;


                        default:
                            break;
                    }
                    
                    OperatorName = VMReturnData.cardInfoRead.cardDetails.OperatorName;
                    OperatorID = VMReturnData.cardInfoRead.cardDetails.OperatorID;
                    OperatorSurname = VMReturnData.cardInfoRead.cardDetails.OperatorSurname;
                    ProductCode = mernokProductFile.mernokProductList.Where(t => t.Product == VMReturnData.cardInfoRead.cardDetails.ProductCode).First().ProductName;
                    //ClientGroup = (VMReturnData.cardInfoRead.cardDetails.Client_Group).ToString().Replace("_", " ");
                    ClientGroup = mernokClientFile.mernokClientList.Where(t => t.Group == VMReturnData.cardInfoRead.cardDetails.Client_Group).First().ClientGroupName;
                    AccessLevel = ((AccessLevel_enum)VMReturnData.cardInfoRead.cardDetails.AccessLevel).ToString().Replace("_", " ");
                    ExpireDate = VMReturnData.cardInfoRead.cardDetails.Expiry_Date.ToShortDateString();
                    WarningDate = VMReturnData.cardInfoRead.cardDetails.Warning_Date.ToShortDateString();
                    TrainingDate = VMReturnData.cardInfoRead.cardDetails.Training_Date.ToShortDateString();
                    TrainingWarningDate = VMReturnData.cardInfoRead.cardDetails.Training_Warn_Date.ToShortDateString();
                    TrainingExpiryDate = VMReturnData.cardInfoRead.cardDetails.Training_Exp_Date.ToShortDateString();
                    MedicalDate = VMReturnData.cardInfoRead.cardDetails.Medical_Date.ToShortDateString();
                    MedicalWarningDate = VMReturnData.cardInfoRead.cardDetails.Medical_Warn_Date.ToShortDateString();
                    MedicalExpiryDate = VMReturnData.cardInfoRead.cardDetails.Medical_Exp_Date.ToShortDateString();
                    DoverDate = VMReturnData.cardInfoRead.cardDetails.Dover_Date.ToShortDateString();
                    DoverWarningDate = VMReturnData.cardInfoRead.cardDetails.Dover_Warn_Date.ToShortDateString();
                    DoverExpiryDate = VMReturnData.cardInfoRead.cardDetails.Dover_Exp_Date.ToShortDateString();
                    IssueDate = VMReturnData.cardInfoRead.cardDetails.Issue_Date.ToShortDateString();
                    IssuerUID = VMReturnData.cardInfoRead.UIDtoString(VMReturnData.cardInfoRead.cardDetails.IssuerUID);
                    EngineerName = VMReturnData.cardInfoRead.cardDetails.EngineerName;
                    EngineerUID = VMReturnData.cardInfoRead.UIDtoString(VMReturnData.cardInfoRead.cardDetails.EngineerUID);
                    HotFlag = VMReturnData.cardInfoRead.cardDetails.Hotflaged_status;
                    if(HotFlag)
                    {
                        HotFlagDate = VMReturnData.cardInfoRead.cardDetails.HotFlagedDate.ToShortDateString();
                        HotFlagUID = VMReturnData.cardInfoRead.cardDetails.HotFlagedVID.ToString();
                    }
                    else
                    {
                        HotFlagDate = "";
                        HotFlagUID = "";
                    }
                    
                    //ClientCode = ((ClientSite)VMReturnData.cardInfoRead.cardDetails.Client_Site + " " + VMReturnData.cardInfoRead.cardDetails.OperationalArea).Replace("_", " ");
                    ClientCode = mernokClientFile.mernokClientList.Where(t => t.Client == VMReturnData.cardInfoRead.cardDetails.Client_Site).First().ClientSiteName +" "+ VMReturnData.cardInfoRead.cardDetails.OperationalArea;

                    if (VMReturnData.cardInfoRead.cardDetails.Expiry_Date < DateTime.Now)
                    {
                        ExpColour = Brushes.Red;
                    }
                    else
                        ExpColour = null;

                    if (VMReturnData.cardInfoRead.cardDetails.Warning_Date < DateTime.Now)
                    {
                        WarnColor = Brushes.OrangeRed;
                    }
                    else
                        WarnColor = null;
                }




                #endregion
            }
            else
            {
                //View is not visible, do not update
                //Stop any animations on this vieModel
                View.Visibility = Visibility.Collapsed;
                CardRead_Done = false;
            }


        }

        #region Binding Proporties

        #region Protected Properties
        private string _operatorName;
        private string _operatorID;
        private string _operatorSurname;
        private string _accessLevel;
        private string _clientGroup;
        private string _clientcode;
        private string _warningDate;
        private byte _byPassbit;
        private byte _optionsByte;
        private string _issuerUID;
        private string _trainingDate;
        private string _trainingWarningDate;
        private string _trainingExpiryDate;
        private string _medicalDate;
        private string _medicalWarningDate;
        private string _medicalExpiryDate;
        private string _doverDate;
        private string _doverWarningDate;
        private string _doverExpiryDate;
        private string _issueDate;
        private string _engineerUID;
        private string _hotFlagDate;
        private string _hotFlagUID;
        private bool _hotflag;
        private string[] _vehiclenames;
        private string[] _vehicleGroups;
        private string[] _VID;
        #endregion

        public string OperatorName
        {
            get { return _operatorName; }
            set
            {
                _operatorName = value;
                base.RaisePropertyChanged("OperatorName");
            }
        }

        public string OperatorID
        {
            get { return _operatorID; }
            set
            {
                _operatorID = value;
                base.RaisePropertyChanged("OperatorID");
            }
        }
        public string OperatorSurname
        {
            get { return _operatorSurname; }
            set
            {
                _operatorSurname = value;
                base.RaisePropertyChanged("OperatorSurname");
            }
        }

        private string productCode;

        public string ProductCode
        {
            get { return productCode; }
            set {
                productCode = value;
                RaisePropertyChanged("ProductCode");
            }
        }


        public string ClientGroup
        {
            get { return _clientGroup; }
            set
            {
                _clientGroup = value;
                base.RaisePropertyChanged("ClientGroup");
            }
        }

        private string _AccessType;

        public string AccessType
        {
            get { return _AccessType; }
            set { _AccessType = value; base.RaisePropertyChanged("AccessType"); }
        }

        public string AccessLevel
        {
            get { return _accessLevel; }
            set
            {
                _accessLevel = value;
                base.RaisePropertyChanged("AccessLevel");
            }
        }     

        public string ClientCode
        {
            get { return _clientcode; }
            set { _clientcode = value; base.RaisePropertyChanged("ClientCode"); }
        }

        public string WarningDate
        {
            get { return _warningDate; }
            set { _warningDate = value; base.RaisePropertyChanged("WarningDate"); }
        }

        private Brush _warncolor;

        public Brush WarnColor
        {
            get { return _warncolor; }
            set { _warncolor =  value; base.RaisePropertyChanged("WarnColor"); }
        }

        private string _ExpireDate;

        public string ExpireDate
        {
            get { return _ExpireDate; }
            set { _ExpireDate = value; base.RaisePropertyChanged("ExpireDate"); }
        }

        private Brush _expColor;

        public Brush ExpColour
        {
            get { return _expColor; }
            set { _expColor = value; base.RaisePropertyChanged("ExpColour"); }
        }

        public byte ByPassBit
        {
            get { return _byPassbit; }
            set { _byPassbit = value; base.RaisePropertyChanged("ByPassBit"); }
        }  

        public byte OptionsByte
        {
            get { return _optionsByte; }
            set { _optionsByte = value; base.RaisePropertyChanged("ByPassBit"); }
        }

        public string IssuerUID
        {
            get { return _issuerUID; }
            set { _issuerUID = value; base.RaisePropertyChanged("IssuerUID"); }
        }

         public string TrainingDate
        {
            get { return _trainingDate; }
            set { _trainingDate = value; base.RaisePropertyChanged("TrainingDate"); }
        }

        public string TrainingWarningDate
        {
            get { return _trainingWarningDate; }
            set { _trainingWarningDate = value; base.RaisePropertyChanged("TrainingWarningDate"); }
        }

        public string TrainingExpiryDate
        {
            get { return _trainingExpiryDate; }
            set { _trainingExpiryDate = value; base.RaisePropertyChanged("TrainingExpiryDate"); }
        }

        public string MedicalDate
        {
            get { return _medicalDate; }
            set { _medicalDate = value; base.RaisePropertyChanged("MedicalDate"); }
        }

        public string MedicalWarningDate
        {
            get { return _medicalWarningDate; }
            set { _medicalWarningDate = value; base.RaisePropertyChanged("MedicalWarningDate"); }
        }

        public string MedicalExpiryDate
        {
            get { return _medicalExpiryDate; }
            set { _medicalExpiryDate = value; base.RaisePropertyChanged("MedicalExpiryDate"); }
        }

        public string DoverDate
        {
            get { return _doverDate; }
            set { _doverDate = value; base.RaisePropertyChanged("DoverDate"); }
        }

        public string DoverWarningDate
        {
            get { return _doverWarningDate; }
            set { _doverWarningDate = value; base.RaisePropertyChanged("DoverWarningDate"); }
        }

        public string DoverExpiryDate
        {
            get { return _doverExpiryDate; }
            set { _doverExpiryDate = value; base.RaisePropertyChanged("DoverExpiryDate"); }
        }

        public string IssueDate
        {
            get { return _issueDate; }
            set { _issueDate = value; base.RaisePropertyChanged("IssueDate"); }
        }

        public string EngineerUID
        {
            get { return _engineerUID; }
            set { _engineerUID = value; base.RaisePropertyChanged("EngineerUID"); }
        }

        private string _engineerName;

        public string EngineerName
        {
            get { return _engineerName; }
            set { _engineerName = value; base.RaisePropertyChanged("EngineerName"); }
        }

        public string HotFlagDate
        {
            get { return _hotFlagDate; }
            set { _hotFlagDate = value; base.RaisePropertyChanged("HotFlagDate"); }
        }      

        public string HotFlagUID
        {
            get { return _hotFlagUID; }
            set { _hotFlagUID = value; RaisePropertyChanged("HotFlagDate"); }
        }    

        public bool HotFlag
        {
            get { return _hotflag; }
            set { _hotflag = value; RaisePropertyChanged("HotFlag"); }
        }

        public string[] VehicleNames
        {
            get { return _vehiclenames; }
            set { _vehiclenames = value; RaisePropertyChanged("VehicleNames"); }
        }       

        public string[] VehicleGroups
        {
            get { return _vehicleGroups; }
            set { _vehicleGroups = value; RaisePropertyChanged("VehicleGroups"); }
        }      

        public string[] VID
        {
            get { return _VID; }
            set { _VID = value; RaisePropertyChanged("VID"); }
        }

        public ObservableCollection<VehicleInfoClass> VehicleInfoList
        {
            get { return _vehicleList; }
            set { _vehicleList = value; RaisePropertyChanged("VehicleInfoList"); }
        }

        public class VehicleInfoClass
        {
            private string _vehicleInfo;

            public string VehicleInfo
            {
                get { return _vehicleInfo; }
                set { _vehicleInfo = value; }
            }

            private string _imagePath;

            public string ImagePath
            {
                get { return _imagePath; }
                set { _imagePath = value; }
            }

        }

        #endregion

    }
}
