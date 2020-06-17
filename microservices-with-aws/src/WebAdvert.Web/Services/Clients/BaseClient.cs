using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebAdvert.Web.Services.Clients
{
    public class BaseClient
    {
        protected readonly HttpClient _client;

        public BaseClient(HttpClient client)
        {
            _client = client;
            Initialize(_client);
        }

        #region api-invocation methods

        /// <summary>
        /// For getting data from a web api using GET
        /// </summary>
        /// <param name="apiUrl">the full url of the api get method</param>
        /// <returns>The item requested</returns>
        public async Task<TOutput> GetAsync<TOutput>(string apiUrl)
        {
            var result = default(TOutput);

            var response = await _client.GetAsync(apiUrl).ConfigureAwait(false);

            // send response back as-is if input type is HttpResponseMessage
            if (typeof(TOutput) == typeof(HttpResponseMessage))
            {
                return (TOutput)Convert.ChangeType(response, typeof(TOutput));
            }

            await response.Content.ReadAsStringAsync().ContinueWith(x =>
            {
                if (x.IsFaulted)
                    throw x.Exception;

                result = JsonSerializer.Deserialize<TOutput>(x.Result);
            });

            return result;
        }

        /// <summary>
        /// For creating a new item over a web api using POST
        /// </summary>
        /// <param name="apiUrl">the full url of the api post method</param>
        /// <param name="postObject">The object to be created</param>
        /// <returns>The item created</returns>
        public async Task<TOutput> PostAsync<TOutput, TInput>(string apiUrl, TInput postObject)
        {
            var result = default(TOutput);

            var content = new StringContent(JsonSerializer.Serialize(postObject), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(apiUrl, content).ConfigureAwait(false);

            // send response back as-is if input type is HttpResponseMessage
            if (typeof(TOutput) == typeof(HttpResponseMessage))
            {
                return (TOutput)Convert.ChangeType(response, typeof(TOutput));
            }

            await response.Content.ReadAsStringAsync().ContinueWith(x =>
            {
                if (x.IsFaulted)
                    throw x.Exception;

                result = JsonSerializer.Deserialize<TOutput>(x.Result);
            });

            return result;
        }

        /// <summary>
        /// For updating an existing item over a web api using PUT
        /// </summary>
        /// <param name="apiUrl">the full url of the api put method</param>
        /// <param name="putObject">The object to be edited</param>
        public async Task<TOutput> PutAsync<TOutput, TInput>(string apiUrl, TInput putObject)
        {
            var result = default(TOutput);

            var content = new StringContent(JsonSerializer.Serialize(putObject), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(apiUrl, content).ConfigureAwait(false);

            // send response back as-is if input type is HttpResponseMessage
            if (typeof(TOutput) == typeof(HttpResponseMessage))
            {
                return (TOutput)Convert.ChangeType(response, typeof(TOutput));
            }

            await response.Content.ReadAsStringAsync().ContinueWith(x =>
            {
                if (x.IsFaulted)
                    throw x.Exception;

                result = JsonSerializer.Deserialize<TOutput>(x.Result);
            });

            return result;
        }

        /// <summary>
        /// For deleting an existing item over a web api using DELETE
        /// </summary>
        /// <param name="apiUrl">the full url of the api delete method</param>
        public async Task<HttpResponseMessage> DeleteAsync(string apiUrl)
        {
            var response = await _client.DeleteAsync(apiUrl).ConfigureAwait(false);

            return response;
        }

        #endregion API Invocation Methods

        #region Private Methods

        /// <summary>
        /// Used to setup the client
        /// </summary>
        /// <param name="client">The HttpClient we are configuring</param>
        /// <param name="authKey">the authorization key to be used to authenticate & authorize caller</param>
        /// <param name="timeOut">the timeout for the request in milliseconds</param>
        /// <param name="additionalheaders">any additional header values for the request </param>
        private static void Initialize(HttpClient client, string authKey = null, int timeOut = 0, List<KeyValuePair<string, string>> additionalheaders = null)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(authKey))
            {
                client.DefaultRequestHeaders.Add("Authorization", authKey);
            }

            if (timeOut > 0)
            {
                client.Timeout = TimeSpan.FromMilliseconds(timeOut);
            }

            if (additionalheaders != null)
            {
                foreach (KeyValuePair<string, string> item in additionalheaders)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
        }

        #endregion Private Methods
    }
}
