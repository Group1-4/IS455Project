using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Globalization;
using System.IO;

namespace RecommendationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private const string contentFilteringCsv = "/path/to/content_filtering_results.csv";
        private const string collaborativeFilteringCsv = "/path/to/collaborative_filtering_results.csv";

        // Get content recommendations from the CSV
        [HttpGet("getContentRecommendations/{contentId}")]
        public IActionResult GetContentRecommendations(string contentId)
        {
            var recommendations = GetRecommendationsFromCsv(contentFilteringCsv, contentId);
            return Ok(recommendations);
        }

        // Get collaborative recommendations from the CSV
        [HttpGet("getCollaborativeRecommendations/{contentId}")]
        public IActionResult GetCollaborativeRecommendations(string contentId)
        {
            var recommendations = GetRecommendationsFromCsv(collaborativeFilteringCsv, contentId);
            return Ok(recommendations);
        }

        // Helper method to get recommendations from a CSV file
        private List<string> GetRecommendationsFromCsv(string filePath, string contentId)
        {
            var recommendations = new List<string>();

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>().ToList();

                foreach (var record in records)
                {
                    if (record.ContentId == contentId)
                    {
                        recommendations.Add(record.RecommendedItem.ToString());
                    }
                }
            }

            return recommendations;
        }
    }
}
        