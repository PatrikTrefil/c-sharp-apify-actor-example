public class SimpleApifyClient
{
    HttpClient httpClient = new HttpClient();
    /// <summary>
    /// Save given content to the key-value store under given key.
    /// Content type is included in the content object.
    /// </summary>
    /// <exception>Throws HttpRequestException if the request fails.</exception>
    public async Task SaveHTMLToKeyValueStore(string key, StringContent content)
    {
        if (Apify.isOnApify())
        {
            var defaultKeyValueStoreID = Environment.GetEnvironmentVariable("APIFY_DEFAULT_KEY_VALUE_STORE_ID");
            var token = Environment.GetEnvironmentVariable("APIFY_TOKEN");

            var url = $"https://api.apify.com/v2/key-value-stores/{defaultKeyValueStoreID}/records/OUTPUT?token={token}";
            var response = await httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
        }
        else
        {
            // TODO: save to file
        }
    }
}

public class Apify
{
    public static bool isOnApify()
    {
        return Environment.GetEnvironmentVariable("APIFY_IS_AT_HOME") != null;
    }
}
