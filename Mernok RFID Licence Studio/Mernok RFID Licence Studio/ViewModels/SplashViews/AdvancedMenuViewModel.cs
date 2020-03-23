using RFID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mernok_RFID_Licence_Studio
{
    class AdvancedMenuViewModel : ViewModel
    {
        private AdvancedMenuView _viewInstance;

        public ICommand HotFlag { get; private set; }

        public ICommand Exitbtn { get; private set; }

        public ICommand Formatbtn { get; private set; }

        public ICommand MernokCard { get; private set; }

        private bool _HotflagBtnPressed = false;

        private bool _FormatbtnPressed = false;

        private bool _buttonExitPressed = false;

        private bool _mernokcardBtnPressed = false;

        public AdvancedMenuViewModel(UserControl control) : base(control)
        {

            Exitbtn = new DelegateCommand(ExitBtnHandler);
            HotFlag = new DelegateCommand(HotFlagBtnHandler);
            Formatbtn = new DelegateCommand(Formatbtnhandler);
            MernokCard = new DelegateCommand(MernokCardbtnHandler);
            control.DataContext = this;
            _viewInstance = (AdvancedMenuView)control;
        }

        private void MernokCardbtnHandler()
        {
            _mernokcardBtnPressed = true;
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {
            //Only update this view if visible

            if (VMReturnData.AdvancedMenu_Active)
            {
                VMReturnData.OptionsPressed = true;
                this.View.Visibility = Visibility.Visible;


                if (_HotflagBtnPressed)
                {
                    CardDetails WriteCardDetails = new CardDetails() { Hotflaged_status = true, HotFlagedDate = DateTime.Now, HotFlagedVID = 123 };
                    RFIDCardInfoWrite rFIDCardInfoWrite = new RFIDCardInfoWrite();
                    _HotflagBtnPressed = false;
                    byte[] temp = rFIDCardInfoWrite.Block9(WriteCardDetails);
                    if (MernokRFID_interface.Mifare_Write_Block(Mifare_key.A, 0, WriteCardDetails.Block9_Addr, temp))
                    {

                    }

                    VMReturnData.CardProramed_done = true;
                    VMReturnData.App_datareset();
                }

                if(_mernokcardBtnPressed)
                {
                    _mernokcardBtnPressed = false;
                    VMReturnData.MernokCardPresent_active = true;
                    VMReturnData.NewMernokCard = true;
                }

                if (_FormatbtnPressed)
                {
                    _FormatbtnPressed = false;
                    RFIDCardInfoWrite rFIDCardInfoWrite = new RFIDCardInfoWrite();
                    CardDetails WriteCardDetails = new CardDetails()
                    {
                        cardUID = 0,
                        AccessLevel = 0,
                        ByPassBits = 0,
                        Client_Group = 0,
                        Client_Site = 0,
                        EngineerName = "",
                        EngineerUID = 0,
                        Expiry_Date = DateTime.MinValue,
                        HotFlagedDate = DateTime.MinValue,
                        HotFlagedVID = 0,
                        Hotflaged_status = false,
                        IssuerUID = 0,
                        Issue_Date = DateTime.MinValue,
                        OperationalArea = 0,
                        OperatorName = "",
                        Options = 0,
                        ProductCode = 0,
                        Training_Date = DateTime.MinValue,
                        VehicleGroup = Enumerable.Repeat((byte)0, 16).ToArray(),
                        VID = Enumerable.Repeat((UInt16)0, 16).ToArray(),
                        VehicleNames = Enumerable.Repeat("", 16).ToArray(),
                        VehicleLicenceType = Enumerable.Repeat((uint)0, 32).ToArray(),
                        Warning_Date = DateTime.MinValue
                    };
                    rFIDCardInfoWrite.WriteInfoToCard(WriteCardDetails, VMReturnData.CardType);

                    VMReturnData.CardProramed_done = true;
                    VMReturnData.App_datareset();
                }


                #region Exit button
                else if (_buttonExitPressed)
                {
                    _buttonExitPressed = false;
                    VMReturnData.AdvancedMenu_Active = false;
                }
                #endregion
            }
            else
            {
                VMReturnData.OptionsPressed = false;
                this.View.Visibility = Visibility.Collapsed;
            }


        }

        private void Formatbtnhandler()
        {
            _FormatbtnPressed = true;
        }

        private void HotFlagBtnHandler()
        {
            _HotflagBtnPressed = true;
        }

        private void ExitBtnHandler()
        {
            _buttonExitPressed = true;
        }
    }
}
