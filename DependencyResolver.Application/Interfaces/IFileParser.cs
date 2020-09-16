using System.Threading.Tasks;

namespace DependencyResolver.Application.Interfaces
{
    public interface IFileParser
    {
        Task<IParserSession> ParseAsync(string path);
    }
}
