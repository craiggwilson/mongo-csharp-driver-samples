using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Coffee.Models;

namespace Coffee.Controllers
{
    [RoutePrefix("api/coffeeshop")]
    public class CoffeeShopController : ApiController
    {
        [Route("{id:int}/order")]
        [HttpPost]
        public HttpResponseMessage Order(int id, [FromBody] DrinkOrder drinker)
        {
            return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "not yet");
        }
    }
}