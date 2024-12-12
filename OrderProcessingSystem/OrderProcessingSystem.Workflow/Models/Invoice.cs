using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessingSystem.Workflows.Models
{
    public class Invoice
    {
        public string InvoiceId { get; set; }
        public Guid TransanctionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public Customer? Customer { get; set; }
        public Order? Order { get; set; }
        public int TotalPrice => Order?.Items?.Sum(x => x.UnitPrice * x.Quantity) ?? 0;
    }

    public class Customer
    {
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerAddress { get; set; }
    }

    public class Order
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<Item>? Items { get; set; }
    }

    public class Item
    {
        public long ItemId { get; set; }
        public string? ItemName { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
