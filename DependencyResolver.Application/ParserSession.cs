using DependencyResolver.Application.Enums;
using DependencyResolver.Application.Interfaces;
using DependencyResolver.Application.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace DependencyResolver.Application
{
    internal class ParserSession : IParserSession
    {
        private int _pendingPackagesCount;
        private int _pendingDependenciesCount;

        private readonly HashSet<Package> _packages = new HashSet<Package>();
        private readonly HashSet<Dependency> _orphans = new HashSet<Dependency>();

        public ParserSession()
        {
            Status = ParseStatus.ParsingPackagesCount;
        }

        public ParseStatus Status { get; private set; }

        public bool HasErrors { get; private set; }

        public void Fail()
        {
            HasErrors = true;
        }

        public void SetPackagesCount(int count)
        {
            _pendingPackagesCount = count;
            Status = ParseStatus.ParsingPackages;
        }

        public void AddPackage(Package package)
        {
            if (IsVersionMismatch(package))
            {
                HasErrors = true;
                return;
            }

            _packages.Add(package);

            _pendingPackagesCount -= 1;

            if (_pendingPackagesCount == 0)
            {
                Status = ParseStatus.ParsingDependenciesCount;
            }
        }

        public void AddDependency(Dependency dependency)
        {
            if (_packages.Contains(dependency.PackageFrom))
            {
                _packages.Add(dependency.PackageTo);
            }
            else
            {
                _orphans.Add(dependency);
            }

            _pendingDependenciesCount -= 1;

            if (_pendingDependenciesCount == 0)
            {
                AddOrphans();

                if (HasDuplicatedPackages())
                {
                    HasErrors = true;
                }

                Status = ParseStatus.Completed;
            }
        }

        public void SetDependenciesCount(int count)
        {
            _pendingDependenciesCount = count;
            Status = ParseStatus.ParsingDependencies;
        }

        private void AddOrphans(Queue<Dependency> pending = null)
        {
            if (pending == null)
            {
                pending = new Queue<Dependency>(_orphans);
            }

            if (pending.Count == 0)
            {
                return;
            }

            var current = pending.Dequeue();

            if (_packages.Contains(current.PackageFrom))
            {
                _packages.Add(current.PackageTo);
            }
            else if (_orphans.Any(o => o.PackageTo.Equals(current.PackageFrom)))
            {
                pending.Enqueue(current);
            }

            AddOrphans(pending);
        }

        private bool HasDuplicatedPackages()
        {
            return _packages.GroupBy(p => p.Name).Where(g => g.Count() > 1).Any();
        }

        private bool IsVersionMismatch(Package package)
        {
            return _packages.Any(p => p.Name == package.Name && p.Version != package.Version);
        }
    }
}
