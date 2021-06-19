using System;
using System.IO;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.Configuration.Json;
using Lib = d03.Nasa;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

var commands = new string[] {"apod", "neows"};

if (args.Length < 1 || !commands.Contains(args[0].ToLower()))
{
    Console.WriteLine($"Usage:\n\tdotnet run <command> <parameters>");
    return;
}

async Task RunApot(string[] args, JsonConfigurationProvider config)
{
    if (args.Length < 1 || !ushort.TryParse(args[0], out var count))
    {
        Console.WriteLine($"Usage:\n\tdotnet run apot <count>");
        return;
    }
    if (config.TryGet("ApiKey", out var apiKey))
    {
        var client = new Lib.Apod.ApodClient(apiKey);
        var apot = await client.GetAsync(count);
        foreach (var a in apot.OrderBy(x=>x.DateTime))
            Console.WriteLine(a.ToString() + "\n");
    }
    else
        Console.WriteLine($"Error in configuration!!!!");

}

async Task RunNeows(string[] args, JsonConfigurationProvider config)
{
    if (args.Length < 1 || !ushort.TryParse(args[0], out var count))
    {
        count = ushort.MaxValue;
    }
    bool GetRequest(JsonConfigurationProvider config, ushort count, out Lib.NewWs.Models.AsteroidRequest req, out string apiKey)
    {
        req = null;
        if (!config.TryGet("ApiKey", out apiKey)
            || !config.TryGet("NeoWs:StartDate", out var start)
            || !config.TryGet("NeoWs:EndDate", out var end)
            || !DateTime.TryParse(start, out var startDT)
            || !DateTime.TryParse(end, out var endDT))
            return false;
        req = new Lib.NewWs.Models.AsteroidRequest()
        {
            StartDT = startDT,
            EndDT = endDT,
            Count = count
        };
        return true;
    }

    if (GetRequest(config, count, out var req, out var apiKey))
    {
        var client = new Lib.NewWs.NeoWsClient(apiKey);
        var neows = await client.GetAsync(req);
        foreach (var neo in neows)
            Console.WriteLine(neo + "\n");
    }
    else
        Console.WriteLine($"Error in configuration!!!!");
}
string codeBase = Assembly.GetExecutingAssembly().Location;
var exDir = Path.GetDirectoryName(codeBase);
var configPath = Path.Combine(exDir, "appsettings.json");

if (File.Exists(configPath))
{
    var configuration = new JsonConfigurationProvider(
            new JsonConfigurationSource(){Path = configPath});
    configuration.Load(File.Open(configPath, FileMode.Open));


    var func = new Func<string[], JsonConfigurationProvider, Task>[] {RunApot, RunNeows};

    var i = commands.ToList().IndexOf(args[0]);
    try{
        await func[i](args[1..], configuration);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}
else
    Console.WriteLine("Where appsettings.json?");