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

namespace ElectrolessCalculator.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AboutWindow AboutWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AboutClick(object sender, RoutedEventArgs e) {
            if (AboutWindow == null) {
                AboutWindow = new AboutWindow();
                AboutWindow.Owner = this;
                AboutWindow.Closed += AboutWindow_Closed;
                AboutWindow.ShowDialog();
            }
        }

        private void AboutWindow_Closed(object sender, EventArgs e)
        {
            this.AboutWindow = null;
        }
    }
}