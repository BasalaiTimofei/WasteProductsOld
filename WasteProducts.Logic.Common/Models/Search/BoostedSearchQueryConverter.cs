using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Models.Search
{
    class BoostedSearchQueryConverter : TypeConverter
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
                BoostedSearchQuery query = new BoostedSearchQuery();
                string key = value as string;
                if (String.IsNullOrEmpty(key))
                {
                    return null;
                }
                BoostedSearchQuery result = new BoostedSearchQuery();

                var data = key.Split(new char[] { ';' });
                if (data.Length > 1)
                {
                    result.Query = data[0];
                    string boostFieldValues = data[1];
                    var boosts = boostFieldValues.Split(new char[] { ',' });
                    if (boosts.Length > 0)
                    {
                        foreach (var boost in boosts)
                        {
                            var fieldNameBoost = boost.Split(new char[] { ':' });
                            string fieldName = fieldNameBoost[0];
                            float fieldBoost = float.Parse(fieldNameBoost[1], CultureInfo.InvariantCulture);
                            result.AddField(fieldName, fieldBoost);
                        }
                    }
                    return result;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
