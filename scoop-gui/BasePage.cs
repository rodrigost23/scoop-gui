using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ScoopGui.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoopGui
{
    public abstract class BasePage : Page
    {
        private App _app = Application.Current as App;

        protected abstract CommandList MenuItems { get; }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _app.MainWindow.CommandBar.PrimaryCommands.RemoveAll(MenuItems);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _app.MainWindow.CommandBar.PrimaryCommands.InsertAll(0, MenuItems);
        }
    }
}
