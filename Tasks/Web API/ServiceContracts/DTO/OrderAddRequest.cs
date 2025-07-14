using OrderManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class OrderAddRequest
    {
        [Required(ErrorMessage = "The CustomerName field is required.")]
        [StringLength(50, ErrorMessage = "The CustomerName field must not exceed 50 characters.")]
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "The OrderNumber field is required.")]
        [RegularExpression(@"^(?i)ORD_\d{4}_\d+$\r\n", ErrorMessage = "The Order number should begin with 'ORD' followed by an underscore (_) and a sequential number.")]
        public string? OrderNumber { get; set; }

        [Required(ErrorMessage = "The OrderDate field is required.")]
        public DateTime OrderDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The TotalAmount field must be a positive number.")]
        public decimal TotalAmount { get; set; }

        public List<OrderItemAddRequest> OrderItems { get; set; } = new List<OrderItemAddRequest>();

        public Order ToOrder()
        {
            return new Order
            {
                CustomerName = CustomerName,
                OrderNumber = OrderNumber,
                OrderDate = OrderDate,
                TotalAmount = TotalAmount
            };
        }
    }
}
