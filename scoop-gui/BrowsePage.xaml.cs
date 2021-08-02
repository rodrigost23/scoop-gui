#nullable enable
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ScoopGui.Models;
using ScoopGui.Util;
using System;
using System.Linq;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ScoopGui
{
    public sealed partial class BrowsePage : BasePage
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

        public string? Query { get; private set; }

        public BrowsePage()
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
            if (AppsList.All.Count == 0 && !IsLoading)
            {
                await RefreshData();
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            string query = (string)e.Parameter;
            if (Query != query)
            {
                Query = query;
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

            await Task.Run(async () =>
            {
                await foreach (ScoopApp item in Scoop.Search(Query))
                {
                    // Run in UI Thread
                    _ = DispatcherQueue.TryEnqueue(() =>
                    {
                        int index = AppsList.All.ToList().FindIndex(x => x.Name == item.Name);
                        if (index > -1)
                        {
                            AppsList.All[index].VersionUpstream = item.VersionUpstream;
                        }
                        else
                        {
                            index = AppsList.All.ToList().FindIndex(x => string.Compare(x.Name, item.Name, StringComparison.OrdinalIgnoreCase) > 0);

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

            IsLoading.Value = false;
        }
    }
}
