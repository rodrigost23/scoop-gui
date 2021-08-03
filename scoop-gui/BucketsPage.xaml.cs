#nullable enable
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ScoopGui.Models;
using ScoopGui.Util;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ScoopGui
{
    public sealed partial class BucketsPage : BasePage
    {
        public ObservableCollection<ScoopBucket> BucketsList => state.BucketsList;

        public ObservableObject<bool> IsLoading { get; } = false;

        protected override CommandList MenuItems => _menuItems;

        private readonly CommandList _menuItems = new()
        {
        };

        public string? Query { get; private set; }

        public BucketsPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (BucketsList.Count == 0 && !IsLoading)
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

            await Task.Run(async () =>
            {
                await foreach (ScoopBucket item in Scoop.BucketList())
                {
                    // Run in UI Thread
                    _ = DispatcherQueue.TryEnqueue(() =>
                    {
                        int index = BucketsList.ToList().FindIndex(x => x.Name == item.Name);
                        if (index <= -1)
                        {
                            index = BucketsList.ToList().FindIndex(x => string.Compare(x.Name, item.Name, StringComparison.OrdinalIgnoreCase) > 0);

                            if (index > -1)
                            {
                                BucketsList.Insert(index, item);
                            }
                            else
                            {
                                BucketsList.Add(item);
                            }
                        }
                    });
                }
            });

            IsLoading.Value = false;
        }
    }
}
