using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MernokPasswords;
using Microsoft.Win32;

namespace Mernok_RFID_Licence_Studio
{
    class NewIssuerCardViewModel : ViewModel
    {
        private NewIssuerCardView _viewInstance;

        RFIDCardInfoRead cardInfoRead = new RFIDCardInfoRead();
        PasswordFinder passwordFinder = new PasswordFinder();
        private const string V = @"C:\Passwords\MernokPasswordMasterList.xml";
        public string LicenseFilePath;
        public MernokPasswordFile mernokPasswordFile = new MernokPasswordFile();
        public CardDetailsFile CardDetailsFile = new CardDetailsFile();

        public ICommand IssuerFileBtn { get; private set; }
        private bool IssuerFileCardBtnPressed = false;

        ulong CardinFieldUID = 0;

        public NewIssuerCardViewModel(NewIssuerCardView control) : base(control)
        {
            PassColour = Brushes.OrangeRed;
            
            IssuerFileBtn = new DelegateCommand(IssuerFileCardBtnHandler);
            control.DataContext = this;
            _viewInstance = (NewIssuerCardView)control;
        }

        private void IssuerFileCardBtnHandler()
        {
            IssuerFileCardBtnPressed = true;
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {


            if (VMReturnData.NewCardIssuer_Active)
            {
                this.View.Visibility = Visibility.Visible;

                #region Navigationbar details
                VMReturnData.ViewTitle = "New Issuer Card";
                VMReturnData.SubTitle = "New Issuer details";
                //VMReturnData.CurrentPageNumber = 1;
                //VMReturnData.TotalPageNumber = 4;
                VMReturnData.MenuButtonEnabled = Visibility.Collapsed;
                VMReturnData.HelpButtonEnabled = Visibility.Visible;
                #endregion

                if (IssuerFileCardBtnPressed)
                {
                    IssuerFileCardBtnPressed = false;
                    MernokPasswordFile mernokPasswordFile = MernokPasswordManager.ReadMernokPasswordFile(V);
                    OpenFileDialog openFileDialog1 = new OpenFileDialog();
                    openFileDialog1.Filter = "License Files|*.merlic";
                    openFileDialog1.Title = "Select a Mernok Licnese File";
                    if (openFileDialog1.ShowDialog()==true)
                    {
                        // Assign the cursor in the Stream to the Form's Cursor property.  
                        Console.WriteLine(Path.GetFullPath(openFileDialog1.FileName));
                        LicenseFilePath = Path.GetFullPath(openFileDialog1.FileName);
                        CardDetailsFile = CardDetailManager.ReadCardDetailFile(LicenseFilePath);
                        VMReturnData.VMCardDetails = CardDetailsFile.FCardDetails;
                        VMReturnData.NewCardUID = CardDetailsFile.FCardDetails.cardUID;
    }

                    //
                }

                if(CardDetailsFile.FCardDetails == null)
                {
                    WarningMessageF = "Please select a license file.";
                    CardUidPresVis = Visibility.Collapsed;
                }
                else
                {
                    CardUidPresVis = Visibility.Visible;
                }

                if (!VMReturnData.CardInField)
                {
                    if(CardDetailsFile.FCardDetails != null)
                        WarningMessageI = "Present RFID card with UID: " + VMReturnData.cardInfoRead.UIDtoString(CardDetailsFile.FCardDetails.cardUID);
                    MessageColour = Brushes.OrangeRed;
                }
                else if(CardDetailsFile.FCardDetails != null && CardDetailsFile.FCardDetails.cardUID == VMReturnData.UID)
                {

                    VMReturnData.VMCardDetails.IssuerUID = CardinFieldUID = VMReturnData.UID;
                    MernokPasswordFile mernokPasswordFile = MernokPasswordManager.ReadMernokPasswordFile(V);
                    bool password = PasswordFinder.FindPasswordinFile(AdminPassword, mernokPasswordFile);
                    if (password)
                    {
                        PassColour = Brushes.GreenYellow;
                        
                        string[] IssuerDetails = /*{ "1", "piet", "1", "2" };*/PasswordDecriptor.PasswordToDetails(AdminPassword);
                        VMReturnData.VMCardDetails.EngineerName = IssuerDetails[3];
                        VMReturnData.VMCardDetails.EngineerUID = UInt32.Parse(IssuerDetails[0]);
                        VMReturnData.NextButtonEnabled = true;
                        WarningMessageI = "Password good, click next to continue";
                        MessageColour = Brushes.White;

                    }
                    else
                    {
                        MessageColour = Brushes.OrangeRed;
                        PassColour = Brushes.OrangeRed;
                        VMReturnData.NextButtonEnabled = false;
                        if(AdminPassword == "")
                            WarningMessageI = "Enter your password";
                        else
                            WarningMessageI = "Enter correct password";
                    }

                }

            }
            else
            {
                //View is not visible, do not update
                //Stop any animations on this vieModel
                this.View.Visibility = Visibility.Collapsed;
                AdminPassword = "";
            }
        }


        #region Binding Properties
        private string _warnMessage;

        public string WarningMessageI
        {
            get { return _warnMessage; }
            set { _warnMessage = value; RaisePropertyChanged("WarningMessageI"); }
        }

        private string _issuerUID;

        public string IssuerUID
        {
            get { return _issuerUID; }
            set { _issuerUID = value; base.RaisePropertyChanged("IssuerUID"); }
        }

        private string _issueDate;
        public string IssueDate
        {
            get { return _issueDate; }
            set { _issueDate = value; base.RaisePropertyChanged("IssueDate"); }
        }

        private uint _engineerUID;
        public uint EngineerUID
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

        private string _password;

        public string AdminPassword
        {
            get { return _password; }
            set { _password = value; base.RaisePropertyChanged("AdminPassword"); }
        }

        private Brush _PassColour;

        public Brush PassColour
        {
            get { return _PassColour; }
            set { _PassColour = value; base.RaisePropertyChanged("PassColour"); }
        }

        private Brush _MessageColour;

        public Brush MessageColour
        {
            get { return _MessageColour; }
            set { _MessageColour = value; base.RaisePropertyChanged("MessageColour"); }
        }

        private string _WarningMessageF;

        public string WarningMessageF
        {
            get {return _WarningMessageF; }
            set { _WarningMessageF = value; RaisePropertyChanged("WarningMessageF"); }
        }

        private Visibility _CardUidPresVis;

        public Visibility CardUidPresVis
        {
            get { return _CardUidPresVis; }
            set { _CardUidPresVis = value; RaisePropertyChanged("CardUidPresVis"); }
        }


        #endregion
    }
}
