using System.Text;
using System.Text.Json;
using System.Net.Http.Json;

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
    public class InputException : Exception
    {
        public InputException() { }
        public InputException(string message)
            : base(message) { }
        public InputException(string message, Exception inner)
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

    /// <summary>Get input object from INPUT.json from the key-value store</summary>
    /// <exception cref="InputException">Thrown if getting input failed.</exception>
    public async Task<inputType> GetInput<inputType>()
    {
        inputType? input;
        if (Apify.isOnApify())
        {
            var defaultKeyValueStoreID = Environment.GetEnvironmentVariable("APIFY_DEFAULT_KEY_VALUE_STORE_ID");

            var url = $"https://api.apify.com/v2/key-value-stores/{defaultKeyValueStoreID}/records/INPUT";
            try
            {
                input = await httpClient.GetFromJsonAsync<inputType>(url);
            }
            catch (Exception e)
            {
                throw new InputException($"Failed to get input from {url}", e);
            }
        }
        else
        {
            var storageLocation = Environment.GetEnvironmentVariable("APIFY_LOCAL_STORAGE_DIR") ?? "./apify_storage";
            var pathToInput = Path.Join(storageLocation, "key_value_stores/default/INPUT.json");

            try
            {
                using (var inputStream = new FileStream(pathToInput, FileMode.Open))
                {
                    input = JsonSerializer.Deserialize<inputType>(inputStream);
                }
            }
            catch (Exception e)
            {
                throw new InputException($"Failed to read input from {pathToInput}", e);
            }
        }
        if (input == null) throw new Exception("Input is null");
        return input;
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
