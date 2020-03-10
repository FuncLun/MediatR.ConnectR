namespace MediatR.ConnectR.AspNetCore
{
    public class ConnectorMiddlewareOptions
    {
        public bool BreakOnException { get; set; }
        public bool ReturnExceptionMessage { get; set; }
        public bool ReturnExceptionDetails { get; set; }
    }
}
