using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace ScoopGui.Util
{
    public class CommandList : KeyedCollection<string, ICommandBarElement>
    {
        protected override string GetKeyForItem(ICommandBarElement item)
        {
            if (item is FrameworkElement f)
            {
                return f.Tag as string;
            } else
            {
                return item.ToString();
            }
        }
    }
}