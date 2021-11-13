using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectrolessCalculator.View
{
    /// <summary>
    /// This class allows to attach routed property object to any framework element in XAML.
    /// Usage in XAML:
    /// 
    /// <TextBox SomeTextBoxProperty="Value">
    ///     <RoutedPropertyContainer.RoutedProperty>
    ///         <RoutedProperty 
    ///             PropertyPath="(Validation.HasError)" 
    ///             Bindning={Binding ViewModelProperty}/>
    ///     </RoutedPropertyContainer.RoutedProperty>
    /// </TextBox>
    /// 
    /// </summary>
    public static class RoutedPropertyContainer
    {
        public static readonly DependencyProperty RoutedPropertyProperty =
            DependencyProperty.RegisterAttached(
                "RoutedProperty",
                typeof(object),
                typeof(RoutedPropertyContainer),
                new PropertyMetadata(null, OnRoutedPropertyChanged)
            );

        public static void SetRoutedProperty(this FrameworkElement TargetElement, RoutedProperty value) {
            TargetElement.SetValue(RoutedPropertyProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static RoutedProperty GetRoutedProperty(this FrameworkElement TargetElement) {
            return TargetElement.GetValue(RoutedPropertyProperty) as RoutedProperty;
        }

        private static void OnRoutedPropertyChanged(DependencyObject TargetElement, DependencyPropertyChangedEventArgs e)
        {
            RoutedProperty oldRp = e.OldValue as RoutedProperty;
            RoutedProperty newRp = e.NewValue as RoutedProperty;
            if (oldRp != null)
                oldRp.ClearValue(RoutedProperty.TargetElementProperty);
            if (newRp != null)
                newRp.SetValue(RoutedProperty.TargetElementProperty, TargetElement);
        }
    }
}
