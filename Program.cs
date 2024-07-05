// Lesen von SharePoint-Listenelementen mit .NET Core

using System.Net.Http.Headers;
using System.Text.Json;

var webSiteUrl = "https://sharepoint.contoso.local/sites/koeln";
var endpoint = $"{webSiteUrl}/_api/web/lists/getbytitle('Dokumente')/items?$select=FileRef,FileLeafRef";

// Instantiate a HttpClient object that uses a HttpClientHandler object that uses the default credentials
var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

// retrieve the items from the endpoint
var response = await client.GetAsync(endpoint);
var content = await response.Content.ReadAsStringAsync();

// Deserialize the JSON response into an array of items
var value = JsonSerializer.Deserialize<Value>(content);

if (value != null && value.Items != null)
{
    foreach (var item in value.Items)
    {
        Console.WriteLine($"FileRef: {item.FileRef}, FileLeafRef: {item.FileLeafRef}");
    }
}
