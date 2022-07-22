using System;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        HttpClient client = new HttpClient();
        var url = "https://apify.com"; // TODO: get url from input
        var pageContent = await client.GetStringAsync(url);
        Console.WriteLine($"Got HTML from URL: {url}");
        // Output
        var apifyClient = new SimpleApifyClient();
        await apifyClient.SetOutput(pageContent);
    }
}
