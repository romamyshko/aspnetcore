using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderManagement.ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersAdderService _ordersAdderService;
        private readonly IOrdersGetterService _ordersGetterService;
        private readonly IOrdersUpdaterService _ordersUpdaterService;
        private readonly IOrdersDeleterService _ordersDeleterService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(
            IOrdersAdderService ordersAdderService,
            IOrdersGetterService ordersGetterService,
            IOrdersUpdaterService ordersUpdaterService,
            IOrdersDeleterService ordersDeleterService,
            ILogger<OrdersController> logger)
        {
            _ordersAdderService = ordersAdderService;
            _ordersGetterService = ordersGetterService;
            _ordersUpdaterService = ordersUpdaterService;
            _ordersDeleterService = ordersDeleterService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<OrderResponse>>> GetAllOrders()
        {
            _logger.LogInformation("Retrieving all orders");

            var orders = await _ordersGetterService.GetAllOrders();

            _logger.LogInformation("All orders retrieved successfully");

            return Ok(orders);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderResponse>> GetOrderById(Guid id)
        {
            _logger.LogInformation($"Retrieving order with ID: {id}");

            var order = await _ordersGetterService.GetOrderByOrderId(id);

            if (order == null)
            {
                _logger.LogWarning($"Order with ID {id} not found");
                return NotFound();
            }

            _logger.LogInformation($"Order with ID {id} retrieved successfully");

            return Ok(order);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<OrderResponse>> AddOrder(OrderAddRequest orderRequest)
        {
            _logger.LogInformation("Adding a new order");

            var addedOrder = await _ordersAdderService.AddOrder(orderRequest);

            _logger.LogInformation($"Order with ID {addedOrder.OrderId} added successfully");

            return CreatedAtAction(nameof(GetOrderById), new { id = addedOrder.OrderId }, addedOrder);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderResponse>> UpdateOrder(Guid id, OrderUpdateRequest orderRequest)
        {
            if (id != orderRequest.OrderId)
            {
                _logger.LogWarning($"Mismatched ID between route parameter ({id}) and order request ({orderRequest.OrderId})");
                return BadRequest();
            }

            _logger.LogInformation($"Updating order with ID: {id}");

            var updatedOrder = await _ordersUpdaterService.UpdateOrder(orderRequest);

            _logger.LogInformation($"Order with ID {id} updated successfully");

            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            _logger.LogInformation($"Deleting order with ID: {id}");

            var isDeleted = await _ordersDeleterService.DeleteOrderByOrderId(id);

            if (!isDeleted)
            {
                _logger.LogWarning($"Order with ID {id} not found");
                return NotFound();
            }

            _logger.LogInformation($"Order with ID {id} deleted successfully");

            return NoContent();
        }
    }
}
