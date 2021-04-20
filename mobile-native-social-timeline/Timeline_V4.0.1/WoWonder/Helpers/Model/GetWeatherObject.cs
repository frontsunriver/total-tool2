using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWonder.Helpers.Model
{
    public class GetWeatherObject
    {
        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public LocationObject Location { get; set; }

        [JsonProperty("current", NullValueHandling = NullValueHandling.Ignore)]
        public CurrentObject Current { get; set; }

        [JsonProperty("forecast", NullValueHandling = NullValueHandling.Ignore)]
        public ForecastObject Forecast { get; set; }

    }

    public class CurrentObject
    {
        [JsonProperty("last_updated_epoch", NullValueHandling = NullValueHandling.Ignore)]
        public long? LastUpdatedEpoch { get; set; }

        [JsonProperty("last_updated", NullValueHandling = NullValueHandling.Ignore)]
        public string LastUpdated { get; set; }

        [JsonProperty("temp_c", NullValueHandling = NullValueHandling.Ignore)]
        public long? TempC { get; set; }

        [JsonProperty("temp_f", NullValueHandling = NullValueHandling.Ignore)]
        public double? TempF { get; set; }

        [JsonProperty("is_day", NullValueHandling = NullValueHandling.Ignore)]
        public long? IsDay { get; set; }

        [JsonProperty("condition", NullValueHandling = NullValueHandling.Ignore)]
        public ConditionObject Condition { get; set; }

        [JsonProperty("wind_mph", NullValueHandling = NullValueHandling.Ignore)]
        public long? WindMph { get; set; }

        [JsonProperty("wind_kph", NullValueHandling = NullValueHandling.Ignore)]
        public long? WindKph { get; set; }

        [JsonProperty("wind_degree", NullValueHandling = NullValueHandling.Ignore)]
        public long? WindDegree { get; set; }

        [JsonProperty("wind_dir", NullValueHandling = NullValueHandling.Ignore)]
        public string WindDir { get; set; }

        [JsonProperty("pressure_mb", NullValueHandling = NullValueHandling.Ignore)]
        public long? PressureMb { get; set; }

        [JsonProperty("pressure_in", NullValueHandling = NullValueHandling.Ignore)]
        public double? PressureIn { get; set; }

        [JsonProperty("precip_mm", NullValueHandling = NullValueHandling.Ignore)]
        public long? PrecipMm { get; set; }

        [JsonProperty("precip_in", NullValueHandling = NullValueHandling.Ignore)]
        public long? PrecipIn { get; set; }

        [JsonProperty("humidity", NullValueHandling = NullValueHandling.Ignore)]
        public long? Humidity { get; set; }

        [JsonProperty("cloud", NullValueHandling = NullValueHandling.Ignore)]
        public long? Cloud { get; set; }

        [JsonProperty("feelslike_c", NullValueHandling = NullValueHandling.Ignore)]
        public long? FeelslikeC { get; set; }

        [JsonProperty("feelslike_f", NullValueHandling = NullValueHandling.Ignore)]
        public double? FeelslikeF { get; set; }

        [JsonProperty("vis_km", NullValueHandling = NullValueHandling.Ignore)]
        public long? VisKm { get; set; }

        [JsonProperty("vis_miles", NullValueHandling = NullValueHandling.Ignore)]
        public long? VisMiles { get; set; }

        [JsonProperty("uv", NullValueHandling = NullValueHandling.Ignore)]
        public long? Uv { get; set; }

        [JsonProperty("gust_mph", NullValueHandling = NullValueHandling.Ignore)]
        public double? GustMph { get; set; }

        [JsonProperty("gust_kph", NullValueHandling = NullValueHandling.Ignore)]
        public long? GustKph { get; set; }
    }

    public class ConditionObject
    {
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public string Icon { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public long? Code { get; set; }
    }

    public class LocationObject
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("region", NullValueHandling = NullValueHandling.Ignore)]
        public string Region { get; set; }

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        [JsonProperty("lat", NullValueHandling = NullValueHandling.Ignore)]
        public double? Lat { get; set; }

        [JsonProperty("lon", NullValueHandling = NullValueHandling.Ignore)]
        public double? Lon { get; set; }

        [JsonProperty("tz_id", NullValueHandling = NullValueHandling.Ignore)]
        public string TzId { get; set; }

        [JsonProperty("localtime_epoch", NullValueHandling = NullValueHandling.Ignore)]
        public long? LocaltimeEpoch { get; set; }

        [JsonProperty("localtime", NullValueHandling = NullValueHandling.Ignore)]
        public string Localtime { get; set; }
    }

    public class ForecastObject
    {
        [JsonProperty("forecastday", NullValueHandling = NullValueHandling.Ignore)]
        public List<ForecastDay> ForecastDays { get; set; }
    }
     
    public class ForecastDay
    {
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        [JsonProperty("date_epoch", NullValueHandling = NullValueHandling.Ignore)]
        public long? DateEpoch { get; set; }

        [JsonProperty("day", NullValueHandling = NullValueHandling.Ignore)]
        public DayObject Day { get; set; }

        [JsonProperty("astro", NullValueHandling = NullValueHandling.Ignore)]
        public Astro Astro { get; set; }

        [JsonProperty("hour", NullValueHandling = NullValueHandling.Ignore)]
        public List<HourObject> Hour { get; set; }
    }

    public class Astro
    {
        [JsonProperty("sunrise", NullValueHandling = NullValueHandling.Ignore)]
        public string Sunrise { get; set; }

        [JsonProperty("sunset", NullValueHandling = NullValueHandling.Ignore)]
        public string Sunset { get; set; }

        [JsonProperty("moonrise", NullValueHandling = NullValueHandling.Ignore)]
        public string Moonrise { get; set; }

        [JsonProperty("moonset", NullValueHandling = NullValueHandling.Ignore)]
        public string Moonset { get; set; }

        [JsonProperty("moon_phase", NullValueHandling = NullValueHandling.Ignore)]
        public string MoonPhase { get; set; }

        [JsonProperty("moon_illumination", NullValueHandling = NullValueHandling.Ignore)] 
        public long? MoonIllumination { get; set; }
    }

    public class DayObject
    {
        [JsonProperty("maxtemp_c", NullValueHandling = NullValueHandling.Ignore)]
        public double? MaxtempC { get; set; }

        [JsonProperty("maxtemp_f", NullValueHandling = NullValueHandling.Ignore)]
        public double? MaxtempF { get; set; }

        [JsonProperty("mintemp_c", NullValueHandling = NullValueHandling.Ignore)]
        public double? MintempC { get; set; }

        [JsonProperty("mintemp_f", NullValueHandling = NullValueHandling.Ignore)]
        public double? MintempF { get; set; }

        [JsonProperty("avgtemp_c", NullValueHandling = NullValueHandling.Ignore)]
        public double? AvgtempC { get; set; }

        [JsonProperty("avgtemp_f", NullValueHandling = NullValueHandling.Ignore)]
        public double? AvgtempF { get; set; }

        [JsonProperty("maxwind_mph", NullValueHandling = NullValueHandling.Ignore)]
        public double? MaxwindMph { get; set; }

        [JsonProperty("maxwind_kph", NullValueHandling = NullValueHandling.Ignore)]
        public double? MaxwindKph { get; set; }

        [JsonProperty("totalprecip_mm", NullValueHandling = NullValueHandling.Ignore)]
        public double? TotalprecipMm { get; set; }

        [JsonProperty("totalprecip_in", NullValueHandling = NullValueHandling.Ignore)]
        public double? TotalprecipIn { get; set; }

        [JsonProperty("avgvis_km", NullValueHandling = NullValueHandling.Ignore)]
        public double? AvgvisKm { get; set; }

        [JsonProperty("avgvis_miles", NullValueHandling = NullValueHandling.Ignore)]
        public long? AvgvisMiles { get; set; }

        [JsonProperty("avghumidity", NullValueHandling = NullValueHandling.Ignore)]
        public long? Avghumidity { get; set; }

        [JsonProperty("daily_will_it_rain", NullValueHandling = NullValueHandling.Ignore)]
        public long? DailyWillItRain { get; set; }

        [JsonProperty("daily_chance_of_rain", NullValueHandling = NullValueHandling.Ignore)] 
        public long? DailyChanceOfRain { get; set; }

        [JsonProperty("daily_will_it_snow", NullValueHandling = NullValueHandling.Ignore)]
        public long? DailyWillItSnow { get; set; }

        [JsonProperty("daily_chance_of_snow", NullValueHandling = NullValueHandling.Ignore)] 
        public long? DailyChanceOfSnow { get; set; }

        [JsonProperty("condition", NullValueHandling = NullValueHandling.Ignore)]
        public ConditionObject Condition { get; set; }

        [JsonProperty("uv", NullValueHandling = NullValueHandling.Ignore)]
        public long? Uv { get; set; }
    }

    public class HourObject
    {
        [JsonProperty("time_epoch", NullValueHandling = NullValueHandling.Ignore)]
        public long TimeEpoch { get; set; }

        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public string Time { get; set; }

        [JsonProperty("temp_c", NullValueHandling = NullValueHandling.Ignore)]
        public double? TempC { get; set; }

        [JsonProperty("temp_f", NullValueHandling = NullValueHandling.Ignore)]
        public double? TempF { get; set; }

        [JsonProperty("is_day", NullValueHandling = NullValueHandling.Ignore)]
        public long? IsDay { get; set; }

        [JsonProperty("condition", NullValueHandling = NullValueHandling.Ignore)]
        public ConditionObject Condition { get; set; }

        [JsonProperty("wind_mph", NullValueHandling = NullValueHandling.Ignore)]
        public double? WindMph { get; set; }

        [JsonProperty("wind_kph", NullValueHandling = NullValueHandling.Ignore)]
        public double? WindKph { get; set; }

        [JsonProperty("wind_degree", NullValueHandling = NullValueHandling.Ignore)]
        public long? WindDegree { get; set; }

        [JsonProperty("wind_dir", NullValueHandling = NullValueHandling.Ignore)]
        public string WindDir { get; set; }

        [JsonProperty("pressure_mb", NullValueHandling = NullValueHandling.Ignore)]
        public long? PressureMb { get; set; }

        [JsonProperty("pressure_in", NullValueHandling = NullValueHandling.Ignore)]
        public double? PressureIn { get; set; }

        [JsonProperty("precip_mm", NullValueHandling = NullValueHandling.Ignore)]
        public double? PrecipMm { get; set; }

        [JsonProperty("precip_in", NullValueHandling = NullValueHandling.Ignore)]
        public double? PrecipIn { get; set; }

        [JsonProperty("humidity", NullValueHandling = NullValueHandling.Ignore)]
        public long? Humidity { get; set; }

        [JsonProperty("cloud", NullValueHandling = NullValueHandling.Ignore)]
        public long? Cloud { get; set; }

        [JsonProperty("feelslike_c", NullValueHandling = NullValueHandling.Ignore)]
        public double? FeelslikeC { get; set; }

        [JsonProperty("feelslike_f", NullValueHandling = NullValueHandling.Ignore)]
        public double? FeelslikeF { get; set; }

        [JsonProperty("windchill_c", NullValueHandling = NullValueHandling.Ignore)]
        public double? WindchillC { get; set; }

        [JsonProperty("windchill_f", NullValueHandling = NullValueHandling.Ignore)]
        public double? WindchillF { get; set; }

        [JsonProperty("heatindex_c", NullValueHandling = NullValueHandling.Ignore)]
        public double? HeatindexC { get; set; }

        [JsonProperty("heatindex_f", NullValueHandling = NullValueHandling.Ignore)]
        public double? HeatindexF { get; set; }

        [JsonProperty("dewpoint_c", NullValueHandling = NullValueHandling.Ignore)]
        public double? DewpointC { get; set; }

        [JsonProperty("dewpoint_f", NullValueHandling = NullValueHandling.Ignore)]
        public double? DewpointF { get; set; }

        [JsonProperty("will_it_rain", NullValueHandling = NullValueHandling.Ignore)]
        public long? WillItRain { get; set; }

        [JsonProperty("chance_of_rain", NullValueHandling = NullValueHandling.Ignore)] 
        public long? ChanceOfRain { get; set; }

        [JsonProperty("will_it_snow", NullValueHandling = NullValueHandling.Ignore)]
        public long? WillItSnow { get; set; }

        [JsonProperty("chance_of_snow", NullValueHandling = NullValueHandling.Ignore)] 
        public long? ChanceOfSnow { get; set; }

        [JsonProperty("vis_km", NullValueHandling = NullValueHandling.Ignore)]
        public double? VisKm { get; set; }

        [JsonProperty("vis_miles", NullValueHandling = NullValueHandling.Ignore)]
        public long? VisMiles { get; set; }

        [JsonProperty("gust_mph", NullValueHandling = NullValueHandling.Ignore)]
        public double? GustMph { get; set; }

        [JsonProperty("gust_kph", NullValueHandling = NullValueHandling.Ignore)]
        public double? GustKph { get; set; }
    }

}