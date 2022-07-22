using System.Text;

public class SimpleApifyClient
{
    public class OutputException : Exception
    {
        public OutputException() { }
        public OutputException(string message)
            : base(message) { }
        public OutputException(string message, Exception inner)
            : base(message, inner) { }
    }
    HttpClient httpClient = new HttpClient();
    /// <summary>
    /// Save given content to the key-value store under OUTPUT.html
    /// </summary>
    /// <exception cref="OutputException">Thrown if storing output failed.</exception>
    public async Task SetOutput(string content)
    {
        if (Apify.isOnApify())
        {
            var defaultKeyValueStoreID = Environment.GetEnvironmentVariable("APIFY_DEFAULT_KEY_VALUE_STORE_ID");
            var token = Environment.GetEnvironmentVariable("APIFY_TOKEN");

            var url = $"https://api.apify.com/v2/key-value-stores/{defaultKeyValueStoreID}/records/OUTPUT?token={token}";
            try
            {
                var response = await httpClient.PutAsync(url, new StringContent(content, Encoding.UTF8, "text/html"));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                throw new OutputException($"Failed to post output to {url}", e);
            }
        }
        else
        {
            var storageLocation = Environment.GetEnvironmentVariable("APIFY_LOCAL_STORAGE_DIR") ?? "./apify_storage";
            var pathToOutput = Path.Join(storageLocation, "key_value_stores/default/OUTPUT.html");
            try
            {
                using (var streamWriter = new StreamWriter(pathToOutput))
                {
                    streamWriter.Write(content);
                }
            }
            catch (Exception e)
            {
                throw new OutputException($"Failed to write output to {pathToOutput}", e);
            }
        }
    }
}

public class Apify
{
    /// <returns>bool indicating whether the code is running on the Apify platform or locally</returns>
    public static bool isOnApify()
    {
        return Environment.GetEnvironmentVariable("APIFY_IS_AT_HOME") != null;
    }
}
