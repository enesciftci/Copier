using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Copier help");
            }
            else
            {
                Matcher matcher = new Matcher(StringComparison.InvariantCulture);
                matcher.AddInclude(args[1]);
                var directoryInfo = new DirectoryInfo(args[0]);
                var directoryWrapper = new DirectoryInfoWrapper(directoryInfo);
                var files = matcher.Execute(directoryWrapper).Files;

                Console.WriteLine(files.Select(p => p.Path).Aggregate((a, b) => a + ", " + b));
                Console.ReadKey();
            }

        }
    }
}
