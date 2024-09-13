using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CurrencyAPITest
{
    class Program
    {
        // The entry point of the console application.
        static async Task Main(string[] args)
        {
            // API key to authenticate the request. Typically, you should store this securely.
            // In this case, it's assumed to be stored in a separate `Config` class.
            string apiKey = Config.ApiKey;

            // Base URL for the currency exchange API service.
            string baseUrl = "https://api.apilayer.com/exchangerates_data/";

            // HttpClient is used to send HTTP requests and receive HTTP responses.
            // HttpClient httpClient = new HttpClient();
            HttpClient httpClient = new();

            // Full URL for the API endpoint to retrieve the currency symbols.
            // Appending "symbols" to the base URL to hit the correct API endpoint.
            string fullUrl = baseUrl + "symbols";

            try
            {
                // Create a new HTTP GET request.
                // This specifies the HTTP method (GET) and the full URL for the request.
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, fullUrl);

                // Add the API key to the request headers.
                // Many APIs require authentication via headers.
                request.Headers.Add("apikey", apiKey);

                // Send the HTTP request and wait for the server's response asynchronously.
                // The response will contain the raw HTTP content returned by the API.
                HttpResponseMessage response = await httpClient.SendAsync(request);

                // Ensure that the HTTP response was successful (e.g., status code 200 OK).
                // If not, this will throw an exception.
                response.EnsureSuccessStatusCode();

                // Read the response content as a JSON string.
                // This JSON contains the actual data returned by the API.
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Output the raw JSON response to the console for debugging or verification.
                Console.WriteLine(jsonResponse);

                // Deserialize the JSON response into the `GetExchangeCode` object.
                // This step converts the raw JSON into a C# object that can be worked with.
                // In this case, it's deserializing to the `GetExchangeCode` class which contains a Dictionary for symbols.
                var myCodes = JsonConvert.DeserializeObject<GetExchangeCode>(jsonResponse);

                // If the deserialization was successful and `myCodes.Code` (the dictionary) is not null,
                // proceed to iterate over the dictionary.
                if (myCodes != null && myCodes.Code != null)
                {
                    // Iterate over the dictionary to get each key-value pair (currency code and its description).
                    foreach (var code in myCodes.Code)
                    {
                        // Output each currency code (key) and its corresponding description (value) to the console.
                        Console.WriteLine($"Currency Code: {code.Key}, Description: {code.Value}");
                    }
                }
                else
                {
                    // If no currency codes are found in the API response, print a message indicating that.
                    Console.WriteLine("No currency codes found.");
                }
            }
            catch (Exception e)
            {
                // If any exception occurs during the process (e.g., network error, API failure), 
                // print the error message to the console for debugging purposes.
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                // Dispose of the `httpClient` once it's no longer needed to free up resources.
                httpClient.Dispose();
            }
        }
    }
}
