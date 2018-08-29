using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Globalization;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using WasteProducts.Logic.Common.Models.Search;

namespace WasteProducts.Web.Utils.Search
{
    public class BoostedSearchQueryModelBinder : IModelBinder
    {
        static BoostedSearchQueryModelBinder()
        {

        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(BoostedSearchQuery))
            {
                return false;
            }

            ValueProviderResult val = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (val == null)
            {
                return false;
            }

            string key = val.RawValue as string;
            if (key == null)
            {
                bindingContext.ModelState.AddModelError(
                    bindingContext.ModelName, "Wrong value type");
                return false;
            }
            BoostedSearchQuery result = new BoostedSearchQuery();

            var data = key.Split(new char[] {';'});
            if (data.Length > 1)
            {
                result.Query = data[0];
                string boostFieldValues = data[1];
                var boosts = boostFieldValues.Split(new char[] {','});
                if (boosts.Length > 0)
                {
                    foreach (var boost in boosts)
                    {
                        var fieldNameBoost = boost.Split(new char[] {':'});
                        string fieldName = fieldNameBoost[0];
                        float fieldBoost = float.Parse(fieldNameBoost[1], CultureInfo.InvariantCulture);
                        result.AddField(fieldName, fieldBoost);
                    }
                }
                bindingContext.Model = result;
                return true;
            }

            bindingContext.ModelState.AddModelError( bindingContext.ModelName, "Cannot convert value to BoostedSearchQuery");
            return false;
        }
    }
}