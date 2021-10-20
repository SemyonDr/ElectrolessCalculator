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
    /// Interaction logic for CurrentVolumePresenter.xaml
    /// </summary>
    public partial class CurrentVolumePresenter : UserControl
    {
        public float Volume
        {
            get { return (float)GetValue(VolumeProperty); }
            set { SetValue(VolumeProperty, value); }
        }

        public static readonly DependencyProperty VolumeProperty =
            DependencyProperty.Register("Volume", typeof(float), typeof(CurrentVolumePresenter), new PropertyMetadata(0.0f));



        public ICommand SetVolumeFractionCommand {
            get { return (ICommand)GetValue(SetVolumeFractionCommandProperty); }
            set { SetValue(SetVolumeFractionCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SetVolumeFractionCommand .  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SetVolumeFractionCommandProperty =
            DependencyProperty.Register("SetVolumeFractionCommand", typeof(ICommand), typeof(CurrentVolumePresenter), new PropertyMetadata(null));



        public CurrentVolumePresenter()
        {
            InitializeComponent();
        }
    }
}
