using System;
using System.Collections.Generic;
using MediatR;

namespace ConnectR.MvcCore.Example
{
    public class WeatherForecastRequest : IRequest<WeatherForecastResponse>, IRequest
    {

    }

    public class WeatherForecastResponse
    {
        public IEnumerable<WeatherForecast> WeatherForecasts { get; set; }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
