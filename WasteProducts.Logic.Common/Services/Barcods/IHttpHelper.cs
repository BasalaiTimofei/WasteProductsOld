using WasteProducts.Logic.Common.Models.Barcods;
using System.Drawing;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Services.Barcods
{
    public interface IHttpHelper
    {
        Task<HttpQueryResult> SendGETAsync(string uri);

        Task<Image> DownloadPictureAsync(string uri);
    }
}
