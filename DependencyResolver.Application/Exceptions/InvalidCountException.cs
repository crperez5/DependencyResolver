using System;

namespace DependencyResolver.Application.Exceptions
{
    public class InvalidCountException : Exception
    {
        public InvalidCountException(string line)
           : base($"Expecting a number but found \"{line}\".", new Exception())
        {
        }
    }
}
