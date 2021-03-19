using System;
using System.Collections.Generic;

namespace Libraries.WebApi.ResponseWrapper.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(List<string> errors) : base("Validation errors occured")
        {
            Errors = errors;
        }
        public List<string> Errors { get; set; }
    }
}
