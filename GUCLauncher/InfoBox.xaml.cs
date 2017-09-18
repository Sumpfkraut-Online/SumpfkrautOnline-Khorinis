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
    /// Interaktionslogik für InfoBox.xaml
    /// </summary>
    public partial class InfoBox : Window
    {
        public InfoBox()
        {
            InitializeComponent();
        }

        public static void Show(Window parent, string title)
        {
            var win = new InfoBox();
            win.Left = parent.Left + (parent.Width - win.Width) / 2;
            win.Top = parent.Top + (parent.Height - win.Height) / 2;
            win.tbText.Text = title;
            win.ShowDialog();
        }

        void Click_Accept(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
