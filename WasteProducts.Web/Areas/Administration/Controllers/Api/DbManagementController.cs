using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;
using Swagger.Net.Annotations;
using WasteProducts.Logic.Common.Models.Diagnostic;
using WasteProducts.Logic.Common.Services.Diagnostic;
using WasteProducts.Web.Controllers.Api;

namespace WasteProducts.Web.Areas.Administration.Controllers.Api
{
    public class DbManagementController : BaseApiController
    {
        private readonly IDbManagementService _dbManagementService;

        public DbManagementController(IDbManagementService dbManagementService ,ILogger logger) : base(logger)
        {
            _dbManagementService = dbManagementService;
        }

        /// <summary>
        /// Checks database status 
        /// </summary>
        /// <returns>DatabaseStatus</returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, "Returns Db status object", typeof(DatabaseStatus), nameof(DatabaseStatus), "application/json")]
        public async Task<IHttpActionResult> Get()
        {
            using (_dbManagementService)
            {
                return Ok(await _dbManagementService.GetStatus());
            }
        }

        /// <summary>
        /// Creates new and seeds database if database not exist
        /// </summary>
        /// <param name="useTestData"></param>
        /// <returns>DatabaseResult</returns>
        [HttpPost]
        [Route("{useTestData:bool}")]
        [SwaggerResponse(HttpStatusCode.OK, "Returns result of create & seed actions with Db", typeof(DatabaseResult), nameof(DatabaseResult), "application/json")]
        public async Task<IHttpActionResult> Post(bool useTestData)
        {
            using (_dbManagementService)
            {
                return Ok(await _dbManagementService.CreateAndSeedAsync(useTestData));
            }
        }

        /// <summary>
        /// Deletes database if database exist
        /// </summary>
        /// <returns>DatabaseResult</returns>
        [HttpDelete]
        [SwaggerResponse(HttpStatusCode.OK, "Returns result of delete action with Db", typeof(DatabaseResult), nameof(DatabaseResult), "application/json")]
        public async Task<IHttpActionResult> Delete()
        {
            using (_dbManagementService)
            {
                return Ok(await _dbManagementService.DeleteAsync());
            }
        }
    }
}
