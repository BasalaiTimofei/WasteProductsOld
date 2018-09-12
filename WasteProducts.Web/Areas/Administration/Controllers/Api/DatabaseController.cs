using NLog;
using Swagger.Net.Annotations;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using WasteProducts.Logic.Common.Models.Diagnostic;
using WasteProducts.Logic.Common.Services.Diagnostic;
using WasteProducts.Web.Controllers.Api;

namespace WasteProducts.Web.Areas.Administration.Controllers.Api
{
    /// <summary>
    /// Api controller for database management
    /// </summary>
    [RoutePrefix("api/administration/database")]
    [SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized request.")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, "Exceptions during the process.")]
    public class DatabaseController : BaseApiController
    {
        private readonly IDbService _dbService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbService">Database management service</param>
        /// <param name="logger">NLog logger</param>
        public DatabaseController(IDbService dbService, ILogger logger) : base(logger)
        {
            _dbService = dbService;
        }

        /// <summary>
        /// Returns current database state 
        /// </summary>
        /// <returns>DatabaseState</returns>
        [HttpGet, Route("state")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Returns current database state", typeof(DatabaseState), nameof(DatabaseState), "application/json")]
        public async Task<IHttpActionResult> GetDatabaseStateAsync()
        {
            using (_dbService)
            {
                var status = await _dbService.GetStateAsync().ConfigureAwait(true);
                return Ok(status);
            }
        }

        /// <summary>
        /// Deletes old database if it exists and creates new database
        /// </summary>
        /// <returns>Task</returns>
        [HttpGet, Route("recreate")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "Database was created and seeded.")]
        public async Task<IHttpActionResult> ReCreateDatabaseAsync(bool withTestData)
        {
            using (_dbService)
            {
                await _dbService.ReCreateAsync(withTestData).ConfigureAwait(true);
                return StatusCode(HttpStatusCode.NoContent);
            }
        }

        /// <summary>
        /// Deletes database if database exist, otherwise does nothing
        /// </summary>
        /// <returns>Task</returns>
        [HttpDelete, Route("delete")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "Database was deleted.")]
        public async Task<IHttpActionResult> DeleteDatabaseAsync()
        {
            using (_dbService)
            {
                await _dbService.DeleteAsync().ConfigureAwait(true);
                return StatusCode(HttpStatusCode.NoContent);
            }
        }
    }
}
