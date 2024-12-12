using OrderProcessingSystem.Workflows.Activities;
using OrderProcessingSystem.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Temporalio.Workflows;
using static OrderProcessingSystem.Workflows.Data.Constants;

namespace OrderProcessingSystem.Workflows.Workflows
{
    // 1. Customer placing an order will trigger the workflow
    // 2. An order will have 2 individual states, one related to Order tracking and the other related to Order Payment
    // 3. This Application will work completely internally i.e. there is no DB update or any external services receiving signals from here

    [Workflow]
    public class OrderProcessingWorkflow
    {
        private static Invoice _invoice = new();

        private static State _state;
        private static PaymentStatus _paymentStatus;
        private readonly Guid _OrderId = new Guid();
        private readonly OrderRelatedActions? _relatedActions;


        [WorkflowRun]
        public async Task RunAsync(Order order, Customer customer)
        {
            var workflowInfo = Workflow.Info;

            _invoice.Customer = customer;
            _invoice.Order = order;
            _invoice.TransactionDate = DateTime.Now;
            _invoice.TransanctionId = Guid.NewGuid();
            _invoice.InvoiceId = $"{_invoice.TransanctionId.ToString()}_{customer.CustomerId}";
            _invoice.EstimatedDeliveryDate = DateTime.Now.AddDays(7);

            //Notify Supplier about the Order
            await Workflow.ExecuteActivityAsync(
                (OrderRelatedActions actions) => actions.NotifySupplier(order),
                new()
                {
                    StartToCloseTimeout = TimeSpan.FromMinutes(2)
                });

            await Workflow.WaitConditionAsync(() => _state != State.Transition);

            await Workflow.WaitConditionAsync(() => _state != State.Shipped);

            await Workflow.WaitConditionAsync(() => _state != State.Delivered && _paymentStatus != PaymentStatus.Confirmed);

        }

        [WorkflowSignal]
        public static Task UpdateOrderState(State state)
        {
            // Update the order state to the provided value
            _state = state;
            return Task.CompletedTask;
        }

        [WorkflowSignal]
        public static Task UpdatePaymentStatus(PaymentStatus paymentStatus)
        {
            // Update the payment status to the provided value
            _paymentStatus = paymentStatus;
            return Task.CompletedTask;
        }

        [WorkflowQuery]
        public static async Task<byte[]> GetInvoice()
        {
            return OrderRelatedActions.GenerateInvoice(_invoice);               //return pdf as byte data
        }
    }
}
