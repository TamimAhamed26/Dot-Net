﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderFullfillment.Application.Services.Interfaces;
using OrderFullfillment.Application.ViewModels.Order;
using OrderFullfillment.Entity.Entities.Orders;

namespace OrderFullfillment.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpGet("{id:int}")]
        public async Task<Order> Get(int id)
        {
            _logger.LogInformation("Getting order id {Id}", id);
            return await _orderService.Get(id);
        }
        
        [HttpPost]
        public async Task<Order> CreateOrder(OrderRequestVM orderInfo)
        {
            _logger.LogInformation("Creating order");
            return await _orderService.CreateOrder(orderInfo);
        }
    }
}