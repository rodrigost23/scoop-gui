using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace scoop_gui
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

            ScoopStreamReader.Read((line) => {
                System.Diagnostics.Debug.Print(" READLINE: > " + line);
            });
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string result = await Scoop.RunAsync("list");

            System.Diagnostics.Debug.Print($"RESULT: {result}");
        }
    }
}
