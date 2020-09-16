using DependencyResolver.Application.Enums;
using DependencyResolver.Application.Exceptions;
using DependencyResolver.Application.Interfaces;
using DependencyResolver.Application.ValueObjects;
using System;
using System.Threading.Tasks;

namespace DependencyResolver.Application.Parsers
{
    public class FileParser : IFileParser
    {
        private readonly IFileHandler _fileHandler;
        private readonly ParserSession _session;

        public FileParser(IFileHandler fileHandler)
        {
            this._fileHandler = fileHandler;
            _session = new ParserSession();
        }

        public async Task<IParserSession> ParseAsync(string path)
        {
            try
            {
                await foreach (var line in _fileHandler.GetLinesAsync(path))
                {
                    ParseLine(line);

                    if (_session.HasErrors)
                    {
                        return _session;
                    }
                }

                return _session;
            }
            catch (Exception)
            {
                _session.Fail();
                return _session;
            }
        }

        private void ParseLine(string line)
        {
            Action<string> parse = _session.Status switch
            {
                ParseStatus.ParsingPackagesCount => (line) => _session.SetPackagesCount(GetCount(line)),
                ParseStatus.ParsingDependenciesCount => (line) => _session.SetDependenciesCount(GetCount(line)),
                ParseStatus.ParsingPackages => (line) => _session.AddPackage(Package.For(line)),
                ParseStatus.ParsingDependencies => (line) => _session.AddDependency(Dependency.For(line)),
                _ => throw new NotImplementedException()
            };

            parse(line);
        }

        private int GetCount(string line)
        {
            if (!int.TryParse(line, out int result))
            {
                throw new InvalidCountException(line);
            }
            return result;
        }
    }
}
