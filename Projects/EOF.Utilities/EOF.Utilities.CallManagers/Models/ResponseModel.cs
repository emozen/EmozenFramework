using System.Net;

namespace EOF.Utilities.CallManagers.Models
{
    public class ResponseModel
    {
        public string? Body { get; set; }
        public HttpStatusCode? Status { get; set; }
        public string? Message { get; set; }
        public Exception? Exception { get; set; }
    }
}
