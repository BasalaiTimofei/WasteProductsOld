using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WasteProducts.Logic.Common.Models.Search
{
    class SearchQueryConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string key = value as string;
                if (String.IsNullOrEmpty(key))
                {
                    throw new ArgumentException("Incorrect query.");
                }
                SearchQuery result = new SearchQuery();

                var data = key.Split(new char[] { ';' });
                if (data.Length > 1)
                {
                    result.Query = data[0];
                    string searchableFields = data[1];
                    var fieldNames = searchableFields.Split(new char[] { ',' });
                    if (fieldNames.Length > 0)
                    {
                        foreach (var field in fieldNames)
                        {
                            result.AddField(field);
                        }
                    }
                    return result;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
