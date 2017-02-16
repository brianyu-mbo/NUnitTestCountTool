using CommandLine;

namespace NunitTestCounter
{
    public class CommandLineOptions
    {
        
        [Value(0, 
            HelpText = "This is for the assembly name")]
        public string AssemblyPath { get; set; }
        
        [Value(1, 
            HelpText = "This is for the file of categories")]
        public string FileOfCategories { get; set; }

        [Option('v')]
        public bool Verbose { get; set; }
    }
}