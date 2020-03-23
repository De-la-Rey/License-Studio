using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Mernok_RFID_Licence_Studio
{
    class NoRWDViewModel : ViewModel
    {
        private NoRWDView _viewInstance;



        public NoRWDViewModel(UserControl control) : base(control)
        {

            control.DataContext = this;
            _viewInstance = (NoRWDView)control;
        }


        public override void Update(ViewModelReturnData VMReturnData)
        {
            //Only update this view if visible

                if (!VMReturnData.RWD_connected)
                {
                    this.View.Visibility = Visibility.Visible;
                }
                else
                    this.View.Visibility = Visibility.Collapsed;

        }
    }


}
