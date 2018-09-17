using System;
using System.Drawing;
using System.IO;
using System.Net;
using WasteProducts.Logic.Common.Models.Barcods;
using WasteProducts.Logic.Common.Services.Barcods;

namespace WasteProducts.Logic.Services.Barcods
{
    public class HttpHelper: IHttpHelper
    {
        public HttpQueryResult SendGET(string uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                return new HttpQueryResult() {
                    StatusCode = (int)response.StatusCode,
                    Page = reader.ReadToEnd()
                };
            }
            catch (Exception e)
            {
                return new HttpQueryResult();
            }
        }

        public Image DownloadPicture(string uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                return Image.FromStream(response.GetResponseStream());
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
