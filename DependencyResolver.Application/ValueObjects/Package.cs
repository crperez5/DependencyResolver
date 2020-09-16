using DependencyResolver.Application.Common;
using DependencyResolver.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DependencyResolver.Application.ValueObjects
{
    internal class Package : ValueObject
    {
        public string Name { get; private set; }

        public string Version { get; private set; }

        public static Package For(string line)
        {
            var package = new Package();

            try
            {
                var regex = new Regex(@"(^[-a-zA-Z0-9]+),([-a-zA-Z0-9]+)$");
                var matches = regex.Matches(line);
                if (matches.Count == 0)
                {
                    throw new InvalidPackageException(line);
                }

                package.Name = matches[0].Groups[1].Value;
                package.Version = matches[0].Groups[2].Value;

            }
            catch (Exception ex)
            {
                throw new InvalidPackageException(line, ex);
            }

            return package;
        }

        public static implicit operator string(Package package)
        {
            return package.ToString();
        }

        public static explicit operator Package(string line)
        {
            return For(line);
        }

        public override string ToString()
        {
            return $"{Name},{Version}";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Version;
        }
    }
}
