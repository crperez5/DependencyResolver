using System;

namespace DependencyResolver.Application.Exceptions
{
    public class InvalidPackageException : Exception
    {
        public InvalidPackageException(string package)
            : base($"Package \"{package}\" is invalid.", new Exception())
        {
        }

        public InvalidPackageException(string package, Exception ex)
           : base($"Package \"{package}\" is invalid.", ex)
        {
        }
    }
}
