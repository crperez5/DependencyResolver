using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.IO;
using System.Reflection;

namespace DependencyResolver.Test
{
    [TestFixture]
    public class ParserTests
    {
        private string _cli;

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            _cli = config["cli"];
        }

        [TestCase("input000", 0)]
        [TestCase("input001", 1)]
        [TestCase("input002", 1)]
        [TestCase("input003", 0)]
        [TestCase("input004", 0)]
        [TestCase("input005", 1)]
        [TestCase("input006", 0)]
        [TestCase("input007", 1)]
        [TestCase("input008", 0)]

        public void IsValidOutput(string input, int expected)
        {
            var process = 
                System.Diagnostics.Process.Start(_cli, 
                $"-f {Path.GetFullPath(Path.Combine(Assembly.GetExecutingAssembly().Location, @"..\"))}testdata\\{input}.txt");

            process.WaitForExit();

            Assert.AreEqual(expected, process.ExitCode);
        }
    }
}