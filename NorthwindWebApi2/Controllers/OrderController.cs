﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Northwind.Core.BusinessLayer.Contracts;
using Northwind.Core.EntityLayer;
using NorthwindWebApi2.Models;
using NorthwindWebApi2.Services;

namespace NorthwindWebApi2.Controllers
{
    public class OrderController : ApiController
    {
        protected ISalesBusinessObject Uow;

        public OrderController(IBusinessObjectService service)
        {
            Uow = service.GetSalesBusinessObject();
        }

        protected override void Dispose(Boolean disposing)
        {
            if (Uow != null)
            {
                Uow.Release();
            }

            base.Dispose(disposing);
        }

        // GET: api/Order
        public async Task<HttpResponseMessage> Get()
        {
            var result = new ApiResponse();

            try
            {
                var list = await Uow.GetOrderSummaries();

                result.Model = list.ToList();
            }
            catch (Exception ex)
            {
                result.DidError = true;

                result.ErrorMessage = ex.Message;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // GET: api/Order/5
        public async Task<HttpResponseMessage> Get(Int32 id)
        {
            var result = new ApiResponse();

            try
            {
                result.Model = await Uow.GetOrder(new Order(id));
            }
            catch (Exception ex)
            {
                result.DidError = true;

                result.ErrorMessage = ex.Message;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/Order
        public async Task<HttpResponseMessage> Post([FromBody]Order value)
        {
            var result = new ApiResponse();

            try
            {
                await Task.Run(() =>
                {
                    Uow.CreateOrder(value);
                });

                result.Model = value;
            }
            catch (Exception ex)
            {
                result.DidError = true;

                result.ErrorMessage = ex.Message;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // DELETE: api/Order/5
        public void Delete(Int32 id)
        {
        }
    }
}
