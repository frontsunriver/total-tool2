using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWonder.Helpers.Model
{
    public class Covid19Object
    {
        [JsonProperty("get", NullValueHandling = NullValueHandling.Ignore)]
        public string Get { get; set; }

        [JsonProperty("parameters", NullValueHandling = NullValueHandling.Ignore)]
        public Parameters Parameters { get; set; }

        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Errors { get; set; }

        [JsonProperty("results", NullValueHandling = NullValueHandling.Ignore)]
        public long? Results { get; set; }

        [JsonProperty("response", NullValueHandling = NullValueHandling.Ignore)]
        public List<Response> Response { get; set; }
    }

    public class Parameters
    {
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
    }

    public class Response
    {
        [JsonProperty("continent", NullValueHandling = NullValueHandling.Ignore)]
        public string Continent { get; set; }

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        [JsonProperty("population", NullValueHandling = NullValueHandling.Ignore)]
        public string Population { get; set; }

        [JsonProperty("cases", NullValueHandling = NullValueHandling.Ignore)]
        public Cases Cases { get; set; }

        [JsonProperty("deaths", NullValueHandling = NullValueHandling.Ignore)]
        public Deaths Deaths { get; set; }

        [JsonProperty("tests", NullValueHandling = NullValueHandling.Ignore)]
        public Tests Tests { get; set; }

        [JsonProperty("day", NullValueHandling = NullValueHandling.Ignore)]
        public string Day { get; set; }

        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Time { get; set; }
    }

    public class Cases
    {
        [JsonProperty("new", NullValueHandling = NullValueHandling.Ignore)]
        public string New { get; set; }

        [JsonProperty("active", NullValueHandling = NullValueHandling.Ignore)]
        public string Active { get; set; }

        [JsonProperty("critical", NullValueHandling = NullValueHandling.Ignore)]
        public string Critical { get; set; }

        [JsonProperty("recovered", NullValueHandling = NullValueHandling.Ignore)]
        public string Recovered { get; set; }

        [JsonProperty("1M_pop", NullValueHandling = NullValueHandling.Ignore)] 
        public string The1MPop { get; set; }

        [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
        public string Total { get; set; }
    }

    public class Deaths
    {
        [JsonProperty("new", NullValueHandling = NullValueHandling.Ignore)]
        public string New { get; set; }

        [JsonProperty("1M_pop", NullValueHandling = NullValueHandling.Ignore)] 
        public string The1MPop { get; set; }

        [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
        public string Total { get; set; }
    }

    public class Tests
    {
        [JsonProperty("1M_pop", NullValueHandling = NullValueHandling.Ignore)] 
        public string The1MPop { get; set; }

        [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
        public string Total { get; set; }
    }

    public class ErrorCovid19Object
    {
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }

}