using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using USB_PORTS;

namespace Mernok_RFID_Licence_Studio
{
    public class MainViewModel : ViewModel
    {
        public MainView MyWindow;

        public ICommand Backbtn { get; private set; }
        public ICommand Nextbtn { get; private set; }

        private bool BackbtnPressed = false;
        private bool NextbtnPressed = false;
        //static string VID = "0403", PID = "7E40";

        public MainViewModel(Window window) : base(window)
        {
            MyWindow = (MainView)window;
            

            Backbtn = new DelegateCommand(BackbtnHandler);
            Nextbtn = new DelegateCommand(NextbtnHandler);

            MyWindow.DataContext = this;
            MyWindow.Show();

        }

        public override void Update(ViewModelReturnData VMReturnData)
        {
            //List<string> ports = PortFromVIDPID.ComPortNames(VID, PID);
            //if(ports.Count != VMReturnData.NumberofRWD)
            //{
            //    if (RFID.MernokRFID.OpenRFID(RFID.Mode.Mifare))
            //    {
            //        VMReturnData.RWD_connected = true;
            //    }
            //    else
            //    {
            //        VMReturnData.RWD_connected = false;
            //    }
            //    VMReturnData.NumberofRWD = ports.Count;
            //}

 //           

            if (!RFID.MernokRFID.IsOpen())
            {
                if (RFID.MernokRFID.OpenRFID(RFID.Mode.Mifare))
                {
                    VMReturnData.RWD_connected = true;
                }
                else
                {
                    VMReturnData.RWD_connected = false;
                }
            }
            else
            {
                VMReturnData.UID = BitConverter.ToUInt64(RFID.MernokRFID_interface.read_UID(),0);
                if (VMReturnData.UID != 0)
                {
                    VMReturnData.CardInField = true;

                    if (RFID.MernokRFID_interface.read_Status() == 0x86)
                        VMReturnData.CardType = 1;
                    else if(RFID.MernokRFID_interface.read_Status() == 0x96)
                        VMReturnData.CardType = 4;

                }
                else
                {
                    VMReturnData.CardInField = false;
                }

                if (VMReturnData.EditCard && !VMReturnData.NewIssuerCard && VMReturnData.EditCardUID != VMReturnData.UID && VMReturnData.NewCardWindow>=1)
                {
                    VMReturnData.EditCardWarn_Active = true;
                }
                else
                    VMReturnData.EditCardWarn_Active = false;

                if (!VMReturnData.EditCard && !VMReturnData.NewIssuerCard && VMReturnData.NewCardUID != VMReturnData.UID && VMReturnData.NewCardWindow >= 1)
                {
                    VMReturnData.NewCardWarn_Active = true;
                }
                else
                    VMReturnData.NewCardWarn_Active = false;

                //if (!VMReturnData.EditCard && VMReturnData.NewIssuerCard && VMReturnData.NewIssuerUID != VMReturnData.UID && VMReturnData.NewCardWindow >= 1)
                //{
                //    VMReturnData.NewCardWarn_Active = true;
                //}
                //else
                //    VMReturnData.NewCardWarn_Active = false;

            }

            VMReturnData.StartUpView_Active = VMReturnData.RWD_connected;

            #region Navigation Bar Button Setter

            if (VMReturnData.LicenceView_Active)
            {
                VMReturnData.NavigationBar_Active = true;
            }
            else if (VMReturnData.NewCardDetail_Active)
            {
                VMReturnData.NavigationBar_Active = true;
            }
            else if(VMReturnData.NewCardAccess_Active)
            {
                VMReturnData.NavigationBar_Active = true;
            }
            else if (VMReturnData.NewIssuerPresent_Active)
            {
                VMReturnData.NavigationBar_Active = true;
            }
            else if (VMReturnData.NewCardIssuer_Active)
            {
                VMReturnData.NavigationBar_Active = true;
            }
            else
            {
                VMReturnData.NavigationBar_Active = false;
            }

            VMReturnData.CurrentPageNumber = VMReturnData.NewCardWindow+1;
            VMReturnData.TotalPageNumber = 4;

            #endregion


            if (NextbtnPressed)
            {
                VMReturnData.NextWindow();
                NextbtnPressed = false;
            }
            if(BackbtnPressed)
            {
                VMReturnData.BackWindow();
                BackbtnPressed = false;
            }

        }

#region Properties
        private string _cardInfield;

        public string CardInField
        {
            get { return _cardInfield; }
            set { _cardInfield = value;
                base.RaisePropertyChanged("CardInField");
            }
        }
        #endregion

        public void BackbtnHandler()
        {
            BackbtnPressed = true;
        }

        public void NextbtnHandler()
        {
            NextbtnPressed = true;
        }


    }
}
