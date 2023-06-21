using System;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace TaskManager.Api
{
    public static class ExceptionExtensions
    {
        public static RpcException Handle<T>(
            this Exception exception,
            ILogger<T> logger,
            Guid correlationId)
        {
            logger.LogError(exception, $"CorrelationId: {correlationId} - An error occurred");
            return new RpcException(new Status(StatusCode.Internal, exception.Message), CreateTrailers(correlationId));
        }

        private static Metadata CreateTrailers(Guid correlationId)
        {
            return new Metadata { { "CorrelationId", correlationId.ToString() } };;
        }
    }
}