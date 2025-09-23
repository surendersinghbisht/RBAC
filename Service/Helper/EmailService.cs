using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.Helper
{

    public class EmailService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;

        public EmailService(IConfiguration config)
        {
            _config = config;
            _client = new HttpClient();
        }

        public async Task<bool> SendTwoFactorCodeEmailAsync(string userEmail, string code)
        {
            if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(code))
            {
                Console.WriteLine("User email or code is empty.");
                return false;
            }

            var request = new
            {
                service_id = _config["EmailJs:ServiceId"],
                template_id = _config["EmailJs:TemplateId"],
                user_id = _config["EmailJs:UserId"],
                template_params = new
                {
                    to_email = userEmail,
                    user_email = userEmail,  
                    passcode = code
                }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
            );

            HttpResponseMessage? response = null;

            try
            {
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Add("origin", "http://localhost"); 

                response = await _client.PostAsync("https://api.emailjs.com/api/v1.0/email/send", content);

                if (!response.IsSuccessStatusCode)
                {
                    var respBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"EmailJS Error: {(int)response.StatusCode} - {respBody}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception sending email: {ex.Message}");
                return false;
            }
        }

    }
}
