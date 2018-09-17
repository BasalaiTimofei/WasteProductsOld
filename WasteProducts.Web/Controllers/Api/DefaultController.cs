using System;
using System.Collections.Generic;
using System.Web.Http;
using FluentValidation;
using Ninject.Extensions.Logging;

namespace WasteProducts.Web.Controllers.Api
{
    public class DefaultController : BaseApiController
    {
        // GET: api/Default
        public IEnumerable<string> Get()
        {
            throw new Exception("Api get ecception");
            return new string[] { "value1", "value2" };
        }

        // GET: api/Default/5
        public string Get(int id)
        {
            throw new ValidationException("Api get validation ecception");
            return "value";
        }

        // POST: api/Default
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Default/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Default/5
        public void Delete(int id)
        {
        }

        /// <inheritdoc />
        public DefaultController(ILogger logger) : base(logger)
        {
        }
    }
}
