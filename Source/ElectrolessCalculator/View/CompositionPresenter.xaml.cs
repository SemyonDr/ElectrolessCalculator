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
    /// This control represents a solution composition by listing solution components as ComponentPresenter UI Elements.
    /// </summary>
    public partial class CompositionPresenter : ItemsControl
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        public CompositionPresenter() {
            SetValue(ComponentsProperty, new List<ComponentPresenter>()); //Components list initialization
            this.Loaded += OnLoaded;
            InitializeComponent();
        }
        //---------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// This handler is used for adding ComponentPresenter controls into visual tree and setting their properties.
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e) {
            foreach (ComponentPresenter component in Components) {
                this.MainPanel.Children.Add(component);             //Adding ComponentPresenter control to StackPanel of this Composition Presenter
                component.ParentCompositionPresenter = this;        //Setting this Presenter as ComponentPresenter parent
            }
        }
        //---------------------------------------------------------------------------------------------------------------

        #endregion

        #region COMPONENTS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// List of components, which this presenter holds.
        /// </summary>
        public List<ComponentPresenter> Components {
            get { return (List<ComponentPresenter>)GetValue(ComponentsProperty); }
            set { SetValue(ComponentsProperty, value); }
        }

        public static readonly DependencyProperty ComponentsProperty =
            DependencyProperty.Register("Components", typeof(List<ComponentPresenter>), typeof(CompositionPresenter), new PropertyMetadata(new List<ComponentPresenter>()));
        //---------------------------------------------------------------------------------------------------------------

        #endregion

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
    }
}
