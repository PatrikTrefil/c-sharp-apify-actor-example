# Example Apify actor in C#

Minimal example for developing [Apify](https://apify.com/) actor in [C# programming language](https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/). The code is open source.

The example:

- Reads user provided URL from input
- Does HTTP request to get HTML from this URL
- Saves HTML into OUTPUT record in Key Value Store

This actor works both on Apify platform and locally.

## Local usage

For local usage, you need to:

1. Clone the [repository](https://github.com/PatrikTrefil/c-sharp-apify-actor-example)
2. [Install dotnet](https://dotnet.microsoft.com/en-us/download)
3. Create local storage with `apify init` or manually create folder path `apify_storage/key_value_stores/default/`
4. Add `INPUT.json` file input inside this path that looks like this:

```
{
    "url": "https://apify.com"
}
```

5. Build and run the solution with `dotnet run`
6. You can find `OUTPUT.html` next to `INPUT.json`
