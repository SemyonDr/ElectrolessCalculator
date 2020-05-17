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
using System.ComponentModel;

namespace ElectrolessCalculator.View
{
    /// <summary>
    /// Interaction logic for CompositionPresenter.xaml
    /// </summary>
    public partial class CompositionPresenter : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CompositionPresenter()
        {
            SetValue(ComponentsProperty, new List<ComponentPresenter>()); //Components list initialization

            //Handler for Loaded event, when all Templates have been applied and all necessary elements were created and can be found for future reference.
            this.Loaded += OnLoaded;
            InitializeComponent();
        }

        /// <summary>
        /// After loading this handler will find and bind ExtendableDataGrid parts.
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (ComponentPresenter component in Components)
            { 

                this.MainPanel.Children.Add(component);
            }
        }

        public bool EditMode
        {
            get { return (bool)GetValue(EditModeProperty); }
            set { SetValue(EditModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditModeProperty =
            DependencyProperty.Register("EditMode", typeof(bool), typeof(CompositionPresenter), new PropertyMetadata(false, OnEditModeChanged));

        public static void OnEditModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //((CompositionPresenter)d).NotifyPropertyChanged("EditMode");
        }

        /// <summary>
        /// List of components, which this presenter holds.
        /// </summary>
        public List<ComponentPresenter> Components
        {
            get { return (List<ComponentPresenter>)GetValue(ComponentsProperty); }
            set { SetValue(ComponentsProperty, value); }
        }

        public static readonly DependencyProperty ComponentsProperty =
            DependencyProperty.Register("Components",
                                        typeof(List<ComponentPresenter>),
                                        typeof(CompositionPresenter),
                                        new PropertyMetadata(new List<ComponentPresenter>()));



    }
}
