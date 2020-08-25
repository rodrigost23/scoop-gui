using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using ScoopGui.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ScoopGui
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public string AppTitleText => "Scoop";

        // List of ValueTuple holding the Navigation Tag and the relative Navigation Page
        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("installed", typeof(AppsListPage)),
            ("browse", typeof(BrowsePage)),
            //("buckets", typeof(BucketsPage)),
        };

        public MainWindow()
        {
            InitializeComponent();

            ScoopStreamReader.Read((line) =>
            {
                // TODO: Print to a text box instead
                CommandPeek.Text = line;
            });
        }

        //~MainWindow()
        //{
        //    ScoopStreamReader.Stop();
        //}

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // NavView doesn't load any page by default, so load home page.
            NavView.SelectedItem = NavView.MenuItems[0];

            // Add keyboard accelerators for backwards navigation.
            //var goBack = new KeyboardAccelerator { Key = Windows.System.VirtualKey.GoBack };
            //goBack.Invoked += BackInvoked;
            //this.KeyboardAccelerators.Add(goBack);

            // ALT routes here
            //var altLeft = new KeyboardAccelerator
            //{
            //    Key = Windows.System.VirtualKey.Left,
            //    Modifiers = Windows.System.VirtualKeyModifiers.Menu
            //};
            //altLeft.Invoked += BackInvoked;
            //this.KeyboardAccelerators.Add(altLeft);
        }

        // NavView_SelectionChanged is not used in this example, but is shown for completeness.
        // You will typically handle either ItemInvoked or SelectionChanged to perform navigation,
        // but not both.
        private void NavView_SelectionChanged(NavigationView sender,
                                              NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected == true)
            {
                NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
            }
            else if (args.SelectedItemContainer != null && args.SelectedItemContainer.Tag != null)
            {
                var navItemTag = args.SelectedItemContainer.Tag.ToString();
                NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void NavView_Navigate(
            string navItemTag,
            Microsoft.UI.Xaml.Media.Animation.NavigationTransitionInfo transitionInfo)
        {
            Type _page = null;
            if (navItemTag == "settings")
            {
                //_page = typeof(SettingsPage);
            }
            else
            {
                var item = _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag));
                _page = item.Page;
            }
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            var preNavPageType = ContentFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (!(_page is null) && !Type.Equals(preNavPageType, _page))
            {
                ContentFrame.Navigate(_page, null, transitionInfo);
            }
        }
    }
}
