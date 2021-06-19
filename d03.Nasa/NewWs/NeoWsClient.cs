using System;
using d03.Nasa.NewWs.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace d03.Nasa.NewWs
{
    public class NeoWsClient : ApiClientBase, INasaClient<AsteroidRequest, Task<AsteroidLookup[]>>
    {
        public NeoWsClient (string apikey) : base(apikey)
        {}

        public async Task<AsteroidLookup[]> GetAsync(AsteroidRequest request)
        {
            var frst = $"https://api.nasa.gov/neo/rest/v1/feed?start_date={request.StartDT.ToString("yyyy-MM-dd")}&end_date={request.EndDT.ToString("yyyy-MM-dd")}";
            var lookup = (await HttpGetAsync<Dictionary<DateTime, AsteroidInfo[]>>(frst, "near_earth_objects")).Values.SelectMany(x=>x).ToArray();
            int i = request.Count;
            if (lookup.Length > request.Count)
                lookup = lookup.OrderBy(x => x.Kilometers).ToArray()[..i];
            var result = new ConcurrentQueue<AsteroidLookup>();
            var rez = Parallel.ForEach(lookup, x =>
                result.Enqueue(HttpGetAsync<AsteroidLookup>($"https://api.nasa.gov/neo/rest/v1/neo/{x.Id}").Result));
            return result.ToArray();
        }
    }
}