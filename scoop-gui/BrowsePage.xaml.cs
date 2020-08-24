using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ScoopGui.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ScoopGui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BrowsePage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<ScoopApp> appsList = new ObservableCollection<ScoopApp>();

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsLoading { get; set; } = false;

        public bool IsNotLoading => !IsLoading;

        public BrowsePage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshData();
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            await RefreshData();
        }

        private async Task RefreshData()
        {
            IsLoading = true;
            Bindings.Update();
            appsList.Clear();
            var list = await Task.Run(() => Scoop.Search());
            appsList.AddAll(list);
            IsLoading = false;
            Bindings.Update();
        }
    }
}
