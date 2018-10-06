using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace TuneIn.Converters
{
    public class DictionaryToJsonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var props = (Dictionary<string, string>)value;
            if(props != null)
            {
                var vals = string.Join(",", props.Select(pair => $"\"{pair.Key}\"=\"{pair.Value}\""));
                return $"{{{vals}}}";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
