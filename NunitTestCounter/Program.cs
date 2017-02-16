using System;
using CommandLine;

namespace NunitTestCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(options =>
                {
                    var assemblyPath = options.AssemblyPath;
                    var filePath = options.FileOfCategories;

                    var categoryList = FileParser.ParseToList(filePath);

                    var nunitTestCounter = new NUnitTestCounter(assemblyPath, options.Verbose);

                    var count = nunitTestCounter.GetNumTestsInCategories(categoryList);
                    Console.WriteLine($"\n\nTotal amount of tests found: {count}.");
                });

                //todo do with not parsed
        }
    }
}
