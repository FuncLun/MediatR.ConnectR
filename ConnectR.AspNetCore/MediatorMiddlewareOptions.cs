using Newtonsoft.Json;

namespace MediatR.ConnectR.AspNetCore
{
    public class MediatorMiddlewareOptions
    {
        public JsonSerializerSettings JsonSerializerSettings { get; set; }
        public bool BreakOnException { get; set; }
        public bool ReturnExceptionMessage { get; set; }
        public bool ReturnExceptionDetails { get; set; }
    }
}
