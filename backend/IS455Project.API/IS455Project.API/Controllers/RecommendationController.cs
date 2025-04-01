using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecommendationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public RecommendationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET api/recommendation/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecommendations(string id)
        {
            try
            {
                // Step 1: Call Python Collaborative Filtering model
                var collaborativeResponse = await _httpClient.GetStringAsync($"http://localhost:5000/collaborative/{id}");
                var collaborativeRecommendations = JsonConvert.DeserializeObject<int[]>(collaborativeResponse);

                // Step 2: Call Python Content Filtering model
                var contentResponse = await _httpClient.GetStringAsync($"http://localhost:5000/content/{id}");
                var contentRecommendations = JsonConvert.DeserializeObject<int[]>(contentResponse);

                // Step 3: Call Azure ML endpoint (replace URL with your Azure ML endpoint)
                var azureResponse = await _httpClient.GetStringAsync($"https://<azure-endpoint>/predict/{id}");
                var azureRecommendations = JsonConvert.DeserializeObject<int[]>(azureResponse);

                // Return recommendations in a structured JSON format
                return Ok(new
                {
                    collaborative = collaborativeRecommendations,
                    contentBased = contentRecommendations,
                    azure = azureRecommendations
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
