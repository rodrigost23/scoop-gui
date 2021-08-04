using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace ScoopGui.Util
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
            {
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
