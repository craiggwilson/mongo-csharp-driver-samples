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

        [Route("{id}/order/{orderId}", Name = "GetOrder")]
        [HttpGet]
        public HttpResponseMessage GetOrder(string id, string orderId)
        {
            var order = (from o in _data.Orders.AsQueryable()
                         where o.CoffeeShopId == id && o.Id == orderId
                         select o).FirstOrDefault();

            if (order == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Order does not exist");
            }

            return Request.CreateResponse(HttpStatusCode.OK, order);
        }

        [Route("", Name = "GetShops")]
        [HttpGet]
        public HttpResponseMessage GetShops(double? latitude = null, double? longitude = null)
        {
            IMongoQuery query = Query.Null;
            if (latitude.HasValue && longitude.HasValue)
            {
                query = Query<Shop>.Near(x => x.Location, longitude.Value, latitude.Value);
            }

            var shops = _data.Shops.Find(query).SetLimit(10).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, shops);
        }

        [Route("{id}/order", Name = "SubmitOrder")]
        [HttpPost]
        public HttpResponseMessage SubmitOrder(string id, [FromBody] Order order)
        {
            order.CoffeeShopId = id;
            _data.Orders.Save(order);

            var response = Request.CreateResponse(HttpStatusCode.Created, order);
            response.Headers.Location = new Uri(Url.Link("GetOrder", new { id = id, orderId = order.Id }));
            return response;
        }
    }
}