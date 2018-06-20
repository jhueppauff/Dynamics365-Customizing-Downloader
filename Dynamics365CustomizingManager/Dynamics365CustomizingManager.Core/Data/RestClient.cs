//-----------------------------------------------------------------------
// <copyright file="RestClient.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Core.Data
{
    using System.Threading;
    using System.Threading.Tasks;
    using RestSharp;

    /// <summary>
    ///  Rest Client
    /// </summary>
    public class RestClient
    {
        /// <summary>
        /// The timeout value for the rest client
        /// </summary>
        private readonly int timeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class.
        /// </summary>
        /// <param name="timeout">The timeout Value for the rest connection.</param>
        public RestClient(int timeout)
        {
            this.timeout = timeout;
        }

        /// <summary>
        /// Executes the rest request async.
        /// </summary>
        /// <param name="url">Url of the Rest Endpoint</param>
        /// <param name="restHeaders">Array of <see cref="RestHeader"/></param>
        /// <param name="body">JSON Body</param>
        /// <param name="method">Method to call</param>
        /// <returns>Returns <see cref="IRestResponse"/></returns>
        public async Task<IRestResponse> ExecuteRestRequest(string url, RestHeader[] restHeaders, string body = null, Method method = Method.GET)
        {
            RestSharp.RestClient restClient = new RestSharp.RestClient(url);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            RestRequest restRequest = new RestRequest(method)
            {
                Timeout = this.timeout,
                ReadWriteTimeout = this.timeout
            };

            foreach (RestHeader restHeader in restHeaders)
            {
                restRequest.AddHeader(restHeader.KeyName, restHeader.KeyValue);
            }

            if (body != null)
            {
                restRequest.AddParameter("undefined", body, ParameterType.RequestBody);
            }

            IRestResponse restResponse = await restClient.ExecuteTaskAsync(restRequest, cancellationTokenSource.Token).ConfigureAwait(false);

            return restResponse;
        }
    }
}