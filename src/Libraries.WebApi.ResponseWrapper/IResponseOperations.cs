using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Libraries.WebApi.ResponseWrapper
{
    public interface IResponseOperations
    {
        Task<string> ReadResponseBodyAsync(Stream stream);
        Task RevertResponseBodyStreamAsync(Stream originalStream, Stream currentStream);
        Task HandleSuccessfulResponseAsync(HttpContext context, string bodyAsText);
        Task HandleUnsuccessfulResponseAsync(HttpContext context, string bodyAsText);
        Task HandleExceptionResponseAsync(HttpContext context, Exception ex);
    }
}
