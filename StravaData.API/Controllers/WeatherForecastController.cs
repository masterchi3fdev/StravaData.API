using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StravaData.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("test")]
        [Authorize]
        public IActionResult Test()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            System.Diagnostics.Debug.WriteLine($"User identity: {User.Identity?.IsAuthenticated}");
            System.Diagnostics.Debug.WriteLine($"Claims count: {claims.Count}");
            foreach (var claim in claims)
            {
                System.Diagnostics.Debug.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            }

            return Ok(new
            {
                isAuthenticated = User.Identity?.IsAuthenticated,
                claimsCount = claims.Count,
                claims
            });
        }
    }
}
