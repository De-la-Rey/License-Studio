using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Mernok_RFID_Licence_Studio
{
    public class DefaultDisplayManager
    {

     
        bool[] VMReturnBoolData = new bool[6];


        public IDictionary<string, ViewModel> ViewModels { get; set; }

        

        public DefaultDisplayManager(IDictionary<string, ViewModel> viewModels)
        {
            this.ViewModels = viewModels;
        }

        #region Animations
        public void StartAnimation(string animationName, UserControl view)
        {
            Storyboard sb = view.FindResource(animationName) as Storyboard;
            sb.Begin();
        }

        public void StopAnimation(string animationName, UserControl view)
        {
            Storyboard sb = view.FindResource(animationName) as Storyboard;
            sb.Stop();
        }
        #endregion

        public void Display( ViewModelReturnData VMReturnData)
        {

            
            //Update all the views, and make only visible the set View to display
            foreach (var model in ViewModels)
            {
                model.Value.View.Dispatcher.Invoke(() =>
                {
                    //Update vieModels
                    model.Value.Update(VMReturnData);


                    //if (model.Value.View.Name != "Main")
                    //{ 
                    //    if (VMReturnData.LicenceView_Active)
                    //        model.Value.View.Visibility = Visibility.Visible;
                    //    else
                    //        model.Value.View.Visibility = Visibility.Collapsed;

                    //    if (VMReturnData.StartUpView_Active)
                    //        model.Value.View.Visibility = Visibility.Visible;
                    //    else
                    //        model.Value.View.Visibility = Visibility.Collapsed;

                    //}
                    //Make other views invisible
                    //if (model.Value.View.Name != "Main")
                    //    model.Value.View.Visibility = (model.Value.View.Name == ActiveView.View.Name) ? Visibility.Visible : Visibility.Collapsed;
                    //if (model.Key != "Main")// && model.Key != "varkie")
                });
            }

        }


   

        
    }
}
