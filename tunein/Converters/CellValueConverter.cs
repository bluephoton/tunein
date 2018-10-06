using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace TuneIn.Converters
{
    public class CellValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataGridCell cell;
            var cellInfo = (DataGridCellInfo)value;
            if (cellInfo.Column != null)
            {
                var cellContent = cellInfo.Column.GetCellContent(cellInfo.Item);
                if (cellContent != null)
                {
                    cell = (DataGridCell)cellContent.Parent;
                    var textBlock = (TextBlock)cell?.Content;
                    return textBlock?.Text;
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
