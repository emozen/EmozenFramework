using EOF.Utilities.CallManagers.Interfaces;
using EOF.Utilities.CallManagers.Models;
using System.Net;
using System.Text;
using System.Text.Json;

namespace EOF.Utilities.CallManagers
{
    public class CallManager(IHttpClientFactory clientFactory) : ICallManager
    {
        private readonly RequestModel _requestModel = new();

        public CallManager SetRequestType(HttpMethod httpMethod)
        {
            _requestModel.HttpMethod = httpMethod;
            return this;
        }

        public CallManager SetUrl(Uri url)
        {
            _requestModel.Url = url;
            return this;
        }

        public CallManager SetHeader(Dictionary<string,string>? headers)
        {
            _requestModel.Headers = headers;
            return this;
        }

        public CallManager SetBody<TRequest>(TRequest body)
        {
            _requestModel.Body = JsonSerializer.Serialize(body);
            return this;
        }

        /// <summary>
        /// Api cagirirken default olarak mediatype 'application/json' olarak gitmektedir.
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel?> CallApi()
        {
            var responseModel = new ResponseModel();
            try
            {
                var url = _requestModel.Url;
                var body = _requestModel.Body;
                var request = new HttpRequestMessage(_requestModel.HttpMethod, $"{url}")
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                };

                if (_requestModel.Headers != null)
                {
                    foreach (var item in _requestModel.Headers)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }

                using var client = clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsStringAsync();
                    responseModel.Body = res;
                    responseModel.Status = HttpStatusCode.OK;
                }
                else
                {
                    responseModel.Message = response.ReasonPhrase;
                    responseModel.Status = response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                responseModel.Exception = ex;
                responseModel.Message = "CallApi has error!!!";
            }

            return await Task.FromResult(responseModel);
        }
    }
}
