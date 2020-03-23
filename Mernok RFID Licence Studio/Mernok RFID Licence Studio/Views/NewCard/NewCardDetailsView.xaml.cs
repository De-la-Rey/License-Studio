using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mernok_RFID_Licence_Studio
{
    /// <summary>
    /// Interaction logic for NewCardDetails1View.xaml
    /// </summary>
    public partial class NewCardDetails1View : UserControl
    {
        public NewCardDetails1View()
        {
            InitializeComponent();
        }

        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BindingExpression binding = ((ComboBox)sender).GetBindingExpression(ComboBox.SelectedIndexProperty);
            binding.UpdateSource();
        }
    }
}
