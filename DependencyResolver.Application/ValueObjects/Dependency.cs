using DependencyResolver.Application.Common;
using DependencyResolver.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DependencyResolver.Application.ValueObjects
{
    class Dependency : ValueObject
    {
        public Package PackageFrom { get; private set; }

        public Package PackageTo { get; private set; }

        public static Dependency For(string line)
        {
            var dependency = new Dependency();

            try
            {
                var regex = new Regex(@"(^[-a-zA-Z0-9]+,[-a-zA-Z0-9]+),([-a-zA-Z0-9]+,[-a-zA-Z0-9]+)$");
                var matches = regex.Matches(line);
                if (matches.Count == 0)
                {
                    throw new InvalidDependencyException(line);
                }

                dependency.PackageFrom = Package.For(matches[0].Groups[1].Value);
                dependency.PackageTo = Package.For(matches[0].Groups[2].Value);

            }
            catch (Exception ex)
            {
                throw new InvalidDependencyException(line, ex);
            }

            return dependency;
        }

        public static implicit operator string(Dependency dependency)
        {
            return dependency.ToString();
        }

        public static explicit operator Dependency(string line)
        {
            return For(line);
        }

        public override string ToString()
        {
            return $"{PackageFrom},{PackageTo}";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PackageFrom;
            yield return PackageTo;
        }
    }
}
