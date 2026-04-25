using System.Globalization;

namespace SimplyTodosApp.Converters
{
    public class PriorityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var priority = value?.ToString();
            return priority switch
            {
                "High" => Colors.Red,
                "Medium" => Colors.Orange,
                "Low" => Colors.Green,
                "None" => Colors.Black,
                _ => Colors.Black
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}