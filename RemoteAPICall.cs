using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace PulsarRemoteAPI;

public class RemoteAPICall
{
    // HttpClient lifecycle management best practices:
// https://learn.microsoft.com/dotnet/fundamentals/networking/http/httpclient-guidelines#recommended-use
    
    private static HttpClient client = new()
    {
        BaseAddress = new Uri("http://127.0.0.1:17014"),
    };

    static RemoteAPICall() 
    {
        client.DefaultRequestHeaders.Add("X-Pulsar-Bridge-Id",GetGuid());  
    }

    public static async Task<Dictionary<string,string>> MakeCall(string type, string objectType, Dictionary<string,object> data) 
    {
        var request = new Dictionary<string,object> {
            { "type", type},
            { "object", objectType },
            { "data", data }
        };
        var requestEnvelope = new Dictionary<string,object> {
            { "data", request }
        };
        var prettyRequestJsonString = @"Request could not be built";
        var jsonResponse = @"Request could not be completed";
        try {
            // create our request Json
            using StringContent json = new(
                JsonSerializer.Serialize(requestEnvelope, new JsonSerializerOptions(JsonSerializerDefaults.Web)),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            var requestJsonString = await json.ReadAsStringAsync();
            prettyRequestJsonString = PrettyJson(requestJsonString);

            // create a unique request Id        
            client.DefaultRequestHeaders.Add("X-Pulsar-Request-Id",GetGuid());  

            using HttpResponseMessage httpResponse =
                await client.PostAsync("/jsapi", json);
            
            if (!httpResponse.IsSuccessStatusCode) {
                jsonResponse = $"{jsonResponse}: {httpResponse.StatusCode} - {httpResponse.ReasonPhrase}";
            }
            else {
                jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                jsonResponse = PrettyJson(jsonResponse);
            }
        }
        catch (Exception e) {
            jsonResponse = $"Request Failed:\n{e.Message}";
        }
        Console.WriteLine($"{jsonResponse}\n");
        var results = new Dictionary<string,string>{
            { "request", prettyRequestJsonString},
            { "result", jsonResponse }
        };
        return results;
    }

    private static string PrettyJson(string unPrettyJson)
    {
        var options = new JsonSerializerOptions(){
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        var jsonElement = JsonSerializer.Deserialize<JsonElement>(unPrettyJson);

        return JsonSerializer.Serialize(jsonElement, options);
    }

    private static string GetGuid() 
    {
        return Guid.NewGuid().ToString();
    }


}
