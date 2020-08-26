using System;
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
using Windows.UI.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ScoopGui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppsListPage : BasePage
    {
        public ObservableCollection<ScoopApp> appsList = new ObservableCollection<ScoopApp>();

        public ObservableObject<bool> IsLoading { get; } = false;
        protected override CommandList MenuItems => _menuItems;

        private CommandList _menuItems = new CommandList{
            new AppBarButton
            {
                Tag = "Refresh",
                Icon = new SymbolIcon(Symbol.Refresh),
                Label = "Refresh",
                DynamicOverflowOrder = 1
            }
        };

        public AppsListPage()
        {
            InitializeComponent();

            if (MenuItems["Refresh"] is AppBarButton button)
            {
                button.Click += Refresh_Click;
                IsLoading.PropertyChanged += (sender, args) => button.IsEnabled = !IsLoading;
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (appsList.Count == 0 && !IsLoading)
            {
                await RefreshData();
            }
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            await RefreshData();
        }

        private async Task RefreshData()
        {
            IsLoading.Value = true;
            appsList.Clear();

            await Task.Run(async () =>
            {
                await foreach (var item in Scoop.List())
                {
                    // Run in UI Thread
                    DispatcherQueue.TryEnqueue(() => appsList.Add(item));
                }
            });

            IsLoading.Value = false;
        }
    }
}
