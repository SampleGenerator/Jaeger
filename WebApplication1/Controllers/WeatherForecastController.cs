using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get()
    {
        await Task.Delay(1000);

        using var httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5140"),
        };

        _logger.LogInformation("Getting wether forecast objects.");
        var rs = await httpClient.GetStringAsync("/WeatherForecast");
        var result = JsonConvert.DeserializeObject<WeatherForecast[]>(rs);

        return Ok(result);
    }
}