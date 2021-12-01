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
    /// Interaction logic for TargetVolumePresenter.xaml
    /// </summary>
    public partial class TargetVolumePresenter : UserControl
    {
        public float Volume
        {
            get { return (float)GetValue(VolumeProperty); }
            set { SetValue(VolumeProperty, value); }
        }

        public static readonly DependencyProperty VolumeProperty =
            DependencyProperty.Register("Volume", typeof(float), typeof(TargetVolumePresenter), new PropertyMetadata(0.0f));

        public string EditVolume
        {
            get { return (string)GetValue(EditVolumeProperty); }
            set { SetValue(EditVolumeProperty, value); }
        }

        public static readonly DependencyProperty EditVolumeProperty =
            DependencyProperty.Register("EditVolume", typeof(string), typeof(TargetVolumePresenter), new PropertyMetadata(""));


        public TargetVolumePresenter()
        {
            InitializeComponent();
        }
    }
}
