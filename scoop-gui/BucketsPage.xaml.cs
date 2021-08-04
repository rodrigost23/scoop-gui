#nullable enable
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PropertyChanged.SourceGenerator;
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

        public ObservableCollection<ScoopBucket> BucketsKnown => state.BucketsKnown;

        public AddBucketPoco AddBucket { get; }

        public ObservableObject<bool> IsLoading { get; } = false;

        public ObservableObject<bool> IsLoadingKnown { get; } = false;

        protected override CommandList MenuItems => _menuItems;

        private readonly CommandList _menuItems = new()
        {
        };

        public string? Query { get; private set; }

        public BucketsPage()
        {
            InitializeComponent();

            AddBucket = new(state);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (BucketsList.Count == 0 && !IsLoading)
            {
                RefreshData();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            IsLoading.Value = true;
            _ = Task.Run(async () =>
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
            }).ContinueWith((_task) =>
            {
                _ = DispatcherQueue.TryEnqueue(() =>
                {
                    IsLoading.Value = false;
                });
            });

            IsLoadingKnown.Value = true;
            _ = Task.Run(async () =>
              {
                  await foreach (ScoopBucket item in Scoop.BucketKnown())
                  {
                      // Run in UI Thread
                      _ = DispatcherQueue.TryEnqueue(() =>
                        {
                            int index = BucketsKnown.ToList().FindIndex(x => x.Name == item.Name);
                            if (index <= -1)
                            {
                                index = BucketsKnown.ToList().FindIndex(x => string.Compare(x.Name, item.Name, StringComparison.OrdinalIgnoreCase) > 0);

                                if (index > -1)
                                {
                                    BucketsKnown.Insert(index, item);
                                }
                                else
                                {
                                    BucketsKnown.Add(item);
                                }
                            }
                        });
                  }
              }).ContinueWith((_task) =>
              {
                  _ = DispatcherQueue.TryEnqueue(() =>
                  {
                      IsLoadingKnown.Value = false;
                  });
              });
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _ = AddDialog.ShowAsync();
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            sender.IsSuggestionListOpen = !sender.IsSuggestionListOpen;
        }

        //private void AutoSuggestBox_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    (sender as AutoSuggestBox)!.IsSuggestionListOpen = true;
        //}

        //private void AutoSuggestBox_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    (sender as AutoSuggestBox)!.IsSuggestionListOpen = false;
        //}

    }

    public partial class AddBucketPoco
    {
        private readonly State state;

        public AddBucketPoco(State state)
        {
            this.state = state;
        }

        [Notify] private string? _name;
        [Notify] private string? _url;
        public bool IsUrlEnabled => !state.BucketsKnown.Any(x => x.Name == Name);
    }
}
