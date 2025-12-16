using System;
using System.Globalization;

namespace Inveni.App.ViewModels
{
    /// <summary>
    /// Converter per invertire un valore booleano
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return value;
        }
    }

    /// <summary>
    /// Converter per mostrare/ocultare in base a se un valore è null
    /// </summary>
    public class NullToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Se il valore non è null, restituisce true (mostra l'elemento)
            return value != null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter per formattare la lunghezza della caccia
    /// </summary>
    public class LunghezzaFormatterConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string lunghezzaString)
            {
                // Se è un numero, formatta come "X km"
                if (int.TryParse(lunghezzaString, out int metri))
                {
                    if (metri >= 1000)
                    {
                        double km = metri / 1000.0;
                        return $"{km:F1} km";
                    }
                    else
                    {
                        return $"{metri} m";
                    }
                }

                // Altrimenti restituisci il valore originale
                return lunghezzaString;
            }

            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter che restituisce True se un valore numerico è maggiore di zero
    /// Utilizzato per mostrare/nascondere elementi UI in base a conteggi
    /// Esempio: Mostra riga "3 attive" solo se CacceAttive > 0
    /// </summary>
    public class MaggioreDiZeroConverter : IValueConverter
    {
        /// <summary>
        /// Converte un valore numerico in booleano (true se > 0)
        /// </summary>
        /// <param name="value">Valore numerico da valutare (es: 3, 0, -1)</param>
        /// <returns>True se il valore è maggiore di zero, altrimenti False</returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return intValue > 0;
            }

            // Se non è un int, prova con altri tipi numerici
            if (value is long longValue)
            {
                return longValue > 0;
            }

            if (value is double doubleValue)
            {
                return doubleValue > 0;
            }

            return false;
        }

        /// <summary>
        /// Conversione inversa (non implementata poiché usato solo per binding one-way)
        /// </summary>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}