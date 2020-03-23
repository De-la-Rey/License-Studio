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
    public class ProgramPromptViewModel : ViewModel
    {
        private ProgramPromptView _viewInstance;

        public ICommand OkButton { get; private set; }

        public ICommand NoButton { get; private set; }

        private bool OkButtonPressed = false;

        private bool NoButtonPressed = false;

        public ProgramPromptViewModel(UserControl control) : base(control)
        {
            OkButton = new DelegateCommand(OkButtonHandler);
            NoButton = new DelegateCommand(NoButtonHandler);
            control.DataContext = this;
            _viewInstance = (ProgramPromptView)control;
        }

        public void OkButtonHandler()
        {
            OkButtonPressed = true;
        }

        public void NoButtonHandler()
        {
            NoButtonPressed = true;
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {
            //Only update this view if visible

            if (VMReturnData.ProgramPromtView_Active)
            {
                this.View.Visibility = Visibility.Visible;

                if(VMReturnData.NewIssuerCard)
                {
                    ProgramPrompt = "Do you wish to save this card file?";
                }
                else
                {
                    ProgramPrompt = "Do you wish to Program this card?";
                }

                if (OkButtonPressed)
                {
                    OkButtonPressed = false;
                    VMReturnData.ProgramPromtView_Active = false;
                    if (!VMReturnData.NewIssuerCard)
                    {
                        if (VMReturnData.CardInfoWrite.WriteInfoToCard(VMReturnData.VMCardDetails, VMReturnData.CardType) == 100)
                        {
                            VMReturnData.CardProramed_done = true;
                            VMReturnData.App_datareset();
                        }
                        else
                        {
                            VMReturnData.CardProgramFail = true;
                            VMReturnData.NewCardWindow--;
                        }
                    }
                    else
                    {
                        CardDetailsFile cardDetailsFile = new CardDetailsFile();
                        cardDetailsFile.createdBy = "N. Pretorius";
                        cardDetailsFile.dateCreated = DateTime.Now;
                        cardDetailsFile.version = 1;
                        VMReturnData.VMCardDetails.cardUID = VMReturnData.NewIssuerUID;
                        cardDetailsFile.FCardDetails = VMReturnData.VMCardDetails;
                        CardDetailManager.CreateCardDetailFile(cardDetailsFile);

                        VMReturnData.CardProramed_done = true;
                        VMReturnData.App_datareset();


                    }

                }

                if (NoButtonPressed)
                {
                    NoButtonPressed = false;
                    VMReturnData.ProgramPromtView_Active = false;
                    VMReturnData.NewCardWindow--;
                }
            }
            else
                this.View.Visibility = Visibility.Collapsed;

        }


        private string _ProgramPrompt;

        public string ProgramPrompt
        {
            get { return _ProgramPrompt; }
            set { _ProgramPrompt = value; RaisePropertyChanged("ProgramPrompt"); }
        }

    }
}
