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
    /// This control represents a row representing a solution component and used is used as a part of Solution Presenter.
    /// See default template in Generic.xaml
    /// </summary>
    [TemplateVisualState(Name="DisplayState", GroupName="EditStates")]
    [TemplateVisualState(Name="EditState", GroupName = "EditStates")]
    [TemplatePart(Name = "ComponentNameTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "DisplayValueTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "EditValueTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "UnitsTextBlock", Type = typeof(TextBlock))]
    public class ComponentPresenter : Control
    {
        #region INITIALIZATION
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Default constructor.
        /// </summary>
        static ComponentPresenter()
        {
            //Binds default template for this control.
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComponentPresenter), new FrameworkPropertyMetadata(typeof(ComponentPresenter)));
        }

        //---------------------------------------------------------------------------------------------------------------
        #endregion

        #region DISPLAYED PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Name of the solution component.
        /// Dependency property.
        /// </summary>
        public string ComponentName
        {
            get { return (string)GetValue(ComponentNameProperty); }
            set { SetValue(ComponentNameProperty, value); }
        }

        public static readonly DependencyProperty ComponentNameProperty =
            DependencyProperty.Register("ComponentName", typeof(string), typeof(ComponentPresenter), new PropertyMetadata(""));

        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Units used to display the value.
        /// Dependency property.
        /// </summary>
        public ComponentUnits ValueUnits
        {
            get { return (ComponentUnits)GetValue(ValueUnitsProperty); }
            set { SetValue(ValueUnitsProperty, value); }
        }

        public static readonly DependencyProperty ValueUnitsProperty =
            DependencyProperty.Register("ValueUnits", typeof(ComponentUnits), typeof(ComponentPresenter), new PropertyMetadata(ComponentUnits.kg));

        //---------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Value of the component in composition in selected units.
        /// Dependency property.
        /// </summary>
        public float ComponentValue
        {
            get { return (float)GetValue(ComponentValueProperty); }
            set { SetValue(ComponentValueProperty, value); }
        }

        public static readonly DependencyProperty ComponentValueProperty =
            DependencyProperty.Register("ComponentValue", typeof(float), typeof(ComponentPresenter), new PropertyMetadata(0.0f));

        //---------------------------------------------------------------------------------------------------------------
        #endregion

        #region EDIT MODE
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This property control the state of the template switching it between display mode and edit mode.
        /// Dependency property.
        /// </summary>
        public bool EditMode
        {
            get { return (bool)GetValue(EditModeProperty); }
            set { SetValue(EditModeProperty, value); }
        }

        public static readonly DependencyProperty EditModeProperty =
            DependencyProperty.Register("EditMode", typeof(bool), typeof(ComponentPresenter), new PropertyMetadata(false, OnEditModeChanged));

        //---------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Static callback for EditMode change event.
        /// </summary>
        public static void OnEditModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ComponentPresenter cp = d as ComponentPresenter;
            EditModeChangedEventArgs args = new EditModeChangedEventArgs((bool)e.NewValue, (bool)e.OldValue);
            cp.OnEditModeChanged(args);
        }
        //---------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Method for EditMode change.
        /// </summary>
        private void OnEditModeChanged(EditModeChangedEventArgs e) {
            if (e.NewState != e.OldState) {
                if ((bool)e.NewState == true) {
                    BeginEditing();
                }
                if ((bool)e.NewState == false) {
                    FinishEditing();
                }
            }
        }
        //---------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Arguments for EditMode change event.
        /// </summary>
        public class EditModeChangedEventArgs {
            public bool NewState { get; }
            public bool OldState { get; }
            public EditModeChangedEventArgs(bool NewState, bool OldState) {
                this.NewState = NewState;
                this.OldState = OldState;
            }
        }
        //---------------------------------------------------------------------------------------------------------------

        #endregion

        #region COMPOSITION PRESENTER
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This property will be set by composition presenter.
        /// </summary>
        public CompositionPresenter ParentCompositionPresenter { get; set; }

        #endregion



        public bool BindEditModeToCompositionPresenter
        {
            get { return (bool)GetValue(BindEditModeToCompositionPresenterProperty); }
            set { SetValue(BindEditModeToCompositionPresenterProperty, value); }
        }

        public static readonly DependencyProperty BindEditModeToCompositionPresenterProperty =
            DependencyProperty.Register("BindEditModeToCompositionPresenter", typeof(bool), typeof(ComponentPresenter), new PropertyMetadata(true));

        private void BeginEditing()
        {
            ValueTextBlock.Visibility = Visibility.Hidden;
            EditValueTextBox.Text = ComponentValue.ToString();
            EditValueTextBox.Visibility = Visibility.Visible;
        }

        private void FinishEditing()
        {
            if (true /*Add "cancel editing" argument check here*/)
            {//Save edited value
                float parsedValue;
                if (float.TryParse(EditValueTextBox.Text, out parsedValue))
                {
                    ComponentValue = parsedValue;
                }
                EditValueTextBox.Visibility = Visibility.Hidden;
                ValueTextBlock.Visibility = Visibility.Visible;
            }
            else
            {//Cancel

            }
        }

        public float ValueForEditing
        {
            get { return (float)GetValue(ValueForEditingProperty); }
            set { SetValue(ValueForEditingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueForEditing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueForEditingProperty =
            DependencyProperty.Register("ValueForEditing", typeof(float), typeof(ComponentPresenter), new PropertyMetadata(0.0f));


        public enum ComponentUnits
        {
            kg,
            g,
            l,
            ml,
            kg_l,
            g_l,
            l_l,
            ml_l
        }
    }
}
