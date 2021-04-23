using Microsoft.UI.Xaml.Controls;

namespace ScoopGui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConsolePage : Page
    {
        public ConsolePage()
        {
            InitializeComponent();

            ScoopStreamReader.Read((line) =>
            {
                _ = DispatcherQueue.TryEnqueue(() =>
                {
                    ConsoleBox.Text += $"{line}\n";
                });
            });
        }
    }
}
