using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Coffee.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Coffee.Controllers
{
    [RoutePrefix("api/coffeeshop")]
    public class CoffeeShopController : ApiController
    {
        private readonly CoffeeShopContext _data = new CoffeeShopContext();

        [Route("{id:int}/order/{orderId}", Name = "GetOrder")]
        [HttpGet]
        public HttpResponseMessage GetOrder(int id, string orderId)
        {
            var order = (from o in _data.Orders.AsQueryable()
                         where o.CoffeeShopId == id.ToString() && o.Id == orderId
                         select o).FirstOrDefault();

            if (order == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Order does not exist");
            }

            return Request.CreateResponse(HttpStatusCode.OK, order);
        }

        [Route("{id:int}/order", Name = "SubmitOrder")]
        [HttpPost]
        public HttpResponseMessage SubmitOrder(int id, [FromBody] Order order)
        {
            order.CoffeeShopId = id.ToString();
            _data.Orders.Save(order);

            var response = Request.CreateResponse(HttpStatusCode.Created, order);
            response.Headers.Location = new Uri(Url.Link("GetOrder", new { id = id, orderId = order.Id }));
            return response;
        }
    }
}