using System.Collections.Generic;

namespace DependencyResolver.Application.Interfaces
{
    public interface IFileHandler
    {
        IAsyncEnumerable<string> GetLinesAsync(string path);
    }
}
