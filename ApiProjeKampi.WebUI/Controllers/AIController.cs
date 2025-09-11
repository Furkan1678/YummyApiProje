using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ApiProjeKampi.WebUI.Controllers
{
    public class AIController : Controller
    {
        private readonly HttpClient _httpClient;

        public AIController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult CreateRecipeWithGemini()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> CreateRecipeWithGemini(string prompt)
        {
            // Buraya kendi API keyini yaz (daha güvenlisi: environment variable kullan)
            var apiKey = "AIzaSyBCqsyzufUGru5IdK7dJy7cUryX-AhRjoU";

            var requestUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";

            var requestData = new
            {
                contents = new[]
                {
                    new {
                        parts = new[]
                        {
                            new { text = $"Malzemeler: {prompt}. Bu malzemelerle yapılabilecek bir yemek tarifi öner." }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUrl, content);
            var raw = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    using var doc = JsonDocument.Parse(raw);
                    var text = doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

                    ViewBag.recipe = text;
                }
                catch
                {
                    ViewBag.recipe = $"Beklenmeyen yanıt: {raw}";
                }
            }
            else
            {
                ViewBag.recipe = $"Hata: {response.StatusCode} - {raw}";
            }

            return View();
        }



    }
}