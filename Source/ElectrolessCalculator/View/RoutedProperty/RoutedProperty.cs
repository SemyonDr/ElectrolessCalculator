using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ElectrolessCalculator.View
{
    /// Terminology:
    /// TargetElement - framework element that has target property
    /// TargetProperty - Property that is final target of routing
    /// TargetProxy - binds with TargetProperty
    /// SourceProxy - binds with source (presumably source model property)
    /// Source - binds with SourceProxy
    /// 
    /// Bindings are established in RoutedProperty object
    /// 
    /// Scheme:
    ///                             [TargetElement ]  [                     RoutedProperty                        ]   [   View Model  ]
    /// In routed property terms:   [TargetProperty]--[--binds-->TargetProxy--changes with-->SourceProxy<--binds--]---[Source property]
    ///                                                                         callback
    /// In regular binding terms:    [Source]----OneWay---------->[Target]                    [Target]<--OneWayToSource--[Source]
    public class RoutedProperty : FrameworkElement
    {
        #region Initialization
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        public RoutedProperty()
        {
            this.Initialized += OnInitialized;
        }

        private void OnInitialized(object sender, EventArgs e)
        {
            SetDataContextBinding();
        }
        #endregion

        
        #region USER DEFINED PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Property used to specify which property of TargetElement will be routed.
        /// </summary>
        public string TargetPropertyPath
        {
            get { return (string)GetValue(TargetPropertyPathProperty); }
            set { SetValue(TargetPropertyPathProperty, value); }
        }

        public static readonly DependencyProperty TargetPropertyPathProperty =
            DependencyProperty.Register(
                "TargetPropertyPath",
                typeof(string),
                typeof(RoutedProperty),
                new PropertyMetadata("", OnPropertyPathChanged)
            );


        /// <summary>
        /// Property used to specify binding to view model.
        /// </summary>
        public Binding SourceBinding
        {
            get { return (Binding)GetValue(SourceBindingProperty); }
            set { SetValue(SourceBindingProperty, value); }
        }

        public static readonly DependencyProperty SourceBindingProperty =
            DependencyProperty.Register(
                "SourceBinding",
                typeof(Binding),
                typeof(RoutedProperty),
                new PropertyMetadata(null, OnBindingChanged)
            );
        #endregion


        #region INTERNAL PROPERTIES
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        private FrameworkElement TargetElement {
            get {
                return GetValue(TargetElementProperty) as FrameworkElement;
            }
        }

        /// <summary>
        /// Framework element containing the property to be routed.
        /// </summary>
        public static readonly DependencyProperty TargetElementProperty =
            DependencyProperty.Register("TargetElement",
                typeof(FrameworkElement),
                typeof(RoutedProperty),
                new PropertyMetadata(null, OnTargetElementChanged)
            );


        /// <summary>
        /// Target Proxy property binds with target property in OneWay mode and changes when target changes.
        /// </summary>
        private static readonly DependencyProperty TargetProxyProperty =
            DependencyProperty.RegisterAttached(
                "TargetProxy",
                typeof(object),     //Proxy type is object, so any type is supported
                typeof(RoutedProperty),
                new PropertyMetadata(null, OnTargetProxyChanged)
            );

        /// <summary>
        /// Source Proxy property is changed in OnTargetProxyChanged callback and binds in OneWayToSource mode to source property.
        /// </summary>
        private static readonly DependencyProperty SourceProxyProperty =
            DependencyProperty.Register(
                "SourceProxy",
                typeof(object),
                typeof(RoutedProperty),
                new PropertyMetadata(null)
            );

        /// <summary>
        /// Property bound to TargetElement.DataContext, includes callback for reporting that TargetElement.DataContext is changed.
        /// </summary>
        private static readonly DependencyProperty DataContextWatcherProperty =
            DependencyProperty.Register(
                "DataContextWatcher",
                typeof(object),
                typeof(RoutedProperty),
                new PropertyMetadata(null, OnDataContextChanged)
                );
        #endregion


        #region PROPERTY CHANGED CALLBACKS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Handler for change of target element.
        /// On changing target element resets target binding.
        /// </summary>
        /// <param name="RoutedPropertyObject"></param>
        /// <param name="e"></param>
        private static void OnTargetElementChanged(DependencyObject RoutedPropertyObject, DependencyPropertyChangedEventArgs e)
        {
            RoutedProperty rpo = RoutedPropertyObject as RoutedProperty;
            rpo.SetDataContextBinding();
            SetTargetBinding(rpo);
        }

        /// <summary>
        /// Handler for change of property path. 
        /// If property path is changed sets new target binding.
        /// </summary>
        /// <param name="RoutedPropertyObject"></param>
        /// <param name="e"></param>
        private static void OnPropertyPathChanged(DependencyObject RoutedPropertyObject, DependencyPropertyChangedEventArgs e)
        {
            RoutedProperty rpo = RoutedPropertyObject as RoutedProperty;
            SetTargetBinding(rpo);
        }

        /// <summary>
        /// Handler for change of source binding.
        /// If user entered binding is changed resets source binding.
        /// </summary>
        /// <param name="RoutedPropertyObject">RoutedPropertyObject</param>
        /// <param name="e"></param>
        private static void OnBindingChanged(DependencyObject RoutedPropertyObject, DependencyPropertyChangedEventArgs e)
        {
            RoutedProperty rpo = RoutedPropertyObject as RoutedProperty;
            rpo.SetSourceBinding();
        }

        /// <summary>
        /// When TargetProxy value changed this callback changes SourceProxy value.
        /// </summary>
        /// <param name="RoutedPropertyObject"></param>
        /// <param name="e"></param>
        private static void OnTargetProxyChanged(DependencyObject RoutedPropertyObject, DependencyPropertyChangedEventArgs e)
        {
            RoutedProperty rpo = RoutedPropertyObject as RoutedProperty;
            rpo.SetValue(RoutedProperty.SourceProxyProperty, e.NewValue);
        }


        /// <summary>
        /// When data context changes source binding resets.
        /// </summary>
        /// <param name="RoutedPropertyObject"></param>
        /// <param name="e"></param>
        private static void OnDataContextChanged(DependencyObject RoutedPropertyObject, DependencyPropertyChangedEventArgs e) {
            RoutedProperty rpo = RoutedPropertyObject as RoutedProperty;
            rpo.SetSourceBinding();
        }
        #endregion


        #region METHODS
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Binds [TargetElement.PropertyPath] as source for [TargetProxy]
        /// </summary>
        private static void SetTargetBinding(RoutedProperty RoutedPropertyObject) {
            if (RoutedPropertyObject.TargetElement != null)
            {
                Binding targetBinding = new Binding(RoutedPropertyObject.TargetPropertyPath);
                targetBinding.Source = RoutedPropertyObject.TargetElement;
                targetBinding.Mode = BindingMode.OneWay;
                BindingOperations.SetBinding(RoutedPropertyObject, TargetProxyProperty, targetBinding);
            }
        }


        /// <summary>
        /// Binds [ViewModel.SourceProperty] as source for [SourceProxy].
        /// Uses Binding property entered by user.
        /// </summary>
        private void SetSourceBinding()
        {
            //We need to get binding object created by user
            Binding userBinding = BindingOperations.GetBinding(this, SourceBindingProperty);
            //Removing user defined binding, because it is ficticious and shouldn't be set
            BindingOperations.ClearBinding(this, SourceBindingProperty);
            object dataContext = GetValue(DataContextWatcherProperty);
            if (userBinding != null && dataContext != null)
            {
                //Creating binding using userBinding
                Binding sourceBinding = new Binding();
                sourceBinding.Source = GetValue(DataContextWatcherProperty);
                sourceBinding.Path = userBinding.Path;
                sourceBinding.Mode = BindingMode.OneWayToSource;

                SetBinding(SourceProxyProperty, sourceBinding);
            }
        }


        /// <summary>
        /// Binding data context of target element to data context watcher property.
        /// </summary>
        private void SetDataContextBinding() {
            //Because of the way this object is declared in XAML it doesn't inherit data context automatically
            //and bindings are not updated when data context is changed.
            Binding dcBinding = new Binding("DataContext");
            dcBinding.Source = TargetElement;
            dcBinding.Mode = BindingMode.OneWay;
            SetBinding(DataContextWatcherProperty, dcBinding);
        }
        #endregion
    }
}