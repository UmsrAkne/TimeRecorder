namespace TimeRecorder.Models.Converters;

using System;
using System.Globalization;
using System.Windows.Data;

public class DateTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value != null ? ((DateTime)value).ToString("yy/MM/dd HH:mm") : string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}