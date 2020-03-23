using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mernok_RFID_Licence_Studio
{
    class NewIssuerPresentViewModel : ViewModel
    {
        private NewIssuerPresentView _viewInstance;

        static RFIDCardInfoRead cardInfoRead = new RFIDCardInfoRead();

        private bool formated;

        public NewIssuerPresentViewModel(UserControl control) : base(control)
        {

            control.DataContext = this;
            _viewInstance = (NewIssuerPresentView)control;
        }

        private string _warnMessage;

        public string WarningMessage
        {
            get { return _warnMessage; }
            set { _warnMessage = value; base.RaisePropertyChanged("WarningMessage"); }
        }
        private Brush _MessageColour;

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

        public Brush MessageColour
        {
            get { return _MessageColour; }
            set { _MessageColour = value; base.RaisePropertyChanged("MessageColour"); }
        }

        private BitmapImage _CardImage;

        public BitmapImage CardImage
        {
            get { return _CardImage; }
            set { _CardImage = value; base.RaisePropertyChanged("CardImage"); }
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {
            //Only update this view if visible

            if (VMReturnData.NewIssuerPresent_Active)
            {
                this.View.Visibility = Visibility.Visible;
                #region navigation bar details
                VMReturnData.NavigationBar_Active = true;
                VMReturnData.ViewTitle = "Issuer Card";
                VMReturnData.SubTitle = "Issuer select";
                VMReturnData.MenuButtonEnabled = Visibility.Hidden;
                VMReturnData.HelpButtonEnabled = Visibility.Visible;
                VMReturnData.BackButtonEnabled = true;
                #endregion

                if(VMReturnData.NewCardWindow<1)
                {
                    if (VMReturnData.CardInField)
                    {
                        cardInfoRead.UID = VMReturnData.UID;
                        formated = cardInfoRead.Block1Info();
                        UID = cardInfoRead.UIDtoString(VMReturnData.UID);
                        if (formated)
                        {
                            CardImage = new BitmapImage(new Uri(@"/Resources/Images/CardFormatError.png", UriKind.Relative));
                            WarningMessage = "Card formated, present unformated card.";
                            MessageColour = Brushes.OrangeRed;
                        }
                        else
                        {
                            CardImage = new BitmapImage(new Uri(@"/Resources/Images/CardValid.png", UriKind.Relative));
                            VMReturnData.NextButtonEnabled = true;
                            MessageColour = Brushes.White;
                            WarningMessage = "Card VALID: Click next to continue";
                        }

                    }
                    else
                    {
                        ClearDetails();
                        VMReturnData.NextButtonEnabled = false;
                    }
                }

            }
            else
            {
                //View is not visible, do not update
                //Stop any animations on this vieModel
                View.Visibility = Visibility.Collapsed;
                VMReturnData.NewIssuerCard = false;
                VMReturnData.NewIssuerUID = 0;
            }

        }

        private void ClearDetails()
        {
            UID = "";
            WarningMessage = "Present new card";
            this.CardImage = new BitmapImage(new Uri(@"/Resources/Images/PresentCard.png", UriKind.Relative));
            MessageColour = Brushes.OrangeRed;
        }
    }
}
