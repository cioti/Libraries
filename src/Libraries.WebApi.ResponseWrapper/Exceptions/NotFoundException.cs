using System;

namespace Libraries.WebApi.ResponseWrapper.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Entity not found") { }
    }
}
