using Startup;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Converters;
using System.Text.Json.Serialization.Metadata;

namespace Startup.JsonSourceGeneration
{
    internal partial class JsonContext : JsonSerializerContext
    {
        public override JsonClassInfo GetJsonClassInfo(System.Type type)
        {
            if (type == typeof(Startup.WeatherForecast[]))
            {
                return this.WeatherForecastArray;
            }

            if (type == typeof(Startup.WeatherForecast))
            {
                return this.WeatherForecast;
            }

            if (type == typeof(System.DateTime))
            {
                return this.DateTime;
            }

            if (type == typeof(System.Int32))
            {
                return this.Int32;
            }

            if (type == typeof(System.String))
            {
                return this.String;
            }

            return null!;
        }
    }
}
