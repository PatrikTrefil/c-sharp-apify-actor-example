# Example Apify actor in C#

Minimal example for developing [Apify][1] actor in [C# programming language][2].
[Here][3] is the actor's page on the Apify store. The code is open source.

The example:

-   Reads user provided URL from input
-   Does HTTP request to get HTML from this URL
-   Saves HTML into OUTPUT record in Key Value Store

This actor works both on Apify platform and locally.

## Local usage

For local usage, you need to:

1. Clone the [repository][4]
2. [Install dotnet][5]
3. Create local storage with `apify init` or manually create folder path `apify_storage/key_value_stores/default/`
4. Add `INPUT.json` file input inside this path that looks like this:

```
{
    "url": "https://apify.com"
}
```

5. Build and run the solution with `dotnet run`
6. You can find `OUTPUT.html` next to `INPUT.json`

[1]: https://apify.com/
[2]: https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/
[3]: https://apify.com/patrik.trefil/c-sharp-apify-actor-example
[4]: https://github.com/PatrikTrefil/c-sharp-apify-actor-example
[5]: https://dotnet.microsoft.com/en-us/download
