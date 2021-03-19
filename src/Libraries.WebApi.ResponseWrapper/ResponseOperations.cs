using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Libraries.WebApi.ResponseWrapper.Exceptions;
using Libraries.WebApi.ResponseWrapper.Models;
using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.WebApi.ResponseWrapper
{
    public class ResponseOperations : IResponseOperations
    {
        public async Task HandleSuccessfulResponseAsync(HttpContext context, string bodyAsText)
        {
            var dynamicBody = JsonConvert.DeserializeObject<dynamic>(bodyAsText);

            var apiResponse = ApiResponse<object>.WithSuccess(dynamicBody, context.Response.StatusCode, ResponseMessages.RequestSuccessful);

            var apiResponseJson = JsonConvert.SerializeObject(apiResponse);

            await WriteResponseAsync(context, apiResponseJson);
        }

        public async Task HandleUnsuccessfulResponseAsync(HttpContext context, string bodyAsText)
        {

            var apiError = string.IsNullOrEmpty(bodyAsText) ? GetApiErrorByCode(context.Response.StatusCode) : new ApiError(bodyAsText);

            var apiResponse = ApiResponse.WithError(apiError, context.Response.StatusCode, ResponseMessages.RequestFailed);
            var apiResponseJson = JsonConvert.SerializeObject(apiResponse);
            await WriteResponseAsync(context, apiResponseJson);
        }

        public async Task<string> ReadResponseBodyAsync(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var textBody = await new StreamReader(stream).ReadToEndAsync();
            stream.Seek(0, SeekOrigin.Begin);
            return textBody;
        }

        public async Task RevertResponseBodyStreamAsync(Stream originalStream, Stream currentStream)
        {
            currentStream.Seek(0, SeekOrigin.Begin);
            await currentStream.CopyToAsync(originalStream);
        }

        public async Task HandleExceptionResponseAsync(HttpContext context, Exception ex)
        {
            ApiError error = ex switch
            {
                ValidationException => new ApiError(ResponseMessages.ValidationError, ((ValidationException)ex).Errors),
                NotFoundException => new ApiError(ResponseMessages.NotFoundError),
                _ => new ApiError(ResponseMessages.InternalServerError)
            };

            var apiResponse = ApiResponse.WithError(error, context.Response.StatusCode, ResponseMessages.RequestFailed);
            var jsonApiResponse = JsonConvert.SerializeObject(apiResponse);
            await WriteResponseAsync(context, jsonApiResponse);
        }

        private async Task WriteResponseAsync(HttpContext context, string responseBody)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.ContentLength = string.IsNullOrEmpty(responseBody) ? 0 : responseBody.Length;
            await context.Response.WriteAsync(responseBody);
        }

        private ApiError GetApiErrorByCode(int httpStatusCode)
        {

            var message = httpStatusCode switch
            {
                StatusCodes.Status400BadRequest => ResponseMessages.NotFoundError,
                StatusCodes.Status401Unauthorized => ResponseMessages.NotAuthorized,
                _ => ""
            };

            return new ApiError(message);
        }
    }
}
