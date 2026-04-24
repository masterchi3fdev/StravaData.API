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
        public IActionResult Test(ILogger<WeatherForecastController> logger)
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            logger.LogInformation($"User authenticated: {User.Identity?.IsAuthenticated}");
            logger.LogInformation($"Claims count: {claims.Count}");

            foreach (var claim in claims)
            {
                logger.LogInformation($"Claim: {claim.Type} = {claim.Value}");
            }

            return Ok(new
            {
                isAuthenticated = User.Identity?.IsAuthenticated,
                claims
            });
        }

        [HttpGet("test-no-auth")]
        [AllowAnonymous]
        public IActionResult TestNoAuth(ILogger<WeatherForecastController> logger)
        {
            logger.LogInformation("This endpoint has no auth");
            return Ok(new { message = "No auth required" });
        }
    }
}
