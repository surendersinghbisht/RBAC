using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mscc.GenerativeAI;

namespace RBAC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiController : ControllerBase
    {
        private readonly GoogleAI _googleAI;

        public GeminiController(GoogleAI googleAI)
        {
            _googleAI = googleAI;
        }

        [HttpGet("generate")]
        public async IAsyncEnumerable<string> GenerateContentStream(string prompt)
        {
            // Specify the model to use
            var model = _googleAI.GenerativeModel(model: "gemini-1.5-flash");

            // Use the streaming method
            var responseStream = model.GenerateContentStream(prompt);

            await foreach (var chunk in responseStream)
            {
                // Send each chunk of text back to the client
                yield return chunk.Text;
            }
        }
    }
    
}
