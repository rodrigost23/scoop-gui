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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ScoopGui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppsListPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<ScoopApp> appsList = new ObservableCollection<ScoopApp>();

        public bool isLoading = true;

        public AppsListPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            isLoading = true;
            //Bindings.Update();
            appsList.Clear();
            var list = await Task.Run(() => Scoop.List());
            appsList.AddAll(list);
            isLoading = false;
            //Bindings.Update();
        }
    }
}