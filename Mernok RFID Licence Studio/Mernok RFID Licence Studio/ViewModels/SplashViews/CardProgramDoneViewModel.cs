using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Mernok_RFID_Licence_Studio
{
    class CardProgramDoneViewModel : ViewModel
    {
        private CardProgramDoneView _viewInstance;

        DispatcherTimer LFtagLost_Splash = new DispatcherTimer();
        private bool splash_done;
        private int splashTime;
        private bool splash = false;

        public CardProgramDoneViewModel(UserControl control) : base(control)
        {

            control.DataContext = this;
            _viewInstance = (CardProgramDoneView)control;
            LFtagLost_Splash.Tick += new EventHandler(LFtagLost_Splash_Tick);
            LFtagLost_Splash.Interval = new TimeSpan(0, 0, 0, 1);
        }


        public override void Update(ViewModelReturnData VMReturnData)
        {
            //Only update this view if visible

            if (VMReturnData.CardProramed_done)
            {
                this.View.Visibility = Visibility.Visible;
                VMReturnData.NavigationBar_Active = false;
                
                if (!splash)
                {
                    splash = true;
                    LFtagLost_Splash.Start();                   
                }

                if (splash_done)
                {
                    VMReturnData.CardProramed_done = false;
                    splash_done = false;
                }

            }
            else
            {
                this.View.Visibility = Visibility.Collapsed;
                splash = false;
            }
                

        }
        #region Splash timer
        private void LFtagLost_Splash_Tick(object sender, EventArgs e)
        {
            splashTime++;
            if (splashTime > 2)
            {
                splash_done = true;
                LFtagLost_Splash.Stop();
            }
        }
        #endregion
    }


}
