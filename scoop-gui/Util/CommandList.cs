using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace ScoopGui.Util
{
    public class CommandList : KeyedCollection<string, ICommandBarElement>
    {
        protected override string GetKeyForItem(ICommandBarElement item)
        {
            return item is FrameworkElement f ? f.Tag as string : item.ToString();
        }
    }
}