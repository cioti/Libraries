using Newtonsoft.Json;
using System;

namespace Libraries.WebApi.ResponseWrapper.Models
{
    public class ApiResponse<T> : ApiResponse
    {
        private ApiResponse(T result, int statusCode = 200, string message = "", string apiVersion = "") : base(statusCode, message, apiVersion)
        {
            Result = result;
        }
        public T Result { get; set; }

        public static ApiResponse<T> WithSuccess(T result, int statusCode = 200, string message = "", string apiVersion = "")
            => new ApiResponse<T>(result, statusCode, message, apiVersion);
    }


    public class ApiResponse
    {
        protected ApiResponse(int statusCode, string message = "", string apiVersion = "")
        {
            StatusCode = statusCode;
            Message = message;
            ApiVersion = apiVersion;
            Error = null;
        }

        private ApiResponse(ApiError error, int statusCode, string message = "", string apiVersion = "")
        {
            Error = error;
            StatusCode = statusCode;
            Message = message;
            ApiVersion = apiVersion;
        }

        public string ApiVersion { get; set; }

        public string Message { get; set; }

        public int StatusCode { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ApiError Error { get; set; }

        public static ApiResponse WithError(ApiError error, int statusCode, string message = "", string apiVersion = "")
           => new ApiResponse(error, statusCode, message, apiVersion);
    }
}
