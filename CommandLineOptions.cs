using CommandLine;

namespace JLCSolutionsCR
{
    public class CommandLineOptions
    {
        [Value(index: 0, Required = true, HelpText = "Company Id")]
        public int CompanyId { get; set; }
        
        [Value(index: 1, Required = true, HelpText = "Branch Id")]
        public int BranchId { get; set; }
        
        [Value(index: 2, Required = true, HelpText = "Delay in seconds")]
        public int DelaySeconds { get; set; }
        
        [Value(index: 3, Required = true, HelpText = "Number of printer characters")]
        public int ChartCount { get; set; }
        
        [Value(index: 4, Required = true, HelpText = "Filter for department name")]
        public string Filter { get; set; }
        
        [Value(index: 5, Required = true, HelpText = "Installed printer name")]
        public string PrinterName { get; set; }
    }
}