using System.Collections.Generic;

namespace WebAdvert.App.Models
{
    public class ServiceResponse<T>
    {
        public T Object { get; set; }
        public IList<string> Errors { get; set; }

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
