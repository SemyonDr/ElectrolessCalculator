using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectrolessCalculator.View
{
    public static class OneWayToSource
    {
        public static readonly DependencyProperty BindingProperty = 
            DependencyProperty.RegisterAttached(
                "Binding",
                typeof(OneWayToSourceBinding),
                typeof(OneWayToSource),
                new PropertyMetadata(default(OneWayToSourceBinding), OnBindingsChanged)
            );

        public static void SetBinding(this FrameworkElement element, OneWayToSourceBinding value)
        {
            element.SetValue(BindingProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static OneWayToSourceBinding GetBinding(this FrameworkElement element)
        {
            return (OneWayToSourceBinding)element.GetValue(BindingProperty);
        }

        private static void OnBindingsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((OneWayToSourceBinding)e.OldValue)?.ClearValue(OneWayToSourceBinding.ElementProperty);
            ((OneWayToSourceBinding)e.NewValue)?.SetValue(OneWayToSourceBinding.ElementProperty, d);
        }
    }
}
