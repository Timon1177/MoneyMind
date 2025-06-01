using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;

public class StockService
{
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.twelvedata.com/price";

    public StockService()
    {
        var configText = File.ReadAllText("config.json");
        var json = JObject.Parse(configText);
        _apiKey = json["ApiKey"]?.ToString();
    }

    public async Task<string> GetPrice(string symbol)
    {
        using var client = new HttpClient();
        var response = await client.GetStringAsync($"{BaseUrl}?symbol={symbol}&apikey={_apiKey}");
        var json = JObject.Parse(response);
        return json["price"]?.ToString();
    }
}
