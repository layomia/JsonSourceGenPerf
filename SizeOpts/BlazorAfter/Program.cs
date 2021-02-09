using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BlazorAfter;
using BlazorAfter.Pages;
using BlazorAfter.Shared;
using BlazorAfter.JsonSourceGeneration;

[assembly: JsonSerializable(typeof(Counter))]
[assembly: JsonSerializable(typeof(FetchData))]
[assembly: JsonSerializable(typeof(BlazorAfter.Pages.Index))]
[assembly: JsonSerializable(typeof(MainLayout))]
[assembly: JsonSerializable(typeof(NavMenu))]
[assembly: JsonSerializable(typeof(SurveyPrompt))]
[assembly: JsonSerializable(typeof(WeatherForecast[]))]

namespace BlazorAfter
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
