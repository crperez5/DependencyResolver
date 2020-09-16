using DependencyResolver.Application.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace DependencyResolver.Infrastructure.Files
{
    public class FileHandler : IFileHandler
    {


        public async IAsyncEnumerable<string> GetLinesAsync(string path)
        {
            using var file = new StreamReader(path);

            string ln;

            while ((ln = await file.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(ln))
                {
                    continue;
                }

                yield return ln;
            }
            file.Close();

        }
    }
}
