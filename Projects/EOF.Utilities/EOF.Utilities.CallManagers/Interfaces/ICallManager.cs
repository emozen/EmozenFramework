using EOF.Utilities.CallManagers.Models;
namespace EOF.Utilities.CallManagers.Interfaces
{
    public interface ICallManager
    {
        public CallManager SetUrl(Uri url);
        public CallManager SetHeader(Dictionary<string, string>? headers);
        public CallManager SetBody<TRequest>(TRequest body);
        public Task<ResponseModel?> CallApi();
    }
}
