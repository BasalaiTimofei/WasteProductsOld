using WasteProducts.Logic.Common.Models.Barcods;
using System.Drawing;

namespace WasteProducts.Logic.Common.Services.Barcods
{
    public interface IHttpHelper
    {
        HttpQueryResult SendGET(string uri);
        Image DownloadPicture(string uri);
    }
}
