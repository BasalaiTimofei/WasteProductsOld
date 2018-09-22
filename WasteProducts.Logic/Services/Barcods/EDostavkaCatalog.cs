using System.Text.RegularExpressions;
using WasteProducts.Logic.Common.Services.Barcods;
using WasteProducts.Logic.Common.Models.Barcods;
using System.Threading.Tasks;
using System;

namespace WasteProducts.Logic.Services.Barcods
{
    /// <inheritdoc />
    public class EDostavkaCatalog : ICatalog
    {
        const string SEARCH_URI_FORMATTER = "https://e-dostavka.by/search/?searchtext={0}";
        const string NAME_PATTERN = "<h1>(.*?)</h1>";
        const string COMPOSITION_PATTERN = @"<div class=""title"">Описание</div><table>.*?Состав.*?class=""value"">(.*?)</td>";
        const string BREND_PATTERN = @"<li class=""product_card_country"">.*?</li><li>.*?<span>(.*?)</span></li>";
        const string COUNTRY_PATTERN = @"<li class=""product_card_country"">.*?<span>(.*?)</span></li>";
        const string PICTURE_PATH_PATTERN = @"<a class=""increaseImage.*?src=""(.*?)""";
        const string DESCRIPTION_URI_PATTERN = @"<!--/noindex--><div class=\""img\""><a href=\""(.*?)""";

        IHttpHelper _httpHelper;

        public EDostavkaCatalog(IHttpHelper httpHelper)
        {
            _httpHelper = httpHelper;
        }

        /// <inheritdoc />
        public async Task<Barcode> GetAsync(string barcode)
        {
            var queryResult = await GetPageAsync(barcode);

            //если страница с товаром найдена
            if (queryResult.StatusCode >= 200 && queryResult.StatusCode < 300)
            {
                var nameParseResult = ParseName(queryResult.Page);

                //минимум что мы хотим знать о товаре, это его имя. если оно найдено - дополняем остальными данными ProductInfo и возвращаем значение
                if (nameParseResult.Success)
                {
                    var result = new Barcode();

                    result.Id = Guid.NewGuid().ToString();
                    result.Code = barcode;
                    result.ProductName = nameParseResult.Value;
                    result.Composition = ParseComposition(queryResult.Page).Value;
                    result.Brend = ParseBrend(queryResult.Page).Value;
                    result.Country = ParseCountry(queryResult.Page).Value;

                    var picturePathParseResult = ParsePicturePath(queryResult.Page);
                    if (picturePathParseResult.Success)
                    {
                        result.Picture = await _httpHelper.DownloadPictureAsync(picturePathParseResult.Value);//замокать
                    }

                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets page.
        /// </summary>
        /// <param name="barcode">String code.</param>
        /// <returns>Model of HttpQueryResult</returns>
        private async Task<HttpQueryResult> GetPageAsync(string barcode)
        {
            var result = new HttpQueryResult()
            {
                StatusCode = 404
            };

            var searchPageURI = string.Format(SEARCH_URI_FORMATTER, barcode);
            var searchPageResult = await _httpHelper.SendGETAsync(searchPageURI);

            if (searchPageResult.StatusCode >= 200 && searchPageResult.StatusCode < 300)
            {
                var descriptionPageURIParseResult = ParseDescriptionPageURI(searchPageResult.Page);

                if(descriptionPageURIParseResult.Success)
                {
                    var descriptionPageResult = await _httpHelper.SendGETAsync(descriptionPageURIParseResult.Value);

                    if (descriptionPageResult.StatusCode >= 200 && descriptionPageResult.StatusCode < 300)
                    {
                        return descriptionPageResult;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets product name.
        /// </summary>
        /// <param name="page">String page.</param>
        /// <returns>Product name</returns>
        private ParseResult ParseName(string page)
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

        /// <summary>
        /// Gets product name.
        /// </summary>
        /// <param name="page">String page.</param>
        /// <returns>Product composition</returns>
        private ParseResult ParseComposition(string page)
        {
            var result = new ParseResult();

            Regex r = new Regex(COMPOSITION_PATTERN);
            Match m = r.Match(page);

            if (m.Success)
            {
                result.Success = true;
                result.Value = m.Groups[1].Value;
            }

            return result;
        }

        /// <summary>
        /// Gets product name.
        /// </summary>
        /// <param name="page">String page.</param>
        /// <returns>Product brand</returns>
        private ParseResult ParseBrend(string page)
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

        /// <summary>
        /// Gets product name.
        /// </summary>
        /// <param name="page">String page.</param>
        /// <returns>Product country</returns>
        private ParseResult ParseCountry(string page)
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

        /// <summary>
        /// Gets product name.
        /// </summary>
        /// <param name="page">String page.</param>
        /// <returns>Product picture path</returns>
        private ParseResult ParsePicturePath(string page)
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

        /// <summary>
        /// Gets product name.
        /// </summary>
        /// <param name="page">String page.</param>
        /// <returns>Product Description Page URI</returns>
        private ParseResult ParseDescriptionPageURI(string pageHTML)
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
