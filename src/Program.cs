using System;
using System.Text;

class ActorInput
{
    public string url { get; set; }
    // providing the constructor to get rid of compilation warning CS8618 (not really needed here)
    public ActorInput(string url)
    {
        this.url = url;
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        var apifyClient = new SimpleApifyClient();

        // Input
        var input = await apifyClient.GetInput<ActorInput>();
        Console.WriteLine($"Input URL: {input.url}");

        // Computation
        HttpClient client = new HttpClient();
        var pageContent = await client.GetStringAsync(input.url);
        Console.WriteLine($"Got HTML from URL: {input.url}");

        // Output
        await apifyClient.SetOutput(pageContent);
    }
}
