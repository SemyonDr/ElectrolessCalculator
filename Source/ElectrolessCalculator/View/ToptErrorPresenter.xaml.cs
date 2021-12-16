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
    /// Interaction logic for TopErrorPresenter.xaml
    /// </summary>
    public partial class TopErrorPresenter : UserControl
    {

        public List<ViewModel.InputError> EditErrors
        {
            get { return (List<ViewModel.InputError>)GetValue(EditErrorsProperty); }
            set { SetValue(EditErrorsProperty, value); }
        }

        public static readonly DependencyProperty EditErrorsProperty =
            DependencyProperty.Register("EditErrors", typeof(List<ViewModel.InputError>), typeof(TopErrorPresenter), new PropertyMetadata(null));


        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(TopErrorPresenter), new PropertyMetadata(true));



        public TopErrorPresenter()
        {
            InitializeComponent();
        }
    }
}
