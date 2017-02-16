using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Engine;

namespace NunitTestCounter
{
    public class NUnitTestCounter
    {
        private ITestRunner Runner { get; }
        private bool Verbose { get; }

        /// <summary>
        /// Creates an instance of the NUnitTestCounter given the assembly path
        /// </summary>
        /// <param name="assemblyPath"></param>
        public NUnitTestCounter(string assemblyPath, bool verbose = false)
        {
            var engine = TestEngineActivator.CreateInstance();
            var package = new TestPackage(assemblyPath);
            Runner = engine.GetRunner(package);
            Verbose = verbose;
        }

        /// <summary>
        /// Returns the number of tests in given list of categories
        /// Will print console messages if a specific category yielded no results
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public int GetNumTestsInCategories(IEnumerable<string> categories)
        {
            var result = 0;

            if (categories == null)
                return result;

            var enumerable = categories as IList<string> ?? categories.ToList();

            if (Verbose)
                PrintNumTestsInEveryCategory(enumerable);

            var categoryFilter = ConvertToCategoryFilter(enumerable);
            Console.WriteLine(categoryFilter);
            try
            {
                ITestFilterBuilder filterBuilder = new TestFilterBuilder();
                filterBuilder.SelectWhere($"{categoryFilter}");
                var filter = filterBuilder.GetFilter();
                result = Runner.CountTestCases(filter);
                Console.WriteLine($"\n\n....................................................");
            }
            catch (TestSelectionParserException)
            {
                Console.WriteLine($"Could not parse filter {categoryFilter}");
            }


            return result;
        }

        /// <summary>
        /// Goes through every category and prints out how many tests were found per category
        /// </summary>
        /// <param name="categories"></param>
        private void PrintNumTestsInEveryCategory(IEnumerable<string> categories)
        {
            foreach (var category in categories)
            {
                ITestFilterBuilder filterBuilder = new TestFilterBuilder();
                filterBuilder.SelectWhere($"cat == {category}");
                
                var filter = filterBuilder.GetFilter();
                var count = Runner.CountTestCases(filter);

                Console.WriteLine("\n\n....................................................");
                Console.WriteLine($"Searching for test cases for category \"{category}\".");


                if (count == 0)
                    Console.WriteLine($"Category {category} yielded no results.");
                else
                    Console.WriteLine($"{count} tests found.");
            }
        }

        /// <summary>
        /// Converts an IEnumerable of strings representing categories into a single category filter
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        private string ConvertToCategoryFilter(IEnumerable<string> categories)
        {
            var result = "";

            categories = categories.Select(category => category.Insert(0, "cat == "));
            result = string.Join(" || ", categories.ToArray());

            return result;
        }
    }
}
