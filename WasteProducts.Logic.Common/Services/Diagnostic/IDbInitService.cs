using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Services.Diagnostic
{
    public interface IDbInitService
    {
        Task InitAsync(bool useTestData);
    }
}