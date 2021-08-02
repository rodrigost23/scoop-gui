using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ScoopGui.Models;
using ScoopGui.Util;
using System;
using System.Linq;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ScoopGui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppsListPage : BasePage
    {
        public State.AppsListClass AppsList => state.AppsList;

        public ObservableObject<bool> IsLoading { get; } = false;
        protected override CommandList MenuItems => _menuItems;

        private readonly CommandList _menuItems = new()
        {
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
            if (!AppsList.Installed.Any() && !IsLoading)
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

            // TODO: Instead of clearing the list, try to remove only those that are not installed anymore
            foreach (ScoopApp app in AppsList.Installed)
            {
                // Run in UI Thread
                _ = DispatcherQueue.TryEnqueue(() =>
                {
                    app.IsInstalled = false;
                });
            }

            await Task.Run(async () =>
            {
                await foreach (ScoopApp item in Scoop.List())
                {
                    // Run in UI Thread
                    _ = DispatcherQueue.TryEnqueue(() =>
                    {
                        int index = AppsList.All.ToList().FindIndex(x => x.Name == item.Name);
                        if (index > -1)
                        {
                            item.VersionUpstream = AppsList.All[index].VersionUpstream;
                            AppsList.All[index] = item;
                        }
                        else
                        {
                            index = AppsList.All.ToList().FindIndex(x => string.Compare(x.Name, item.Name, StringComparison.CurrentCultureIgnoreCase) > 0);

                            if (index > -1)
                            {
                                AppsList.All.Insert(index, item);
                            }
                            else
                            {
                                AppsList.All.Add(item);
                            }
                        }
                    });
                }
            });

            await Task.Run(async () =>
            {
                await foreach (ScoopApp item in Scoop.Status())
                {
                    // Run in UI Thread
                    _ = DispatcherQueue.TryEnqueue(() =>
                    {
                        int index = AppsList.All.ToList().FindIndex(x => x.Name == item.Name);
                        if (index > -1)
                        {
                            AppsList.All[index].Version = item.Version;
                            AppsList.All[index].VersionUpstream = item.VersionUpstream;
                        }
                    });
                }
            });

            IsLoading.Value = false;
        }
    }
}
