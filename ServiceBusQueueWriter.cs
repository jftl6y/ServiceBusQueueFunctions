using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure.Messaging.ServiceBus;
using Azure.Identity;

namespace ServiceBusQueueFunc
{
    public class ServiceBusQueueWriter
    {
        private readonly ILogger<ServiceBusQueueWriter> _logger;

        public ServiceBusQueueWriter(ILogger<ServiceBusQueueWriter> logger)
        {
            _logger = logger;
        }

        [Function("ServiceBusQueueWriter")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            string fullyQualifiedNamespace = "azfeddev.servicebus.usgovcloudapi.net";
            string queueName = "testTopic";

            DefaultAzureCredentialOptions options = new DefaultAzureCredentialOptions();
            options.AuthorityHost = AzureAuthorityHosts.AzureGovernment;

            await using var client = new ServiceBusClient(fullyQualifiedNamespace, new DefaultAzureCredential(options));
            
            var sender = client.CreateSender(queueName);
            await sender.SendMessageAsync(new ServiceBusMessage("Hello, Azure!"));


            _logger.LogInformation("C# HTTP trigger function sent a message to Service Bus.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
