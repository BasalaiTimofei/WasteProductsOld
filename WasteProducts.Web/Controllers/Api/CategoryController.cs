using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Ninject.Extensions.Logging;
using Swagger.Net.Annotations;
using WasteProducts.Logic.Common.Models.Products;
using WasteProducts.Logic.Common.Services.Products;

namespace WasteProducts.Web.Controllers.Api
{
    /// <summary>
    /// Controller that performs actions on categories and gives the client the corresponding response.
    /// </summary>
    [RoutePrefix("api/category")]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="categoryService">Category service.</param>
        /// <param name="logger">NInject logger.</param>
        public CategoryController(ICategoryService categoryService, ILogger logger) : base(logger)
        {
            _categoryService = categoryService;
        }


        /// <summary>
        /// Gets the list of all categories.
        /// </summary>
        /// <returns>All categories from database.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "GetAll categories result", typeof(IEnumerable<Category>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Categories were not found in database")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            return Ok(await _categoryService.GetAll());
        }

        /// <summary>
        /// Gets category by id.
        /// </summary>
        /// <param name="id">Category's id.</param>
        /// <returns>Category with the specific id.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "GetById category result", typeof(Category))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect Id")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> GetById([FromUri] string id)
        {
            return Ok(await _categoryService.GetById(id));
        }

        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Created, "Category was successfully created", typeof(Category))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect name")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        [HttpPost, Route("")]
        public async Task<IHttpActionResult> CreateCategory([FromBody]string name)
        {
            var id = await _categoryService.Add(name);

            if (id == null) return BadRequest("Category exists");

            return Created("api/category/"+id, await _categoryService.GetById(id));
        }

        /// <summary>
        /// Deletes the category by id.
        /// </summary>
        /// <param name="id">Id of the category to be deleted.</param>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "Product was successfully deleted")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect Id")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        [HttpDelete, Route("{id}")]
        public async Task Delete([FromUri] string id)
        {
            await _categoryService.Delete(id);
        }

        /// <summary>
        /// Updates the category .
        /// </summary>
        /// <param name="data">Data by which the category should be updated.</param>
        /// <returns>Updated category.</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Product was successfully updated", typeof(Category))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Incorrect data")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled exception has been thrown during the request.")]
        [HttpPut, Route("")]
        public async Task<IHttpActionResult> UpdateCategory([FromBody]Category data)
        {
            await _categoryService.Update(data);

            return Ok(await _categoryService.GetById(data.Id));
        }
    }
}