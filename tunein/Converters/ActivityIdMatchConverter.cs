using System;
using System.Globalization;
using System.Windows.Data;
using TuneIn.Models;

namespace TuneIn.Converters
{
    public class ActivityIdMatchConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var aid = (values[0] as TraceData)?.ActivityId.ToString();
            var name = (values[0] as TraceData)?.Name.ToString();
            var selectedAid = (string)values[1];
            var selectedName = (string)values[2];
            var aidMatch = (aid == null || string.IsNullOrEmpty(selectedAid)) ? true : aid.Equals(selectedAid, StringComparison.OrdinalIgnoreCase);
            var nameMatch = (name == null || string.IsNullOrEmpty(selectedName)) ? true : name.Equals(selectedName, StringComparison.OrdinalIgnoreCase);

            return aidMatch && nameMatch;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
