using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TocLoaderApp
{
    public class ApiKeyHandler : DelegatingHandler
    {
        private string ApiKey;

        public ApiKeyHandler(string apiKey) : base()
        {
            ApiKey = apiKey;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //request.RequestUri = new Uri(request.RequestUri.AbsoluteUri + "?api_key=n0tAr3alAp1K3y");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("apikey", ApiKey);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
