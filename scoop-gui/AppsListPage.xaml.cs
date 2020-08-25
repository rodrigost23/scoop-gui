﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using ScoopGui.Models;
using System.Threading.Tasks;
using System.ComponentModel;
using ScoopGui.Util;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ScoopGui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppsListPage : Page
    {
        public ObservableCollection<ScoopApp> appsList = new ObservableCollection<ScoopApp>();

        public ObservableObject<bool> IsLoading { get; } = false;

        public AppsListPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsLoading.Value = true;
            appsList.Clear();
            var list = await Task.Run(() => Scoop.List());
            appsList.AddAll(list);
            IsLoading.Value = false;
        }
    }
}
