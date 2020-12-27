using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using CommandLine;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandOptions>(args)
                .WithParsed(StartWatching)
                .WithNotParsed(a =>
                {
                    Environment.Exit(1);
                });
        }

        private static void StartWatching(CommandOptions options)
        {
            var files = GetMatchingFiles(options);
            var file = "";
            var watcher = new FileSystemWatcher
            {
                Path = options.SourceDirectoryPath,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                Filter = file
            };
            watcher.Changed += (sender, args) => Console.WriteLine("File has changed");
            watcher.Renamed += (sender, args) => Console.WriteLine("File has renamed");
            watcher.EnableRaisingEvents = true;
            Console.WriteLine(files.Select(p => p.Path).Aggregate((a, b) => a + ", " + b));
        }

        private static IEnumerable<FilePatternMatch> GetMatchingFiles(CommandOptions options)
        {
            Matcher matcher = new Matcher(StringComparison.InvariantCulture);
            matcher.AddInclude(options.FileGlobPattern);
            var directoryInfo = new DirectoryInfo(string.IsNullOrWhiteSpace(options.SourceDirectoryPath) ? Directory.GetCurrentDirectory() : options.SourceDirectoryPath);
            var directoryInfoWrapper = new DirectoryInfoWrapper(directoryInfo);
            var files = matcher.Execute(directoryInfoWrapper).Files;
            return files;
        }
    }
}
