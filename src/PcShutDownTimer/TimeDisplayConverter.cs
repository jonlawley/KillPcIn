using System;
using System.Globalization;
using System.Windows.Data;

namespace PcShutdownTimer
{
    public class TimeDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeSpan = value as TimeSpan? ?? new TimeSpan();
            if (timeSpan != TimeSpan.Zero)
            {
                return timeSpan.TotalHours > 1 ? $"{Math.Floor(timeSpan.TotalHours)}{timeSpan.ToString(@"\:mm\:ss")}":$"{timeSpan.ToString(@"mm\:ss\:ff")}";
            }
            return "00:00:00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}