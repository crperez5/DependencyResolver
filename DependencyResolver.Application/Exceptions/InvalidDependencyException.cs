using System;

namespace DependencyResolver.Application.Exceptions
{
    class InvalidDependencyException : Exception
    {
        public InvalidDependencyException(string dependency)
          : base($"Dependency \"{dependency}\" is invalid.", new Exception())
        {
        }

        public InvalidDependencyException(string dependency, Exception ex)
           : base($"Dependency \"{dependency}\" is invalid.", ex)
        {
        }
    }
}
