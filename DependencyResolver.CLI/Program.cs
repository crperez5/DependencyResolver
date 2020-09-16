using DependencyResolver.Application.Interfaces;
using DependencyResolver.Application.Parsers;
using DependencyResolver.Infrastructure.Files;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DependencyResolver.CLI
{
    class Program
    {
        private static ServiceProvider _serviceProvider;

        static async Task<int> Main(string[] args)
        {
            string[] arguments = Environment.GetCommandLineArgs();

            if (arguments.Length < 3 || arguments[1] != "-f")
            {
                Console.WriteLine("Missing arguments. Example usage: DependencyResolver.CLI.exe -f <path to config file>");
                return 1;
            }
            
            RegisterServices();

            IServiceScope scope = _serviceProvider.CreateScope();

            try
            {
                var result = await scope.ServiceProvider.GetRequiredService<IFileParser>().ParseAsync(arguments[2]);
                

                if (result.HasErrors)
                {
                    return 1;
                }

                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
            finally
            {
                DisposeServices();
            }
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IFileHandler, FileHandler>();
            services.AddSingleton<IFileParser, FileParser>();
            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
