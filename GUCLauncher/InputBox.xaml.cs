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
using System.Windows.Shapes;

namespace GUCLauncher
{
    /// <summary>
    /// Interaktionslogik für InputBox.xaml
    /// </summary>
    public partial class InputBox : Window
    {
        public InputBox()
        {
            InitializeComponent();
        }
        
        public static string Show(Window parent, string title, string input = "")
        {
            var win = new InputBox();
            win.Left = parent.Left + (parent.Width - win.Width) / 2;
            win.Top = parent.Top + (parent.Height - win.Height) / 2;
            win.Title = title;
            win.lbTitle.Content = title;
            win.tbInput.Text = input;
            win.tbInput.Focus();
            return win.ShowDialog() == true ? win.tbInput.Text : null;
        }

        void Click_Accept(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        void Click_Return(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
