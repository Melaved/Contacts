using System.Globalization;
using System.Windows.Data;

namespace View.Converters
{
    /// <summary>
    /// Конвертер, который инвертирует значение типа <see cref="bool"/>.
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Инвертирует значение типа <see cref="bool"/>.
        /// </summary>
        /// <param name="value">Значение типа <see cref="bool"/>, которое необходимо инвертировать.</param>
        /// <param name="targetType">Тип целевого свойства (не используется).</param>
        /// <param name="parameter">Дополнительный параметр (не используется).</param>
        /// <param name="culture">Культура (не используется).</param>
        /// <returns>
        /// Возвращает инвертированное значение типа <see cref="bool"/>, если <paramref name="value"/> является <see cref="bool"/>.
        /// В противном случае возвращает исходное значение <paramref name="value"/>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return value;
        }

        /// <summary>
        /// Инвертирует значение типа <see cref="bool"/> обратно.
        /// </summary>
        /// <param name="value">Значение типа <see cref="bool"/>, которое необходимо инвертировать.</param>
        /// <param name="targetType">Тип целевого свойства (не используется).</param>
        /// <param name="parameter">Дополнительный параметр (не используется).</param>
        /// <param name="culture">Культура (не используется).</param>
        /// <returns>
        /// Возвращает инвертированное значение типа <see cref="bool"/>, если <paramref name="value"/> является <see cref="bool"/>.
        /// В противном случае возвращает исходное значение <paramref name="value"/>.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return value;
        }
    }
}