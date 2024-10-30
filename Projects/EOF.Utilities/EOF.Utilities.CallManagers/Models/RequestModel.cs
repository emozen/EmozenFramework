namespace EOF.Utilities.CallManagers.Models
{
    public class RequestModel
    {
        public HttpMethod HttpMethod { get; set; }
        public Uri Url { get; set; }
        public Dictionary<string,string>? Headers{ get; set; }
        public string? Body { get; set; }
    }
}
