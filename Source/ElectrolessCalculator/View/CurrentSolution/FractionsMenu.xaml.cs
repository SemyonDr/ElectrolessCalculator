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
    /// Interaction logic for FractionsMenu.xaml
    /// </summary>
    public partial class FractionsMenu : UserControl
    {
        public event EventHandler ButtonClickedEvent;

        public FractionsMenu() {
            InitializeComponent();
        }

        
        private void Button_Click(object sender, RoutedEventArgs e) {
            if (ButtonClickedEvent != null) {
                ButtonClickedEvent.Invoke(this, new EventArgs());
            }
        }
    }
}
