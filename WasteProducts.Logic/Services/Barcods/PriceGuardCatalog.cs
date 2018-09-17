using System.Text.RegularExpressions;
using WasteProducts.Logic.Common.Services.Barcods;
using WasteProducts.Logic.Common.Models.Barcods;

namespace BarcodeSearch.Logic.Services.WebCatalogs
{
    public class PriceGuardCatalog : ICatalog
    {
        const string SEARCH_URI_FORMATTER = "https://priceguard.ru/search?q={0}";
        const string DESCRIPTION_URI_FORMATTER = "https://priceguard.ru{0}";
        const string NAME_PATTERN = "<h1.*?>(.*?)</h1>";
        const string BREND_PATTERN = @"<tr>\S*<td.*?>\S*<span.*?>Производитель</span>\S*</td>\S*<td.*?>\S*<span>\S*<span>\S*<a.*?><span.*?>(.*?)</span></a>\S*</span>\S*</span>\S*</td>\S*</tr>";
        const string COUNTRY_PATTERN = @"<tr>\S*<td.*?>\S*<span.*?>Страна-изготовитель</span>\S*</td>\S*<td.*?>\S*<span>\S*<span>\S*.*?<span.*?>(.*?)</span></a>\S*</span>\S*</span>\S*</td>\S*</tr>";
        const string PICTURE_PATH_PATTERN = @"<img class=\""op-image\"" src=\""(.*?)\""";
        const string DESCRIPTION_URI_PATTERN = @"<a title=\""Перейти на страницу с описанием\"" href=\""\.(.*?)\"">";

        IHttpHelper _httpHelper;

        public PriceGuardCatalog(IHttpHelper httpHelper)
        {
            _httpHelper = httpHelper;
        }

        public CatalogProductInfo Get(string barcode)
        {
            var queryResult = GetPage(barcode);

            //если страница с товаром найдена
            if (queryResult.StatusCode >= 200 && queryResult.StatusCode < 300)
            {
                var nameParseResult = ParseName(queryResult.Page);

                //минимум что мы хотим знать о товаре, это его имя. если оно найдено - дополняем остальными данными ProductInfo и возвращаем значение
                if (nameParseResult.Success)
                {
                    var result = new CatalogProductInfo();
                    
                    result.Name = nameParseResult.Value;
                    result.Composition = ParseComposition(queryResult.Page).Value;
                    result.Brend = ParseBrend(queryResult.Page).Value;
                    result.Country = ParseCountry(queryResult.Page).Value;

                    var picturePathParseResult = ParsePicturePath(queryResult.Page);
                    if (picturePathParseResult.Success)
                    {
                        result.Picture = _httpHelper.DownloadPicture(picturePathParseResult.Value);
                    }

                    return result;
                }
            }

            return null;
        }

        HttpQueryResult GetPage(string barcode)
        {
            var result = new HttpQueryResult()
            {
                StatusCode = 404
            };

            var searchPageURI = string.Format(SEARCH_URI_FORMATTER, barcode);
            var searchPageResult = _httpHelper.SendGET(searchPageURI);

            if(searchPageResult.StatusCode >= 200 && searchPageResult.StatusCode < 300)
            {
                var descriptionPageURIParseResult = ParseDescriptionPageURI(searchPageResult.Page);

                if(descriptionPageURIParseResult.Success)
                {
                    var descriptionPageURI = string.Format(DESCRIPTION_URI_FORMATTER, descriptionPageURIParseResult.Value);
                    var descriptionPageResult = _httpHelper.SendGET(descriptionPageURI);

                    if (descriptionPageResult.StatusCode >= 200 && descriptionPageResult.StatusCode < 300)
                    {
                        return descriptionPageResult;
                    }
                }
            }

            return result;
        }

        ParseResult ParseName(string page)
        {
            var result = new ParseResult();

            Regex r = new Regex(NAME_PATTERN);
            Match m = r.Match(page);

            if (m.Success)
            {
                result.Success = true;
                result.Value = m.Groups[1].Value;
            }

            return result;
        }

        ParseResult ParseComposition(string page)
        {
            return new ParseResult(); ;
        }

        ParseResult ParseBrend(string page)
        {
            var result = new ParseResult();

            Regex r = new Regex(BREND_PATTERN);
            Match m = r.Match(page);

            if (m.Success)
            {
                result.Success = true;
                result.Value = m.Groups[1].Value;
            }

            return result;
        }

        ParseResult ParseCountry(string page)
        {
            var result = new ParseResult();

            Regex r = new Regex(COUNTRY_PATTERN);
            Match m = r.Match(page);

            if (m.Success)
            {
                result.Success = true;
                result.Value = m.Groups[1].Value;
            }

            return result;
        }

        ParseResult ParsePicturePath(string page)
        {
            var result = new ParseResult();

            Regex r = new Regex(PICTURE_PATH_PATTERN);
            Match m = r.Match(page);

            if (m.Success)
            {
                result.Success = true;
                result.Value = m.Groups[1].Value;
            }

            return result;
        }
        
        ParseResult ParseDescriptionPageURI(string pageHTML)
        {
            var result = new ParseResult();

            Regex r = new Regex(DESCRIPTION_URI_PATTERN);
            Match m = r.Match(pageHTML);

            if (m.Success)
            {
                result.Success = true;
                result.Value = m.Groups[1].Value;
            }

            return result;
        }
    }
}
