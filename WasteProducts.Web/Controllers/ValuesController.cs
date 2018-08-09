using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WasteProducts.Web.Controllers
{
    public class ValuesController : Base.BaseApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            throw new HttpException(403, "Доступ запрещён!"); //
            return "value";
        }
    }
}