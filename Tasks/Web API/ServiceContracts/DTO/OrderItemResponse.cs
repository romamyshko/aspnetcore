using OrderManagement.Entities;
using System;
using System.Collections.Generic;

namespace ServiceContracts.DTO
{
    public class OrderItemResponse
    {
        public Guid OrderItemId { get; set; }

        public Guid OrderId { get; set; }

        public string? ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }

    public static class OrderItemResponseExtensions
    {
        public static OrderItemResponse ToOrderItemResponse(this OrderItem orderItem)
        {
            return new OrderItemResponse
            {
                OrderItemId = orderItem.OrderItemId,
                OrderId = orderItem.OrderId,
                ProductName = orderItem.ProductName,
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice,
                TotalPrice = orderItem.TotalPrice
            };
        }

        public static List<OrderItemResponse> ToOrderItemResponseList(this List<OrderItem> orderItems)
        {
            var orderItemResponses = new List<OrderItemResponse>();
            foreach (var orderItem in orderItems)
            {
                orderItemResponses.Add(orderItem.ToOrderItemResponse());
            }
            return orderItemResponses;
        }
    }
}
