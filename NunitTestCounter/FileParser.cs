using System.Collections.Generic;
using System.IO;

namespace NunitTestCounter
{
    public static class FileParser
    {
        public static IEnumerable<string> ParseToList(string filePath)
        {
            var result = new List<string>();
            var file = new StreamReader(filePath);
             
            string line;
            while ((line = file.ReadLine()) != null)
            {
                result.Add(line);
            }

            file.Close();
            return result;
        } 
    }
}
