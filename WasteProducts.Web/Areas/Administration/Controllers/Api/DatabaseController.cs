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
    /// <summary>
    /// Api controller for database management
    /// </summary>
    [RoutePrefix("api/Administration/Database")]
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
        /// Checks database status 
        /// </summary>
        /// <returns>DatabaseStatus</returns>
        [HttpGet]
        [Route("")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Returns database status", typeof(DatabaseStatus), nameof(DatabaseStatus), "application/json")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized request.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Exceptions during the process.")]
        public async Task<IHttpActionResult> Get()
        {
            using (_dbService)
            {
                var status = await Task.FromResult(_dbService.GetStatus()).ConfigureAwait(true);
                return Ok(status);
            }
        }

        /// <summary>
        /// Creates new database if database not exist
        /// </summary>
        /// <returns>DatabaseResult</returns>
        [HttpPost]
        [Route("")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Created, "Database was successfully created and seeded.")]
        [SwaggerResponse(HttpStatusCode.Conflict, "Database already exists.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized request.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Exceptions during the creation of a database or seeding process.")]
        public async Task<IHttpActionResult> Post(bool seedTestData)
        {
            using (_dbService)
            {
                if (await _dbService.CreateAndSeedAsync(seedTestData).ConfigureAwait(true))
                    return StatusCode(HttpStatusCode.Created);

                return Conflict();
            }
        }

        /// <summary>
        /// Deletes database if database exist
        /// </summary>
        /// <returns>DatabaseResult</returns>
        [HttpDelete]
        [Route("")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Database was successfully deleted")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Database don't exist or was exceptions in the deleting process")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized request.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Exceptions during the database deletion process.")]
        public async Task<IHttpActionResult> Delete()
        {
            using (_dbService)
            {
                if (await Task.FromResult(_dbService.Delete()).ConfigureAwait(true))
                    return Ok();

                return NotFound();
            }
        }
    }
}
