using WasteProducts.Logic.Common.Models.Barcods;
using System.Drawing;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Services.Barcods
{
    /// <summary>
    /// This interface provides barcodes methods.
    /// </summary>
    public interface IHttpHelper
    {
        /// <summary>
        /// Gets HttpQueryResult.
        /// </summary>
        /// <param name="uri">String code.</param>
        /// <returns>Model of HttpQueryResult</returns>
        Task<HttpQueryResult> SendGETAsync(string uri);

        /// <summary>
        /// Gets image of product.
        /// </summary>
        /// <param name="uri">String code.</param>
        /// <returns>Image</returns>
        Task<Image> DownloadPictureAsync(string uri);
    }
}
