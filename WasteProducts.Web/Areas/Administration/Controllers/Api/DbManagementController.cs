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
    public class DbManagementController : BaseApiController
    {
        private readonly IDbManagementService _dbManagementService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbManagementService">Database management service</param>
        /// <param name="logger">NLog logger</param>
        public DbManagementController(IDbManagementService dbManagementService, ILogger logger) : base(logger)
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
        /// Creates new database if database not exist
        /// </summary>
        /// <returns>DatabaseResult</returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.Created, "Returns result of create action with Db", typeof(DatabaseResult), nameof(DatabaseResult), "application/json")]
        public async Task<IHttpActionResult> Post()
        {
            using (_dbManagementService)
            {
                return Created("Database server",await _dbManagementService.CreateAsync());
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

        /// <summary>
        /// Seeds database if database not exist
        /// </summary>
        /// <param name="seedTestData">If <c>true</c> also will be seeded test data.</param>
        /// <returns>DatabaseResult</returns>
        [HttpPut]
        [SwaggerResponse(HttpStatusCode.OK, "Returns result of seed action with Db", typeof(DatabaseResult), nameof(DatabaseResult), "application/json")]
        public async Task<IHttpActionResult> Put(bool seedTestData)
        {
            using (_dbManagementService)
            {
                return Ok(await _dbManagementService.SeedAsync(seedTestData));
            }
        }
    }
}
