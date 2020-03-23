#region windows defines
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Media;

#endregion 

namespace Mernok_RFID_Licence_Studio
{
    public class StartupViewModel : ViewModel
    {
        #region ViewModel Global varaibles
        private StartupView _viewInstance;
        RFIDCardInfoRead RFIDCardInfoRead = new RFIDCardInfoRead();
        DispatcherTimer ProgressBarTimer = new DispatcherTimer();

        public ICommand ReadCardInfobtn { get; private set; }
        public ICommand NewCardInfobtn { get; private set; }
        public ICommand Exitbtn { get; private set; }
        public ICommand Optionsbtn { get; private set; }

        private bool ReadCardPressed = false;
        private bool NewCardPressed = false;
        private bool OptionsPressed = false;
        private bool formated;
        private bool oneRead = false;

        public bool doneProgress = false;
        int default_max = 5;

        #endregion

        #region View Model Constructor
        public StartupViewModel(UserControl control) : base(control)
        {
            #region Button defines
            ReadCardInfobtn = new DelegateCommand(ReadCardInfoHandler);
            NewCardInfobtn = new DelegateCommand(NewCardInfoHandler);
            Exitbtn = new DelegateCommand(ExitBtnHandler);
            Optionsbtn = new DelegateCommand(OptionsbtnHandler);
            #endregion

            #region Progressbar init
            ProgressBarTimer.Tick += new EventHandler(ProgressBarTimer_Tick);
            ProgressBarTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            ReadProgress = default_max;
            #endregion

            readVis = Visibility.Collapsed;
            formated = false;
            control.DataContext = this;
            _viewInstance = (StartupView)control;
        }
        #endregion

        #region Progressbar run
        private void ProgressBarTimer_Tick(object sender, EventArgs e)
        {
            ReadProgress = default_max - ProgressBarValue++;
            if (ProgressBarValue > default_max)
            {
                ProgressBarTimer.Stop();
                doneProgress = true;
                readVis = Visibility.Collapsed;
            }
        }
        #endregion

        public override void Update(ViewModelReturnData VMReturnData)
        {
            //Only update this view if visible                        
            if (VMReturnData.StartUpView_Active)
            {
                this.View.Visibility = Visibility.Visible;
                #region Startup operation
                CardPresent = formated & VMReturnData.CardInField;
                if (VMReturnData.CardInField && !VMReturnData.OptionsPressed && !VMReturnData.NewIssuerCard)
                {

                    if (VMReturnData.prevUID == 0)
                    {
                        VMReturnData.prevUID = VMReturnData.UID;
                        readVis = Visibility.Collapsed;
                    }
                    else if (VMReturnData.prevUID != VMReturnData.UID)
                    {
                        VMReturnData.cardChanged = true;
                        VMReturnData.CardFormatError = false;
                        oneRead = false;
                        VMReturnData.prevUID = VMReturnData.UID;
                    }

                    if (VMReturnData.cardChanged && VMReturnData.LicenceView_Active && VMReturnData.EditCard != true)
                    {
                        VMReturnData.BackApp();                       
                        VMReturnData.cardChanged = false;
                    }
                    else if(VMReturnData.NewCardWindow < 1 && formated && !VMReturnData.NewCardAccess_Active)
                    {
                        VMReturnData.cardChanged = false;                        
                        if (!oneRead)
                        {
                            ReadCardInfoHandler();
                            readVis = Visibility.Visible;
                            oneRead = true;
                        }
                        
                    }
                    else if (VMReturnData.NewCardWindow < 1 && !formated && !VMReturnData.NewCardAccess_Active)
                    {
                        VMReturnData.cardChanged = false;
                        if (!oneRead)
                        {
                            NewCardInfoHandler();
                            readVis = Visibility.Visible;
                            oneRead = true;
                        }

                    }

                    UID = RFIDCardInfoRead.UIDtoString(VMReturnData.UID);
                    RFIDCardInfoRead.UID = VMReturnData.UID;
                    formated = RFIDCardInfoRead.Block1Info();
                                      
                    if (formated)
                        CardImage = new BitmapImage(new Uri(@"/Resources/Images/CardValid.png", UriKind.Relative));
                    else
                    {
                        CardImage = new BitmapImage(new Uri(@"/Resources/Images/CardFormatError.png", UriKind.Relative));
                        
                        //readVis = Visibility.Collapsed;
                    }
                    VMReturnData.cardInfoRead.formatted = formated;


                    if (ReadCardPressed == true)
                    {
                        ReadCardPressed = false;
                        doneProgress = false;
                        ProgressBarTimer.Start();
                        VMReturnData.cardInfoRead.ReadInfo(VMReturnData.UID);                                                                                
                    }

                    if (NewCardPressed == true)
                    {
                        //VMReturnData.NewCardAccess_Active = true;
                        ReadCardInfoHandler();
                        VMReturnData.EditCard = false;
                        VMReturnData.EditCardUID = 0;
                        VMReturnData.NewCardWindow = 0;
                        VMReturnData.NewCardUID = VMReturnData.UID;
                        NewCardPressed = false;
                    }
                }
                else
                {
                    UID = "Present RFID card";
                    ProgressBarValue = 0;
                    ReadProgress = default_max;
                    doneProgress = false;
                    oneRead = false;
                    this.CardImage = new BitmapImage(new Uri(@"/Resources/Images/PresentCard.png", UriKind.Relative));
                    ProgressBarTimer.Stop();
                    readVis = Visibility.Collapsed;
                }

                if (OptionsPressed)
                {
                    OptionsPressed = false;
                    VMReturnData.MenuView_Active = true;
                }

                if (doneProgress)
                {
                    if (!VMReturnData.cardInfoRead.formatted)
                    {
                        VMReturnData.CardFormatError = true;
                    }
                    else
                    {
                        VMReturnData.LicenceView_Active = true;
                    }

                    doneProgress = false;
                    ProgressBarValue = 2;
                    ReadProgress = default_max;
                }
                #endregion

                #region Exit button
                else if (_buttonExitPressed)
                {
                    _buttonExitPressed = false;
                    VMReturnData.ExitPromtView_Active = true;
                }
                #endregion

            }
            else
                this.View.Visibility = Visibility.Collapsed;

        }

        #region Button Handlers
        public void ReadCardInfoHandler()
        {
            ReadCardPressed = true;
        }

        public void NewCardInfoHandler()
        {
            NewCardPressed = true;
        }

        private void ExitBtnHandler()
        {
            _buttonExitPressed = true;
        }

        private void OptionsbtnHandler()
        {
            OptionsPressed = true;
        }
        #endregion

        #region Binding Properties
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

        private Visibility _PresentCard;

        public Visibility PresentCard
        {
            get { return _PresentCard; }
            set { _PresentCard = value; base.RaisePropertyChanged("PresentCard"); }
        }

        private Visibility _readVis;

        public Visibility readVis
        {
            get { return _readVis; }
            set { _readVis = value; base.RaisePropertyChanged("readVis"); }
        }


        private BitmapImage _CardImage;

        public BitmapImage CardImage
        {
            get { return _CardImage; }
            set { _CardImage = value; base.RaisePropertyChanged("CardImage"); }
        }

        private Brush _ProgressBarColour;

        public Brush ProgressBarColour
        {
            get { return _ProgressBarColour; }
            set { _ProgressBarColour = value; base.RaisePropertyChanged("ProgressBarColour"); }
        }

        private int _ReadProgress;
        private bool _buttonExitPressed;

        public int ReadProgress
        {
            get { return _ReadProgress; }
            set
            {
                _ReadProgress = value;
                base.RaisePropertyChanged("ReadProgress");
            }
        }

        private bool _cardPresent;

        public bool CardPresent
        {
            get { return _cardPresent; }
            set { _cardPresent = value; base.RaisePropertyChanged("CardPresent"); }
        }


        public int ProgressBarValue { get; set; } = 2;


        public int ProgressBarMax
        {
            get { return default_max; }
            set { default_max = value; base.RaisePropertyChanged("ProgressBarMax"); }
        }

        #endregion

    }

}
