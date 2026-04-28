using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StravaData.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet(Name = "GetWeatherForecast")]
        [Authorize]
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
        public IActionResult Test(ILogger<WeatherForecastController> logger)
        {
            logger.LogInformation($"User.Identity.IsAuthenticated: {User.Identity?.IsAuthenticated}");
            logger.LogInformation($"User.Identity.AuthenticationType: {User.Identity?.AuthenticationType}");
            logger.LogInformation($"Claims count: {User.Claims.Count()}");

            foreach (var claim in User.Claims.Take(5))
            {
                logger.LogInformation($"Claim: {claim.Type} = {claim.Value}");
            }

            return Ok();
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
