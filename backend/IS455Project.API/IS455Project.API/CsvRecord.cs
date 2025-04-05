using CsvHelper.Configuration.Attributes;

namespace IS455Project.API
{
    public class CsvRecord
    {
        [Name("contentId")]
        public string ContentId { get; set; }
        
        [Name("Recommendation 1")]
        public string Recommendation1 { get; set; }
        
        [Name("Recommendation 2")]
        public string Recommendation2 { get; set; }
        
        [Name("Recommendation 3")]
        public string Recommendation3 { get; set; }
        
        [Name("Recommendation 4")]
        public string Recommendation4 { get; set; }
        
        [Name("Recommendation 5")]
        public string Recommendation5 { get; set; }
    }
}