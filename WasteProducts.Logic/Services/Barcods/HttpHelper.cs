using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services.Barcods;

namespace WasteProducts.Logic.Services.Barcods
{
    /// <inheritdoc />
    public class HttpHelper: IHttpHelper
    {
        private  Image _image;

        /// <inheritdoc />
        public async Task<HttpQueryResult> SendGETAsync(string uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                return new HttpQueryResult() {
                    StatusCode = (int)response.StatusCode,
                    Page = await reader.ReadToEndAsync()
                };
            }
            catch (Exception e)
            {
                return new HttpQueryResult();
            }
        }

        /// <inheritdoc />
        public async Task<Image> DownloadPictureAsync(string uri)
        {
            try
            {
                await Task.Run(() =>
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    _image = Image.FromStream(response.GetResponseStream());
                });
                return _image;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
