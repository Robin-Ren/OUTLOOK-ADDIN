﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
using MaterialDesignThemes.Wpf;
using OutlookAddin.Domain;

namespace OutlookAddIn
{
    /// <summary>
    /// BookingsControl.xaml
    /// </summary>
    public partial class BookingsControl : UserControl
    {
        public BookingsControl()
        {
            InitializeComponent();
        }

        private void CanOpenDialog(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
