using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessingSystem.Workflows.Data
{
    public static class Constants
    {
        public enum State
        {
            Placed,         //Order has been placed
            Transition,     //Order is in transition from supplier to local distributor
            Failed,         //Order placement failed
            Shipped,        //Order has been shipped
            OutforDelivery, //Order is out for delivery
            Delivered,      //Order has been delivered
            Cancelled       //Order has been cancelled
        }

        public enum PaymentStatus
        {
            Pending,         // Payment is pending
            Confirmed,       // Payment has been confirmed
            Failed,          // Payment failed
            Refunded         // Payment has been refunded
        }
    }
}
