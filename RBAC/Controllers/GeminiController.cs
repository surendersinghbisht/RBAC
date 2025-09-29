using System.Collections.Generic;
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
            // The previous model might not have been available to your project.
            // Using a model with broader availability to ensure the request is successful.
            var model = _googleAI.GenerativeModel(model: "gemini-2.5-flash-preview-05-20");

            // Use the streaming method to get a real-time response.
            var responseStream = model.GenerateContentStream(prompt);

            await foreach (var chunk in responseStream)
            {
                // Send each chunk of text back to the client
                yield return chunk.Text;
            }
        }
    }
}
