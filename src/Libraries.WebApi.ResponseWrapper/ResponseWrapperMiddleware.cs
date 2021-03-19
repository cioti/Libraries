using Microsoft.AspNetCore.Http;
using Libraries.WebApi.ResponseWrapper.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace Libraries.WebApi.ResponseWrapper
{
    public class ResponseWrapperMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IResponseOperations _responseOperations;

        public ResponseWrapperMiddleware(RequestDelegate next, IResponseOperations responseOperations)
        {
            _next = next;
            _responseOperations = responseOperations;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            Stream originalBodyStream = context.Response.Body;

            using var memoryStream = new MemoryStream();

            context.Response.Body = memoryStream;

            try
            {
                await _next(context);
                context.Response.Body = originalBodyStream;

                if (context.IsWrapSkipped())
                {
                    return;
                }

                if (context.IsWrapIgnored())
                {
                    await _responseOperations.RevertResponseBodyStreamAsync(originalBodyStream, memoryStream);
                    return;
                }

                var bodyAsText = await _responseOperations.ReadResponseBodyAsync(memoryStream);

                if (context.IsSuccess())
                {
                    await _responseOperations.HandleSuccessfulResponseAsync(context, bodyAsText);
                }
                else
                {
                    await _responseOperations.HandleUnsuccessfulResponseAsync(context, bodyAsText);
                }
            }
            catch (Exception ex)
            {
                await _responseOperations.HandleExceptionResponseAsync(context, ex);
                await _responseOperations.RevertResponseBodyStreamAsync(originalBodyStream, memoryStream);
            }
        }
    }
}
