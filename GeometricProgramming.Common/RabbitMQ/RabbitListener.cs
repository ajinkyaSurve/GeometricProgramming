using Microsoft.Extensions.Configuration;
using RestSharp;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using System;

namespace GeometricProgramming.Common
{
    public class RabbitListener
    {
        private readonly IRestClient restClient;

        public RabbitListener(IRestClient client)
        {
            this.restClient = client;
        }

        [RabbitListener("Geometry.Inputs")]
        public void ListenForMessage(string message)
        {
            System.Diagnostics.Trace.WriteLine("Geometry Input: {Message}", message);
            var request = new RestRequest(message, DataFormat.Json);
            _ = restClient.Post(request);
        }
    }
}
