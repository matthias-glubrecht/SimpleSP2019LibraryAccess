// Lesen von SharePoint-Listenelementen mit .NET Core

using System.ComponentModel;
using System.Net;
using System.Net.Http.Headers;
using System.Security;
using System.Text.Json;
using System.Threading.Tasks.Dataflow;

var webSiteUrl = "https://sharepoint.contoso.local/sites/koeln";

// Define the endpoint URLs
var endpointItemsTemplate = $"{webSiteUrl}/_api/web/lists/getbytitle('Dokumente')/items?$select=FileLeafRef,FileRef,ProjektId,Title,ID,FileSizeDisplay&$top=5000&$orderby=FileLeafRef asc&$filter=ProjektId eq '$PROJEKTID$'";
var endpointListFields = $"{webSiteUrl}/_api/web/lists/getbytitle('Dokumente')/fields?$filter=Hidden eq false&$select=Title,InternalName,TypeDisplayName";

// Prüfen, ob die Antwort vom server einen Fehler darstellt
bool isErrorResponse(HttpResponseMessage response)
{
    if (response.StatusCode != HttpStatusCode.OK)
    {
        Console.WriteLine($"Fehler: {response.StatusCode}");
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            Console.WriteLine("Entweder das Kennwort ist falsch oder Sie haben keinen Zugriff.");
        }
        var s = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        // Try to deserialize the JSON response to a RequestError object
        try
        {
            var error = JsonSerializer.Deserialize<RequestError>(s);
            Console.WriteLine($"Fehlercode: {error.OdataError.Code}");
            Console.WriteLine($"Fehlermeldung: {error.OdataError.Message.Value}");
        }
        catch { }
        return true;
    }
    else
    {
        return false;
    }
}

// Function to query credentials
NetworkCredential QueryCredentials()
{
    Console.Write("Benutzername: ");
    var userName = Console.ReadLine();

    Console.Write($"Passwort für den Benutzer {userName}: ");

    if (Console.IsInputRedirected)
    {
        // Wenn wir das Programm direkt in Visual Studio ausführen, funktioniert Console.ReadKey nicht
        var password = Console.ReadLine();
        return new NetworkCredential(userName, password);
    }
    else
    {
        var password = new SecureString();

        // Das Kennwort wird zeichenweise eingelesen und in einen SecureString geschrieben
        while (true)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }
            Console.Write('*');
            password.AppendChar(key.KeyChar);
        }
        return new NetworkCredential(userName, password);
    }
}

// Instantiate a HttpClient object that uses a HttpClientHandler object that uses specific credentials
var credentials = QueryCredentials();
var client = new HttpClient(new HttpClientHandler() { Credentials = credentials });
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

Console.Write("Möchten Sie die Spalten der Liste anzeigen? (j/n): ");
var entry = Console.ReadLine();

if (entry != null && entry.ToLowerInvariant() == "j")
{
    // Feldinformationen auslesen
    var responseFields = await client.GetAsync(endpointListFields);
    if (!isErrorResponse(responseFields))
    {
        var contentFields = await responseFields.Content.ReadAsStringAsync();

        var fieldsValue = JsonSerializer.Deserialize<FieldsValue>(contentFields);

        if (fieldsValue != null && fieldsValue.Fields != null)
        {
            foreach (var field in fieldsValue.Fields)
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Feldname (Anzeige): " + field.Title);
                Console.WriteLine("Feldname (intern):  " + field.InternalName);
                Console.WriteLine("Feldtyp:            " + field.TypeDisplayName);
                Console.WriteLine("---------------------------------");
                Console.WriteLine();
            }
        }
    }
}
Console.WriteLine("");
Console.WriteLine("Bitte geben Sie die ProjektId ein, um die Dokumente zu filtern.");

var projektId = Console.ReadLine();

// retrieve the items from the endpoint
var endPoint = endpointItemsTemplate.Replace("$PROJEKTID$", projektId);
Console.WriteLine($"Endpoint: {endPoint}");
var responseItems = await client.GetAsync(endPoint);

if (isErrorResponse(responseItems))
{
    return;
}


var contentItems = await responseItems.Content.ReadAsStringAsync();

// Deserialize the JSON response
var itemsValue = JsonSerializer.Deserialize<ItemsValue>(contentItems);

if (itemsValue != null && itemsValue.Items != null)
{
    if (itemsValue.Items.Count == 0)
    {
        Console.WriteLine("Keine Dokumente gefunden.");
    }
    else
    {
        var lfdNr = 1;
        foreach (var item in itemsValue.Items)
        {
            // Ausgabe aller Spaltenwerte
            Console.WriteLine();
            Console.WriteLine("---------------------------------");
            Console.WriteLine($"Dokument {lfdNr++}");
            Console.WriteLine($"Dateiname:  '{item.FileLeafRef}'");
            Console.WriteLine($"Pfad:       '{item.FileRef}'");
            Console.WriteLine($"Projekt-ID: '{item.ProjektId}'");
            Console.WriteLine($"Titel:      '{item.Title}'");
            Console.WriteLine($"ID          {item.ID}");
            Console.WriteLine($"Größe:      {item.FileSizeDisplay}");
            Console.WriteLine();
        }
    }
}
