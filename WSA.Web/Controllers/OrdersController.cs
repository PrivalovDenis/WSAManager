using AutoMapper;
using WSAManager.Core.Entities;
using WSAManager.Core.Services;
using WSAManager.Dto.Dtos;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Swashbuckle;
using Swashbuckle.Swagger.Annotations;
using System.Web.Http.Description;
using System.Net;
using System.Linq;

namespace WSAManager.Web.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="service"></param>
        public OrdersController(IOrderService service)
        {
            _orderService = service;
        }

        public OrdersController(IOrderService orderService, IOrderItemService orderItemService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
        }

        /// <summary>
        /// Get Order List
        /// </summary>
        /// <returns>Returns List typeof(OrderDto)</returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetOrders()
        {
            try
            {
                var orders = await _orderService.GetAllAsync();

                var ordersDto = new List<OrderDto>();

                Mapper.CreateMap<Order, List<OrderDto>>();
                Mapper.Map(orders, ordersDto);

                return Ok(ordersDto);
            }
            catch (Exception ex)
            {
                //TODO: Log something here
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get Order By Order Id
        /// </summary>
        /// <param name="id">Order Identifier</param>
        /// <returns>Retruns instance typeof(OrderDto)</returns>
        [Route("ById/{id:int}")]
        public async Task<IHttpActionResult> GetOrder(int id)
        {
            try
            {
                Order order = await _orderService.GetByIdAsync(id);

                if (order == null)
                {
                    return NotFound();
                }

                var orderDto = new GetOrderDto();

                Mapper.CreateMap<Order, GetOrderDto>().IgnoreAllPropertiesWithAnInaccessibleSetter();
                Mapper.Map(order, orderDto);

                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                //TODO: Log something here
                return InternalServerError();
            }
        }

        /// <summary>
        /// Update exiting Order By Id
        /// </summary>
        /// <param name="id">Order Identifier</param>
        /// <param name="orderDto">Order entity</param>
        /// <returns>Returns instance typeof(OrderDto)</returns>
        [HttpPut]
        public async Task<IHttpActionResult> PutOrder(int id,  OrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderDto.Id)
            {
                return BadRequest();
            }

            try
            {
                Order order = await _orderService.GetByIdAsync(id);

                order.ClientId = orderDto.ClientId;
                order.DateBegin = Convert.ToDateTime(orderDto.DateBegin);
                order.PhoneNumber = orderDto.PhoneNumber;
                order.StatusId = orderDto.StatusId;
                order.Comment = orderDto.Comment;

                await _orderService.UpdateAsync(order);
              
                return Ok(orderDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Create new Order
        /// </summary>
        /// <param name="orderDto">Order entity</param>
        /// <returns>Returns instance typeof(OrderDto)</returns>
        [HttpPost]
        public async Task<IHttpActionResult> PostOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try {

                Order order = new Order();

                order.ClientId = orderDto.ClientId;
                order.DateBegin = Convert.ToDateTime(orderDto.DateBegin);
                order.PhoneNumber = orderDto.PhoneNumber;
                order.StatusId = orderDto.StatusId;
                order.Comment = orderDto.Comment;

                order = await _orderService.AddAsync(order);

                orderDto.Id = order.Id;
                return CreatedAtRoute("ApiRoute", new { id = orderDto.Id }, orderDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                //Log something her...
                return InternalServerError();
            }
        }

        /// <summary>
        /// Delete Order By id
        /// </summary>
        /// <param name="id">Order Identifier</param>
        /// <returns>Returns 200 if Deleted</returns>
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteOrder(int id)
        {
            try
            {
                Order order = await _orderService.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }

                await _orderService.DeleteAsync(order);

                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                //Log something here..
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("AddOrderItem")]
        public async Task<IHttpActionResult> AddOrderItem(OrderItemDto orderItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try {
                var orderItem = new OrderItem();
                orderItem.OrderId = orderItemDto.OrderId;
                orderItem.ProductId = orderItemDto.ProductId;
                orderItem.Quantity = orderItemDto.Quantity;

                var createdOrderItem = _orderItemService.AddAsync(orderItem);

                orderItemDto.Id = createdOrderItem.Id;
                return CreatedAtRoute("ApiRoute", new { id = orderItemDto.Id }, orderItemDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                //Log something here..
                return InternalServerError();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _orderService.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IsOrderExists(int id)
        {
            return _orderService.GetByIdAsync(id) != null;
        }


        
    }
}