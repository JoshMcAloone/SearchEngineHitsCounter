using System;

namespace SearchEngineHitCounter.Exceptions
{
    public class ExternalApiException : Exception
    {
        public ExternalApiException()
            : base("External API exception")
        {

        }

        public ExternalApiException(string message, string externalApiName, Exception innerException)
            : base(message, innerException)
        {
            ExternalApiName = externalApiName;
        }

        public string ExternalApiName { get; }
    }
}
