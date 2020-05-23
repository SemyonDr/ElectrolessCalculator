﻿using System;
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

    public partial class TargetEditPanel : UserControl
    {


        public ICommand StartEditCommand {
            get { return (ICommand)GetValue(StartEditCommandProperty); }
            set { SetValue(StartEditCommandProperty, value); }}

        public static readonly DependencyProperty StartEditCommandProperty =
            DependencyProperty.Register("StartEditCommand", typeof(ICommand), typeof(TargetEditPanel), new PropertyMetadata(null));


        public TargetEditPanel()
        {
            InitializeComponent();
        }
    }
}
