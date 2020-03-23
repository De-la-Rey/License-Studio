using System.Windows.Controls;

namespace Mernok_RFID_Licence_Studio
{
    class ViewModelExample : ViewModel
    {
        //        private ViewName _viewInstance;

        public ViewModelExample(UserControl control) : base(control)
        {
            control.DataContext = this;
           
            //            _viewInstance = (ViewName)control;
        }

        public override void Update(ViewModelReturnData VMReturnData)
        {
            if (this.View.Visibility == System.Windows.Visibility.Visible)
            {
                //Only update this viewModel when this view is visible
            }
            else
            {
                //View is not visible, do not update
                //Stop any animations on this vieModel


            }
        }
    }
}
