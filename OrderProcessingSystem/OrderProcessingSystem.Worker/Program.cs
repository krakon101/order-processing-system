using OrderProcessingSystem.Worker;
using OrderProcessingSystem.Workflows.Activities;
using OrderProcessingSystem.Workflows.Workflows;
using Temporalio.Client;
using Temporalio.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ctx =>
    {
        ctx.AddHostedTemporalWorker(
            clientNamespace: "default",
            clientTargetHost: "localhost:7233",
            taskQueue: "order-processing")
        .AddScopedActivities<OrderRelatedActions>()
        .AddWorkflow<OrderProcessingWorkflow>();


    }).Build();


builder.Run();
