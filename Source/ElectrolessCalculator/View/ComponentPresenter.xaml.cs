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
    /// Interaction logic for ComponentPresenter.xaml
    /// </summary>
    public partial class ComponentPresenter : UserControl
    {


        public string ComponentName
        {
            get { return (string)GetValue(ComponentNameProperty); }
            set { SetValue(ComponentNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ComponentName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ComponentNameProperty =
            DependencyProperty.Register("ComponentName", typeof(string), typeof(ComponentPresenter), new PropertyMetadata(""));



        public float ComponentValue
        {
            get { return (float)GetValue(ComponentValueProperty); }
            set { SetValue(ComponentValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ComponentValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ComponentValueProperty =
            DependencyProperty.Register("ComponentValue", typeof(float), typeof(ComponentPresenter), new PropertyMetadata(0.0f));



        public string ValueUnits
        {
            get { return (string)GetValue(ValueUnitsProperty); }
            set { SetValue(ValueUnitsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueUnits.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueUnitsProperty =
            DependencyProperty.Register("ValueUnits", typeof(string), typeof(ComponentPresenter), new PropertyMetadata(""));



        public ComponentPresenter()
        {
            InitializeComponent();
        }
    }
}
