using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ElectrolessCalculator.View
{
    public class OneWayToSourceBinding : FrameworkElement
    {
        
        private static readonly PropertyPath DataContextPath = new PropertyPath(nameof(DataContext));
        private static readonly PropertyPath HasErrorPath = new PropertyPath($"({typeof(Validation).Name}.{Validation.HasErrorProperty.Name})");


        //Property ViewModel will bind to this property
        public bool HasError
        {
            get { return (bool)this.GetValue(HasErrorProperty); }
            set { this.SetValue(HasErrorProperty, value); }
        }

        public static readonly DependencyProperty HasErrorProperty = DependencyProperty.Register(
            nameof(HasError),
            typeof(bool),
            typeof(OneWayToSourceBinding),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        internal static readonly DependencyProperty ElementProperty = DependencyProperty.Register(
            "Element",                          
            typeof(UIElement),
            typeof(OneWayToSourceBinding),
            new PropertyMetadata(default(UIElement), OnElementChanged));


        private static readonly DependencyProperty HasErrorProxyProperty = DependencyProperty.RegisterAttached(
            "HasErrorProxy",
            typeof(bool),
            typeof(OneWayToSourceBinding),
            new PropertyMetadata(false, OnHasErrorProxyChanged));


        private static void OnHasErrorProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetCurrentValue(HasErrorProperty, e.NewValue);
        }


        private static void OnElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        { 
            if (e.NewValue == null)
            {
                BindingOperations.ClearBinding(d, DataContextProperty);
                BindingOperations.ClearBinding(d, HasErrorProxyProperty);
            }
            else
            {
                //     Source           Binding            Target
                //[Element.DataContext]--------->[OneWayToSourceBinding.DataContext]
                Binding dataContextBinding = new Binding
                {
                    Path = DataContextPath,
                    Mode = BindingMode.OneWay,
                    Source = e.NewValue
                };
                BindingOperations.SetBinding(d, DataContextProperty, dataContextBinding);

                //     Source                   Binding            Target
                //[Element.Validation.HasError]--------->[OneWayToSourceBinding.HasErrorProxy]
                Binding hasErrorBinding = new Binding
                {
                    Path = HasErrorPath,
                    Mode = BindingMode.OneWay,
                    Source = e.NewValue
                };
                BindingOperations.SetBinding(d, HasErrorProxyProperty, hasErrorBinding);
            }
        }
    }
}
