using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Globalization;
using IS455Project.API;
using IS455Project.API.Services;


namespace RecommendationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private const string contentFilteringCsv = "/path/to/content_filtering_results.csv";
        private string collaborativeFilteringCsv = Path.Combine("collab_contentId.csv");

        // Get content recommendations from the CSV
        // [HttpGet("getContentRecommendations/{contentId}")]
        // public IActionResult GetContentRecommendations(string contentId)
        // {
        //     var recommendations = GetRecommendationsFromCsv(contentFilteringCsv, contentId);
        //     return Ok(recommendations);
        // }

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
                // Read records using the CsvRecord class
                var records = csv.GetRecords<CsvRecord>().ToList();

                // Loop through the records to find the ones with the matching contentId
                foreach (var record in records)
                {
                    if (record.ContentId == contentId)
                    {
                        recommendations.Add(record.Recommendation1); // Add the appropriate recommendation
                        recommendations.Add(record.Recommendation2); // Repeat for other recommendations if needed
                        recommendations.Add(record.Recommendation3);
                        recommendations.Add(record.Recommendation4);
                        recommendations.Add(record.Recommendation5);
                    }
                }
            }

            return recommendations;
        }
        [HttpGet("getContentRecommendations/{contentId}")]
        public IActionResult GetRecommendations(string contentId, int topN = 5)
        {
            try
            {
                string csvPath = Path.Combine("content_filtering_results.csv");


                var recommendations = TFIDFRecommender.GetTopSimilarContent(csvPath, contentId, topN);

                return Ok(recommendations.Select(r => new
                {
                    contentId = r.ContentId,
                    score = r.Score
                }));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }
        [HttpGet("all-content-ids")]
        public IActionResult GetAllContentIds()
        {
            try
            {
                var contentIds = System.IO.File.ReadAllLines(collaborativeFilteringCsv)
                    .Skip(1) // Skip header
                    .Select(line => line.Split(',')[0]) // Assuming 'contentId' is in the first column
                    .Distinct()
                    .ToList();

                return Ok(contentIds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to read content IDs", detail = ex.Message });
            }
        }
    }
}
        