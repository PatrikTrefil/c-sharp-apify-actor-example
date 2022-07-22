using System.Text;

public class SimpleApifyClient
{
    HttpClient httpClient = new HttpClient();
    /// <summary>
    /// Save given content to the key-value store under OUTPUT.html
    /// </summary>
    /// <exception>Throws HttpRequestException if the request fails.</exception>
    public async Task SetOutput(string content)
    {
        if (Apify.isOnApify())
        {
            var defaultKeyValueStoreID = Environment.GetEnvironmentVariable("APIFY_DEFAULT_KEY_VALUE_STORE_ID");
            var token = Environment.GetEnvironmentVariable("APIFY_TOKEN");

            var url = $"https://api.apify.com/v2/key-value-stores/{defaultKeyValueStoreID}/records/OUTPUT?token={token}";
            var response = await httpClient.PutAsync(url, new StringContent(content, Encoding.UTF8, "text/html"));
            response.EnsureSuccessStatusCode();
        }
        else
        {
            var storageLocation = Environment.GetEnvironmentVariable("APIFY_LOCAL_STORAGE_DIR") ?? "./apify_storage";
            var pathToDefaultKeyValueStore = Path.Join(storageLocation, "key_value_stores/default/OUTPUT.html");
            using (var streamWriter = new StreamWriter(pathToDefaultKeyValueStore))
            {
                streamWriter.Write(content);
            }
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
