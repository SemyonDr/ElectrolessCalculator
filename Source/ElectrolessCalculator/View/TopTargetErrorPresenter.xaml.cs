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
    /// Interaction logic for TargetErrorPresenter.xaml
    /// </summary>
    public partial class TopTargetErrorPresenter : UserControl
    {

        public List<ViewModel.TargetEditError> EditErrors
        {
            get { return (List<ViewModel.TargetEditError>)GetValue(EditErrorsProperty); }
            set { SetValue(EditErrorsProperty, value); }
        }

        public static readonly DependencyProperty EditErrorsProperty =
            DependencyProperty.Register("EditErrors", typeof(List<ViewModel.TargetEditError>), typeof(TopTargetErrorPresenter), new PropertyMetadata(null));


        public bool IsEditValid
        {
            get { return (bool)GetValue(IsEditValidProperty); }
            set { SetValue(IsEditValidProperty, value); }
        }

        public static readonly DependencyProperty IsEditValidProperty =
            DependencyProperty.Register("IsEditValid", typeof(bool), typeof(TopTargetErrorPresenter), new PropertyMetadata(true));



        public TopTargetErrorPresenter()
        {
            InitializeComponent();
        }
    }
}
