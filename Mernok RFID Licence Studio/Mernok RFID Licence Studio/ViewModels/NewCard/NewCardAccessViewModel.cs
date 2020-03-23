using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Threading;
using static Mernok_RFID_Licence_Studio.RFIDCardInfoRead;
using System.Windows.Media.Imaging;

namespace Mernok_RFID_Licence_Studio
{
    public class NewCardAccessViewModel : ViewModel
    {
        private NewCardAccessView _viewInstance;

        static RFIDCardInfoRead cardInfoRead = new RFIDCardInfoRead();

        static ulong CardinFieldUID = 0;
        bool EngineerName_good = false;
        bool EngineerUID_good = false;
        bool Accesslevel_good = false;
        bool Issuer_WarnDate_good = false;
        bool onetimeread = false;

        DateTime lastYear = DateTime.Today.AddYears(-1);

        public NewCardAccessViewModel(NewCardAccessView control) : base(control)
        {

            ClearDetails();
            control.DataContext = this;
             _viewInstance = (NewCardAccessView)control;
        }

        public void ClearDetails()
        {
            WarningMessage = "Present Issuer card";
            this.CardImage = new BitmapImage(new Uri(@"/Resources/Images/PresentCard.png", UriKind.Relative));
            MessageColour = Brushes.OrangeRed;
            IssuerUID = "";
            EngineerName = "";
            EngineerUID = "";
            IssueDate = "";
            AccessLevel = "";
            onetimeread = false;
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {
              
            if (VMReturnData.NewCardAccess_Active)
            {

                this.View.Visibility = Visibility.Visible;
                #region navigation bar details
                VMReturnData.ViewTitle = VMReturnData.EditCard ? "Edit Card" : "New Card";
                VMReturnData.SubTitle = "Issuer details";
                VMReturnData.CurrentPageNumber = 1;
                VMReturnData.TotalPageNumber = 4;
                VMReturnData.MenuButtonEnabled = Visibility.Hidden;
                VMReturnData.HelpButtonEnabled = Visibility.Visible;
                VMReturnData.BackButtonEnabled = false;
                #endregion

                #region menu buttons
                VMReturnData.MenuEditBtnEnabled = Visibility.Collapsed;
                VMReturnData.MenuIssueBtnEnabled = Visibility.Visible;
                MessageColour = Brushes.OrangeRed;
                #endregion


                

                if(VMReturnData.CardInField && ((VMReturnData.UID != VMReturnData.EditCardUID && VMReturnData.EditCard) || VMReturnData.UID != VMReturnData.NewCardUID))
                {
                    if (!onetimeread)
                    {
                        CardinFieldUID = VMReturnData.UID;
                        cardInfoRead.cardDetails.IssuerUID = CardinFieldUID;
                        if (cardInfoRead.ReadIssuer(CardinFieldUID))
                        {
                            #region Warning messages
                            MessageColour = Brushes.OrangeRed;
                            if ((char)cardInfoRead.cardDetails.AccessLevel == 9 || (char)cardInfoRead.cardDetails.AccessLevel == 13 || (char)cardInfoRead.cardDetails.AccessLevel == 14 || (char)cardInfoRead.cardDetails.AccessLevel == 'C' || (char)cardInfoRead.cardDetails.AccessLevel == 'Z')
                            {
                                Accesslevel_good = true;
                                VMReturnData.IssuerAccess = cardInfoRead.cardDetails.AccessLevel;
                            }
                            else
                            {
                                Accesslevel_good = false;
                                WarningMessage = "Issuer Card has insufficient access";
                            }

                            if (cardInfoRead.cardDetails.EngineerName != "")
                            {
                                EngineerName_good = true;
                            }
                            else
                            {
                                EngineerName_good = false;
                                WarningMessage = "Issuer name is undefined";
                            }

                            if (cardInfoRead.cardDetails.IssuerUID != 0)
                            {
                                EngineerUID_good = true;

                            }
                            else
                            {
                                EngineerUID_good = false;
                                WarningMessage = "Issuer UID undefined";
                            }

                            if (cardInfoRead.cardDetails.EngineerUID != 0)
                            {
                                EngineerUID_good = true;
                            }
                            else
                            {
                                EngineerUID_good = false;
                                WarningMessage = "Engineer UID undefined";
                            }

                            if ((lastYear < cardInfoRead.cardDetails.Issue_Date) && (cardInfoRead.cardDetails.Expiry_Date > DateTime.Now))
                            {
                                Issuer_WarnDate_good = true;
                            }
                            else
                            {
                                Issuer_WarnDate_good = false;
                                WarningMessage = "Issuer Card has expired";
                            }
                            #endregion
                            CardImage = new BitmapImage(new Uri(@"/Resources/Images/CArdInvalid.png", UriKind.Relative));
                            onetimeread = true;                          
                        }
                        else
                        {
                            ClearDetails();
                            WarningMessage = "Issuer Card not formatted";
                            CardImage = new BitmapImage(new Uri(@"/Resources/Images/CardFormatError.png", UriKind.Relative));
                        }
                       
                    }
                    if (onetimeread)
                    {

                        IssueDate = cardInfoRead.cardDetails.Issue_Date.ToShortDateString();
                        IssuerUID = cardInfoRead.UIDtoString(CardinFieldUID);
                        EngineerName = cardInfoRead.cardDetails.EngineerName;
                        EngineerUID = cardInfoRead.UIDtoString(cardInfoRead.cardDetails.EngineerUID);
                        AccessLevel = ((AccessLevel_enum)cardInfoRead.cardDetails.AccessLevel).ToString().Replace("_"," ");

                        if (EngineerName_good && EngineerUID_good && Issuer_WarnDate_good && Accesslevel_good && VMReturnData.NewCardWindow < 1)
                        {
                            VMReturnData.NextButtonEnabled = true;
                            MessageColour = Brushes.White;
                            WarningMessage = "Card VALID: Click next to continue";
                            CardImage = new BitmapImage(new Uri(@"/Resources/Images/CardValid.png", UriKind.Relative));
                            VMReturnData.VMCardDetails.EngineerName = cardInfoRead.cardDetails.EngineerName;
                            VMReturnData.VMCardDetails.IssuerUID = cardInfoRead.cardDetails.IssuerUID;
                            VMReturnData.VMCardDetails.EngineerUID = cardInfoRead.cardDetails.EngineerUID;
                            VMReturnData.VMCardDetails.Client_Group = cardInfoRead.cardDetails.Client_Group;
                            VMReturnData.VMCardDetails.Client_Site = cardInfoRead.cardDetails.Client_Site;

                        }
                        else
                        {
                            //WarningMessage = "Issuer Card has expired";
                            
                        }
                    }
                }
                else
                {
                    ClearDetails();
                    VMReturnData.NextButtonEnabled = false;
                    onetimeread = false;
                }                   

            }
            else
            {
                //View is not visible, do not update
                //Stop any animations on this vieModel
                View.Visibility = Visibility.Collapsed;
                onetimeread = false;
                VMReturnData.NextButtonEnabled = false;
            }
        }

        #region Binding Properties

        private BitmapImage _CardImage;

        public BitmapImage CardImage
        {
            get { return _CardImage; }
            set { _CardImage = value; base.RaisePropertyChanged("CardImage"); }
        }

        private string _AccessLevel;

        public string AccessLevel
        {
            get { return _AccessLevel; }
            set { _AccessLevel = value; base.RaisePropertyChanged("AccessLevel"); }
        }


        private string _warnMessage;

        public string WarningMessage
        {
            get { return _warnMessage; }
            set { _warnMessage = value; base.RaisePropertyChanged("WarningMessage"); }
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

        private string _engineerUID;
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

        private Brush _MessageColour;

        public Brush MessageColour
        {
            get { return _MessageColour; }
            set { _MessageColour = value; base.RaisePropertyChanged("MessageColour"); }
        }

        #endregion

    }
}
