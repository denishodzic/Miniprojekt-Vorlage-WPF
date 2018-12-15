using System;
using System.Windows.Data;

namespace Gadgeothek.WinUI
{
    public class IntFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string strVal = value.ToString();
                if (string.IsNullOrEmpty(strVal))
                {
                    return 0;
                }
                else
                {
                    return int.Parse(strVal);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
