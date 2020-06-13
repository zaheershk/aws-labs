using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace WebAdvert.Models.Shared
{
    public class ServiceResponse<T>
    {
        public bool IsSuccess => Errors == null || (Errors != null && !Errors.Any());

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Object { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Errors { get; set; }

        public ServiceResponse(T responseObject)
        {
            Object = responseObject;
        }

        public ServiceResponse(T responseObject, string errorMessage)
        {
            Object = responseObject;
            Errors = new List<string>
            {
                errorMessage
            };
        }

        public ServiceResponse(T responseObject, IList<string> errors)
        {
            Object = responseObject;
            Errors = errors;
        }
    }
}
