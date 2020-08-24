using Microsoft.UI.Xaml;
using ScoopGui.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        public MainWindow()
        {
            InitializeComponent();

            ContentFrame.Navigate(typeof(AppsListPage));

            ScoopStreamReader.Read((line) => {
                // TODO: Print to a text box instead
                System.Diagnostics.Debug.Print(line);
            });
        }
    }
}
