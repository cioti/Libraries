using Newtonsoft.Json;
using System.Collections.Generic;

namespace Libraries.WebApi.ResponseWrapper.Models
{
    public class ApiError
    {
        public ApiError(string message, IEnumerable<string> validationErrors) : this(message)
        {
            ValidationErrors = validationErrors;
        }

        public ApiError(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Details { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<string> ValidationErrors { get; set; }
    }
}
