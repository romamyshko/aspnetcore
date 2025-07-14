using OrderManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class OrderItemAddRequest
    {
        public Guid OrderId { get; set; }

        [Required(ErrorMessage = "The ProductName field is required.")]
        [StringLength(50, ErrorMessage = "The ProductName field must not exceed 50 characters.")]
        public string? ProductName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The Quantity field must be a positive number.")]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The UnitPrice field must be a positive number.")]
        public decimal UnitPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The total price of the order item.")]
        public decimal TotalPrice { get; set; }

        public OrderItem ToOrderItem()
        {
            return new OrderItem
            {
                OrderId = OrderId,
                ProductName = ProductName,
                Quantity = Quantity,
                UnitPrice = UnitPrice,
                TotalPrice = TotalPrice
            };
        }
    }

    public static class OrderItemAddRequestExtensions
    {
        public static List<OrderItem> ToOrderItems(this List<OrderItemAddRequest> orderItemRequests)
        {
            var orderItems = new List<OrderItem>();
            foreach (var orderItemRequest in orderItemRequests)
            {
                var orderItem = new OrderItem
                {
                    OrderId = orderItemRequest.OrderId,
                    ProductName = orderItemRequest.ProductName,
                    Quantity = orderItemRequest.Quantity,
                    UnitPrice = orderItemRequest.UnitPrice,
                    TotalPrice = orderItemRequest.TotalPrice
                };

                orderItems.Add(orderItem);
            }

            return orderItems;
        }
    }
}
