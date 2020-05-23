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
    public partial class SolutionVolumePresenter : UserControl
    {
        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }}

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SolutionVolumePresenter), new PropertyMetadata(""));


        public float Volume {
            get { return (float)GetValue(VolumeProperty); }
            set { SetValue(VolumeProperty, value); }}

        public static readonly DependencyProperty VolumeProperty =
            DependencyProperty.Register("Volume", typeof(float), typeof(SolutionVolumePresenter), new PropertyMetadata(0.0f));



        public SolutionVolumePresenter()
        {
            InitializeComponent();
        }
    }
}
