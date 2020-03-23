using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using MernokAssets;
using MernokClients;
using MernokProducts;
using System.Windows.Media;

namespace Mernok_RFID_Licence_Studio
{
    
    public class NewCardDetailsViewModel : ViewModel
    {

        private NewCardDetails1View _viewInstance;

        private ObservableCollection<string> _AccessLevelList;

        private ObservableCollection<string> _VehicleAccessList;

        private RFIDCardInfoRead rFIDCardInfoRead = new RFIDCardInfoRead();

        public MernokClientFile mernokClientFile = MernokClientManager.ReadMernokClientFile();

        public MernokProductFile mernokProductFile = MernokProductManager.ReadMernokProductFile();

        GeneralFunctions generalFunctions = new GeneralFunctions();

        public ICommand AccesstypeHelpbtn { get; private set; }

        public ICommand AccessHelpbtn { get; private set; }
        

        bool onetimeread = false;
        bool onetimeWrite = false;
        bool onetimeWrite2 = false;


        public NewCardDetailsViewModel(UserControl control) : base(control)
        {
            WarningDate = DateTime.Now;
            ExpiryDate = DateTime.Now;
            TrainingDateMax = DateTime.Now;
            ExpiryDateMax = DateTime.Now.AddYears(1).AddDays(1);
            WarningDateMax = DateTime.Now.AddYears(1);
            DateStart = DateTime.Now;

            AccessHelpbtn = new DelegateCommand(AccessHelpbtnHandler);
            AccesstypeHelpbtn = new DelegateCommand(AccesstypeHelpbtnHandler);

            control.DataContext = this;
            
            _viewInstance = (NewCardDetails1View)control;
        }

        private void AccessHelpbtnHandler()
        {
            AboutBtnPressed = true;
        }

        private void AccesstypeHelpbtnHandler()
        {
            AboutTypeBtnPressed = true;
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {
   
            if (VMReturnData.NewCardDetail_Active)
            {
                this.View.Visibility = Visibility.Visible;

                #region Navigation bar
                VMReturnData.ViewTitle = VMReturnData.EditCard ? "Edit Card" : "New Card";
                if(VMReturnData.NewMernokCard)
                    VMReturnData.SubTitle = "Engineer details";
                else
                    VMReturnData.SubTitle = "Operator details";
                //VMReturnData.CurrentPageNumber = 2;
                //VMReturnData.TotalPageNumber = 4;
                VMReturnData.MenuButtonEnabled = Visibility.Collapsed;
                VMReturnData.BackButtonEnabled = true;
                VMReturnData.HelpButtonEnabled = Visibility.Visible;
                #endregion

                //Only update this viewModel when this view is visible
                //if (VMReturnData.CardInField)
                //{
                //    UID = rFIDCardInfoRead.UIDtoString(VMReturnData.UID);
                //    CardImage = new BitmapImage(new Uri(@"/Resources/Images/CardValid.png", UriKind.Relative));
                //}
                //else
                //{
                //    UID = "Card not in field";
                //    CardImage = new BitmapImage(new Uri(@"/Resources/Images/PresentCard.png", UriKind.Relative));
                //}

                //if (VMReturnData.VMCardDetails.IssuerUID == VMReturnData.UID)
                //{
                //    VMReturnData.CardStillIssuer = true;
                //    UID = "Issuer Card in field";
                //}
                //else
                //{
                //    VMReturnData.CardStillIssuer = false;
                //}
                if (AboutTypeBtnPressed)
                {
                    AboutTypeBtnPressed = false;
                    VMReturnData.VehicleAccessLevelHelp = true;
                    VMReturnData.HelpButton();
                }

                if (AboutBtnPressed)
                {
                    AboutBtnPressed = false;
                    VMReturnData.AccessLevelHelp = true;
                    VMReturnData.HelpButton();
                }

                if (!onetimeread)
                {
                    ClientCode = new ObservableCollection<string>();
                    ClientSite = new ObservableCollection<string>();
                    AccessLevelList = new ObservableCollection<string>();
                    VehicleAccessList = new ObservableCollection<string>();
                    ProductCode = new ObservableCollection<string>();

                    foreach (RFIDCardInfoRead.AccessLevel_enum item in Enum.GetValues(typeof(RFIDCardInfoRead.AccessLevel_enum)))
                    {
                        string Level = item.ToString().Replace("_", " ");
                        
                        if (((char)VMReturnData.IssuerAccess == 'C' || (VMReturnData.IssuerAccess == 9)) && (Level == "Operator" || Level == "Temporary Operator" || Level == "Trainee Operator" || Level == "Electrician" || Level == "Foreman" || Level == "Shift Boss" || Level == "Maintenance" || Level == "Field Technician") )
                        {
                            //AccessLevelList.Where(t => t.Equals("Operator") || t.Equals("Temporary Operator") || t.Equals("Trainee Operator")).ToList();
                            AccessLevelList.Add(Level);
                        }
                        else if ((char)VMReturnData.IssuerAccess == 'Z' || VMReturnData.NewMernokCard)
                        {
                            AccessLevelList.Add(Level);
                        }
                        else if(VMReturnData.NewIssuerCard)
                        {
                            if(!Level.Contains("Mernok"))
                            {
                                AccessLevelList.Add(Level);
                            }
                        }

                    }
                    AccessLevelnum = 0;

                    foreach (RFIDCardInfoRead.VehicleAccessType item in Enum.GetValues(typeof(RFIDCardInfoRead.VehicleAccessType)))
                    {
                        VehicleAccessList.Add(item.ToString().Replace("_", " "));                       
                    }
                    VehicleAccessType_ret = 0;

                    ClientCode = mernokClientFile.mernokClientList.Select(l => l.ClientGroupName).Distinct().ToList();
                    ClientCodenum = 0;
                    ClientSite = mernokClientFile.mernokClientList.Where(l => l.Group == ClientCodenum).Select(t=> t.ClientSiteName).ToList();
                    ClientSitenum = 0;
                    ProductCode = mernokProductFile.mernokProductList.Select(t => t.ProductName).ToList();
                    ProductList_ret = 0;
                    if (((char)VMReturnData.IssuerAccess == 'Z')|| VMReturnData.NewIssuerCard || VMReturnData.NewMernokCard || ((char)VMReturnData.IssuerAccess == 13))
                    {
                        ClientEdit = true;
                        AccessEdit = true;

                    }
                    else if ((char)VMReturnData.IssuerAccess == 'C' || ((char)VMReturnData.IssuerAccess == 9))
                    {
                        ClientEdit = false;
                        ClientCodenum = (int)VMReturnData.VMCardDetails.Client_Group;
                        ClientSitenum = Math.Abs(ClientSite.IndexOf(ClientSite.Where(T => T == mernokClientFile.mernokClientList[(int)VMReturnData.VMCardDetails.Client_Site].ClientSiteName).First()));
                    }

                    VMReturnData.VMCardDetails.Issue_Date = DateTime.Now;
                    ExpiryDate = DateTime.Now;
                    WarningDate = DateTime.Now;
                    TrainDate = DateTime.Now;
                    TrainWarnDate = DateTime.Now;
                    TrainExpDate = DateTime.Now;
                    MedicalDate = DateTime.Now;
                    MedicalWarnDate = DateTime.Now;
                    MedicalExpDate = DateTime.Now;
                    DoverDate = DateTime.Now;
                    DoverWarnDate = DateTime.Now;
                    DoverExpDate = DateTime.Now;
                    onetimeread = true;
                    OperatorName = "";
                    OperatorID = "";
                    OperatorSurname = "";
                    OperationalArea = "";
                }

                if (VMReturnData.EditCard && !onetimeWrite)
                {
                    OperatorName = VMReturnData.CopiedVMCardDetails.OperatorName.Replace(" ","");
                    OperatorID = VMReturnData.CopiedVMCardDetails.OperatorID.Replace(" ", "");
                    OperatorSurname = VMReturnData.CopiedVMCardDetails.OperatorSurname.Replace(" ", "");
                    ClientCodenum = (int)VMReturnData.CopiedVMCardDetails.Client_Group;
                    ClientSitenum = Math.Abs(ClientSite.IndexOf(ClientSite.Where(T => T == mernokClientFile.mernokClientList[(int)VMReturnData.CopiedVMCardDetails.Client_Site].ClientSiteName).First()));
                    ProductList_ret = (int)VMReturnData.CopiedVMCardDetails.ProductCode;
                    OperationalArea = VMReturnData.CopiedVMCardDetails.OperationalArea.ToString();
                    TrainDate = VMReturnData.CopiedVMCardDetails.Training_Date;
                    TrainWarnDate = VMReturnData.CopiedVMCardDetails.Training_Warn_Date;
                    TrainExpDate = VMReturnData.CopiedVMCardDetails.Training_Exp_Date;
                    MedicalDate = VMReturnData.CopiedVMCardDetails.Medical_Date;
                    MedicalWarnDate = VMReturnData.CopiedVMCardDetails.Medical_Warn_Date;
                    MedicalExpDate = VMReturnData.CopiedVMCardDetails.Medical_Exp_Date;
                    DoverDate = VMReturnData.CopiedVMCardDetails.Dover_Date;
                    DoverWarnDate = VMReturnData.CopiedVMCardDetails.Dover_Warn_Date;
                    DoverExpDate = VMReturnData.CopiedVMCardDetails.Dover_Exp_Date;
                    AccessLevelnum = (int)Math.Abs(AccessLevelList.IndexOf( ((RFIDCardInfoRead.AccessLevel_enum)VMReturnData.CopiedVMCardDetails.AccessLevel).ToString().Replace("_", " ")));
                    VehicleAccessType_ret = VMReturnData.CopiedVMCardDetails.Options;
                    ExpiryDate = VMReturnData.CopiedVMCardDetails.Expiry_Date;
                    WarningDate = VMReturnData.CopiedVMCardDetails.Warning_Date;
                    onetimeWrite = true;
                }

                if (VMReturnData.NewMernokCard && !onetimeWrite2)
                {
                    OperatorName = "Mernok";
                    OperatorID = "";
                    OperatorSurname = "";
                    ClientCodenum = 18;
                    ClientSitenum = 0;
                    ProductList_ret = 0;
                    OperationalArea = 1.ToString();
                    TrainDate = DateTime.Now.AddDays(-1);
                    TrainWarnDate = DateTime.Now.AddDays(-1);
                    TrainExpDate = DateTime.Now.AddDays(-1);
                    MedicalDate = DateTime.Now.AddDays(-1);
                    MedicalWarnDate = DateTime.Now.AddDays(-1);
                    MedicalExpDate = DateTime.Now.AddDays(-1);
                    DoverDate = DateTime.Now.AddDays(-1);
                    DoverWarnDate = DateTime.Now.AddDays(-1);
                    DoverExpDate = DateTime.Now.AddDays(-1);
                    AccessLevelnum = (int)Math.Abs(AccessLevelList.IndexOf((RFIDCardInfoRead.AccessLevel_enum.Mernok_Engineer.ToString().Replace("_", " "))));
                    VehicleAccessType_ret = VMReturnData.CopiedVMCardDetails.Options;
                    ExpiryDate = DateTime.Now.AddMonths(1).AddDays(1);
                    WarningDate = DateTime.Now.AddMonths(1);
                    onetimeWrite2 = true;
                    VMReturnData.VMCardDetails.EngineerName = "Mernok Electronik";
                    VMReturnData.VMCardDetails.Issue_Date = DateTime.Now;
                    VMReturnData.VMCardDetails.IssuerUID = VMReturnData.NewMernokUID;
                    VMReturnData.VMCardDetails.EngineerUID = 045;
                    VMReturnData.VMCardDetails.Hotflaged_status = false;
                }

                if (Conditioning())
                {
                    VMReturnData.NextButtonEnabled = true;
                    VMReturnData.VMCardDetails.OperatorName = OperatorName;
                    VMReturnData.VMCardDetails.OperatorID = OperatorID;
                    VMReturnData.VMCardDetails.OperatorSurname = OperatorSurname;
                    VMReturnData.VMCardDetails.Client_Group = (uint)ClientCodenum;
                    VMReturnData.VMCardDetails.Client_Site = (uint)ClientSitenum;
                    VMReturnData.VMCardDetails.ProductCode = (uint)ProductList_ret;
                    VMReturnData.VMCardDetails.Client_Site = mernokClientFile.mernokClientList.Where(t => t.ClientSiteName == ClientSite[ClientSitenum]).First().Client;
                    VMReturnData.VMCardDetails.OperationalArea = Convert.ToUInt16(OperationalArea);
                    VMReturnData.VMCardDetails.Warning_Date = WarningDate;
                    VMReturnData.VMCardDetails.Training_Date = TrainDate;
                    VMReturnData.VMCardDetails.Training_Warn_Date = TrainWarnDate;
                    VMReturnData.VMCardDetails.Training_Exp_Date = TrainExpDate;
                    VMReturnData.VMCardDetails.Medical_Date = MedicalDate;
                    VMReturnData.VMCardDetails.Medical_Warn_Date = MedicalWarnDate;
                    VMReturnData.VMCardDetails.Medical_Exp_Date = MedicalExpDate;
                    VMReturnData.VMCardDetails.Dover_Date = DoverDate;
                    VMReturnData.VMCardDetails.Dover_Warn_Date = DoverWarnDate;
                    VMReturnData.VMCardDetails.Dover_Exp_Date = DoverExpDate;
                    VMReturnData.VMCardDetails.Expiry_Date = ExpiryDate;
                    VMReturnData.VMCardDetails.AccessLevel = (byte)(RFIDCardInfoRead.AccessLevel_enum)Enum.Parse(typeof(RFIDCardInfoRead.AccessLevel_enum), AccessLevelList[(int)AccessLevelnum].Replace(" ", "_"));
                    VMReturnData.VMCardDetails.Options = VehicleAccessType_ret;
                    VMReturnData.VMCardDetails.Hotflaged_status = false;
                }
                else
                {
                    VMReturnData.NextButtonEnabled = false;
                }

            }
            else
            {
                //View is not visible, do not update
                //Stop any animations on this vieModel
                this.View.Visibility = Visibility.Collapsed;
                onetimeread = false;
                onetimeWrite = false;
                onetimeWrite2 = false;
            }
        }

        bool Conditioning()
        {
            bool name, id, clientgroup, clientsite, OpArea, product;


            if (OperatorName != null && OperatorName != "")
            {
                name = true;
                //NameColour = Brushes.White;
                NameReq = Visibility.Hidden;

            }
            else
            {
                name = false;
                //NameColour = Brushes.OrangeRed;
                NameReq = Visibility.Visible;
            }

            if (OperatorID != null && OperatorID != "")
            {
                id = true;
                //NameColour = Brushes.White;
                IDReq = Visibility.Hidden;

            }

            if (OperatorSurname != null && OperatorSurname != "")
            {
                id = true;
                //NameColour = Brushes.White;
                SurnameReq = Visibility.Hidden;

            }
            else
            {
                id = false;
                //NameColour = Brushes.OrangeRed;
                IDReq = Visibility.Visible;
            }

            if (ClientCodenum > 0) 
            {
                clientgroup = true;
                //ClientColour = Brushes.White;
                groupReq = Visibility.Hidden;
            }
            else
            {
                clientgroup = false;
                //ClientColour = Brushes.OrangeRed;
                groupReq = Visibility.Visible;
            }

            /* if (ClientSite[ClientSitenum] != "None")
             {
                 clientsite = true;
                 ClientSColour = Brushes.White;
             }
             else
             {
                 clientsite = false;
                 ClientSColour = Brushes.OrangeRed;
             }*/
            clientsite = true;

            if (OperationalArea != null && OperationalArea != "")
            {
                OpArea = true;
                //OperColour = Brushes.White;
                OpReq = Visibility.Hidden;
            }
            else
            {
                OpArea = false;
                //OperColour = Brushes.OrangeRed;
                OpReq = Visibility.Visible;
            }

            if (ProductList_ret > 0)
            {
                product = true;
                //ProductColour = Brushes.White;
                ProductReq = Visibility.Hidden;
            }
            else
            {
                product = false;
                //ProductColour = Brushes.OrangeRed;
                ProductReq = Visibility.Visible;
            }

            if (name && id && clientsite && clientgroup && OpArea && product)
                return true;
            else
                return false;

        }

        #region Binding properties

        #region VisibilityProps
        private Visibility _NameReq;

        public Visibility NameReq
        {
            get { return _NameReq; }
            set { _NameReq = value; RaisePropertyChanged("NameReq"); }
        }


        private Visibility _SurnameReq;

        public Visibility SurnameReq
        {
            get { return _SurnameReq; }
            set { _SurnameReq = value; RaisePropertyChanged("SurnameReq"); }
        }

        private Visibility _IDReq;
        public Visibility IDReq
        {
            get { return _IDReq; }
            set { _IDReq = value; RaisePropertyChanged("IDReq"); }
        }

        private Visibility _groupReq;

        public Visibility groupReq
        {
            get { return _groupReq; }
            set { _groupReq = value; RaisePropertyChanged("groupReq"); }
        }
        private Visibility _OpReq;

        public Visibility OpReq
        {
            get { return _OpReq; }
            set { _OpReq = value; RaisePropertyChanged("OpReq"); }
        }

        private Visibility _ProductReq;

        public Visibility ProductReq
        {
            get { return _ProductReq; }
            set { _ProductReq = value; RaisePropertyChanged("ProductReq"); }
        }



        #endregion


        private bool _AccessEdit;

        public bool AccessEdit
        {
            get { return _AccessEdit; }
            set { _AccessEdit = value; RaisePropertyChanged("AccessEdit"); }
        }

        private bool _ClientEdit;

        public bool ClientEdit
        {
            get { return _ClientEdit; }
            set { _ClientEdit = value; RaisePropertyChanged("ClientEdit"); }
        }



        private string _operatorName;

        public string OperatorName
        {
            get { return _operatorName; }
            set { _operatorName = generalFunctions.StringConditioner(value); RaisePropertyChanged("OperatorName"); }
        }



        private string _operatorSurname;

        public string OperatorSurname
        {
            get { return _operatorSurname; }
            set { _operatorSurname = generalFunctions.StringConditioner(value); RaisePropertyChanged("OperatorSurname"); }
        }


        private string _operatorID;

        public string OperatorID
        {
            get { return _operatorID; }
            set { _operatorID = generalFunctions.StringConditioner(value); RaisePropertyChanged("OperatorID"); }
        }

        private IList<string> _clientCode;

        public IList<string> ClientCode
        {
            get { return _clientCode; }
            set
            {
                _clientCode = value;
                RaisePropertyChanged("ClientCode");               
            }
        }

        private IList<string> _productCode;

        public IList<string> ProductCode
        {
            get { return _productCode; }
            set { _productCode = value; RaisePropertyChanged("ProductCode"); }
        }

        private int _ProductList_ret;

        public int ProductList_ret
        {
            get { return _ProductList_ret; }
            set { _ProductList_ret = value; RaisePropertyChanged("ProductList_ret"); }
        }



        private int _ClientCodenum;

        public int ClientCodenum
        {
            get { return _ClientCodenum; }
            set {
                _ClientCodenum = value;
                RaisePropertyChanged("ClientCodenum");
                ClientSite = mernokClientFile.mernokClientList.Where(l => l.Group == ClientCodenum).Select(t => t.ClientSiteName).ToList();
                ClientSitenum = 0;
            }
        }


        private IList<string> _clientsite;

        public IList<string> ClientSite
        {
            get { return _clientsite; }
            set { _clientsite = value; RaisePropertyChanged("ClientSite"); }
        }

        private int _ClientSitenum;

        public int ClientSitenum
        {
            get { return _ClientSitenum; }
            set { _ClientSitenum = value; RaisePropertyChanged("ClientSitenum"); }
        }

        private string _operationalArea;

        public string OperationalArea
        {
            get { return _operationalArea; }
            set { _operationalArea = generalFunctions.StringNumConditioner(value); RaisePropertyChanged("OperationalArea"); }
        }



        private int _vehicleAccesstype;

        public int VehicleAccessType
        {
            get { return _vehicleAccesstype; }
            set { _vehicleAccesstype = value; RaisePropertyChanged("VehicleAccessType"); }
        }

        private DateTime _warnDate;

        public DateTime WarningDate
        {
            get { return _warnDate.Date; }
            set { _warnDate = value; RaisePropertyChanged("WarningDate"); }
        }

        private DateTime _expiryDate;

        public DateTime ExpiryDate
        {
            get { return _expiryDate.Date; }
            set { _expiryDate = value; RaisePropertyChanged("ExpiryDate"); }
        }

        private DateTime _trainDate;

        public DateTime TrainDate
        {
            get { return _trainDate.Date; }
            set { _trainDate = value; RaisePropertyChanged("TrainDate"); }
        }

        private DateTime _trainWarnDate;
        public DateTime TrainWarnDate
        {
            get { return _trainWarnDate.Date; }
            set { _trainWarnDate = value; RaisePropertyChanged("TrainWarnDate"); }
        }

        private DateTime _trainExpDate;
        public DateTime TrainExpDate
        {
            get { return _trainExpDate.Date; }
            set { _trainExpDate = value; RaisePropertyChanged("TrainExpDate"); }
        }

        private DateTime _MedicalDate;
        public DateTime MedicalDate
        {
            get { return _MedicalDate.Date; }
            set { _MedicalDate = value; RaisePropertyChanged("MedicalDate"); }
        }

        private DateTime _MedicalWarnDate;

        public DateTime MedicalWarnDate
        {
            get { return _MedicalWarnDate.Date; }
            set { _MedicalWarnDate = value; RaisePropertyChanged("MedicalWarnDate"); }
        }

        private DateTime _MedicalExpDate;

        public DateTime MedicalExpDate
        {
            get { return _MedicalExpDate.Date; }
            set { _MedicalExpDate = value; RaisePropertyChanged("MedicalExpDate"); }
        }

        private DateTime _DoverDate;

        public DateTime DoverDate
        {
            get { return _DoverDate.Date; }
            set { _DoverDate = value; RaisePropertyChanged("DoverDate"); }
        }

        private DateTime _DoverWarnDate;
        public DateTime DoverWarnDate
        {
            get { return _DoverWarnDate.Date; }
            set { _DoverWarnDate = value; RaisePropertyChanged("DoverWarnDate"); }
        }

        private DateTime _DoverExpDate;
        public DateTime DoverExpDate
        {
            get { return _DoverExpDate.Date; }
            set { _DoverExpDate = value; RaisePropertyChanged("DoverExpDate"); }
        }

        private DateTime _TrainingDateMax;

        public DateTime TrainingDateMax
        {
            get { return _TrainingDateMax; }
            set { _TrainingDateMax = value; RaisePropertyChanged("TrainingDateMax"); }
        }

        private DateTime _ExpiryDateMax;

        public DateTime ExpiryDateMax
        {
            get { return _ExpiryDateMax; }
            set { _ExpiryDateMax = value; RaisePropertyChanged("ExpiryDateMax"); }
        }

        private DateTime _WarningDateMax;

        public DateTime WarningDateMax
        {
            get { return _WarningDateMax; }
            set { _WarningDateMax = value; RaisePropertyChanged("WarningDateMax"); }
        }

        private DateTime _DateStart;

        public DateTime DateStart
        {
            get { return _DateStart; }
            set { _DateStart = value; RaisePropertyChanged("DateStart"); }
        }

        private Brush _ProductColour;

        public Brush ProductColour
        {
            get { return _ProductColour; }
            set { _ProductColour = value; base.RaisePropertyChanged("ProductColour"); }
        }

        private Brush _NameColour;

        public Brush NameColour
        {
            get { return _NameColour; }
            set { _NameColour = value; base.RaisePropertyChanged("NameColour"); }
        }


        private Brush _ClientColour;

        public Brush ClientColour
        {
            get { return _ClientColour; }
            set { _ClientColour = value; base.RaisePropertyChanged("ClientColour"); }
        }

        private Brush _OperColour;

        public Brush OperColour
        {
            get { return _OperColour; }
            set { _OperColour = value; base.RaisePropertyChanged("OperColour"); }
        }

        private Brush _AccessColour;

        public Brush AccessColour
        {
            get { return _AccessColour; }
            set { _AccessColour = value; base.RaisePropertyChanged("AccessColour"); }
        }

        private Brush _VehicleAccessColour;

        public Brush VehicleAccessColour
        {
            get { return _VehicleAccessColour; }
            set { _VehicleAccessColour = value; base.RaisePropertyChanged("VehicleAccessColour"); }
        }

        private int _AccessLevelnum;

        public int AccessLevelnum
        {
            get { return _AccessLevelnum; }
            set { _AccessLevelnum = value; RaisePropertyChanged("AccessLevelnum"); }
        }

        private int _vehicleAccessType;

        public int VehicleAccessType_ret
        {
            get { return _vehicleAccessType; }
            set { _vehicleAccessType = value; RaisePropertyChanged("VehicleAccessType_ret"); }
        }

        public ObservableCollection<string> AccessLevelList
        {
            get { return _AccessLevelList; }
            set { _AccessLevelList = value; RaisePropertyChanged("AccessLevelList"); }
        }

        public ObservableCollection<string> VehicleAccessList
        {
            get { return _VehicleAccessList; }
            set { _VehicleAccessList = value; RaisePropertyChanged("VehicleAccessList"); }
        }

        private BitmapImage bitmapImage;
        public BitmapImage CardImage
        {
            get { return bitmapImage; }
            set
            {
                bitmapImage = value;
                RaisePropertyChanged("CardImage");
            }
        }

        private string _UID;

        public string UID
        {
            get { return _UID; }
            set
            {
                _UID = value;
                base.RaisePropertyChanged("UID");
            }
        }

        private SolidColorBrush _ClientSColour;
        private bool AboutBtnPressed;
        private bool AboutTypeBtnPressed;

        public SolidColorBrush ClientSColour { get { return _ClientSColour; } set { _ClientSColour = value; RaisePropertyChanged("UID"); } }
        #endregion
    }



}
