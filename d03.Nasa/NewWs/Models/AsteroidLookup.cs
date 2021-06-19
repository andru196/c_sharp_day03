using System;
using System.Text.Json.Serialization;

namespace d03.Nasa.NewWs.Models
{
    public class AsteroidLookup
    {
        [JsonPropertyName("id")]
        public string Id {get; init;}
        [JsonPropertyName("Kilometers")]
        public double Kilometers {get; init;}
        [JsonPropertyName("name")]
        public string Name {get; set;}
        [JsonPropertyName("nasa_jpl_url")]
        public string NasaUrl {get; set;}
        [JsonPropertyName("is_potentially_hazardous_asteroid")]
        public bool IsPotentiallyHazardousAsteroid {get; set;}

        [JsonPropertyName("orbital_data")]
        public _OrbitalData OrbitalData {get; set;}
        public class _OrbitalData
        {
            [JsonPropertyName("orbit_class")]

            public _OrbitClass OrbitClass {get; set;}
            public class _OrbitClass
            {
                [JsonPropertyName("orbit_class_description")]
                public string OrbitClassDescription {get; set;}
                [JsonPropertyName("orbit_class_type")]
                [JsonConverter(typeof(JsonStringEnumConverter))]
                public _OrbitClassType OrbitClassType {get; set;}
                public enum _OrbitClassType
                {
                    ATE,
                    APO,
                    AMO
                }
            }

        }

        public override string ToString() =>
        $"Asteroid {Name}, SPK-ID: {Id}\n" + (IsPotentiallyHazardousAsteroid ?
        $"IS POTENTIALLY HAZARDOUS!\n" : "") +
        $"Classification: {OrbitalData.OrbitClass.OrbitClassType}, {OrbitalData.OrbitClass.OrbitClassDescription}.\n" +
        $"Url: {NasaUrl}.";
    }
}