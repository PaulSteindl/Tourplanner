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
using System.Windows.Shapes;
using Tourplanner.ViewModels;

namespace Tourplanner.Views
{
    /// <summary>
    /// Interaction logic for TourManagerView.xaml
    /// </summary>
    public partial class TourManagerView : Window
    {
        public TourManagerView()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ICloseWindow cw)
            {
                cw.Close += () => Close();
            }
        }
    }
}
