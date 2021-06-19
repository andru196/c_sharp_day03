using System;
using System.Text.Json.Serialization;
using System.Globalization;

namespace d03.Nasa.NewWs.Models
{
    public record AsteroidInfo
    {
        [JsonPropertyName("id")]
        public string Id {get; init;}
        [JsonIgnore]
        public double Kilometers {
            get
            {
                
                return double.Parse(close_approach_data[0].miss_distance.kilometers, _formatter);
            }
        }
        static private IFormatProvider _formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
        public CloseApproachData[] close_approach_data {get; init;}
        public class CloseApproachData
        {
            public MissDistance miss_distance {get; set;}
            public class MissDistance
            {
                public string kilometers {get; set;}
            }
        }
    }
}