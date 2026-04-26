using System.Globalization;

namespace SimplyTodosApp.Converters
{
    public class TodosViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool showOnlyCompleted)
            {
                return !showOnlyCompleted; //Show the element when NOT in Completions view 
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
