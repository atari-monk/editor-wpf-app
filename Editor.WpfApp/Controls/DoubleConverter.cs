using System.Windows.Data;

namespace AppEngine.WpfLib.Controls.ValueSlider;

class DoubleConverter
    : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter,
    System.Globalization.CultureInfo culture)
    {
        double v = (double)value;
        return (int)v;
    }
    public object ConvertBack(object value, Type targetType, object parameter,
    System.Globalization.CultureInfo culture)
    {
        return value;
    }
}