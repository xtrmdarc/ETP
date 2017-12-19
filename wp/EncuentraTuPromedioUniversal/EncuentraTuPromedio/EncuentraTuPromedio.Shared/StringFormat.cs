using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;


namespace EncuentraTuPromedio 
{   
    public class StringFormat :  IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.Format((string)parameter, value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
