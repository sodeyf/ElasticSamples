// See https://aka.ms/new-console-template for more information


// Define the connection settings
using ElasticSamples_Nest.Models;
using Nest;
using static System.Net.Mime.MediaTypeNames;

/*
var settings = new ConnectionSettings(new Uri("https://localhost:9200"))
    .BasicAuthentication("elastic", "yourpassword")
    .ServerCertificateValidationCallback((cert, chain, errors, n) => true);
*/

var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
    .DefaultIndex("my_index"); // Set the default index

// Create an Elasticsearch client
var client = new ElasticClient(settings);

// Test the connection
var pingResponse = client.Ping();
if (!pingResponse.IsValid)
    Console.WriteLine($"Error: {pingResponse.DebugInformation}");
else
    Console.WriteLine("Elasticsearch is alive!");


//---------------------------- Create an Index
var createIndexResponse = client.Indices.Create("products", c => c
    .Map<Product>(m => m.AutoMap()) // Automatically map the model
);
Console.WriteLine($"Index Created: {createIndexResponse.IsValid}");

//---------------------------- Insert Data into Elasticsearch
var product = new Product { Id = 2, Name = "Laptop", Price = 1299.99m };

var indexResponse = client.IndexDocument(product);
Console.WriteLine($"Document Indexed: {indexResponse.IsValid}");

//--------------------------- Insert bulk Data into Elasticsearch
var products = new List<Product>
{
    new Product { Id = 2, Name = "Smartphone", Price = 699.99m },
    new Product { Id = 3, Name = "Tablet", Price = 499.99m }
};
var bulkResponse = client.IndexMany(products);
Console.WriteLine($"Bulk Insert Success: {bulkResponse.IsValid}");
// ---------------------------Get
var response = client.Get<Product>(2);
Console.WriteLine($"Product: {response.Source.Name}, Price: {response.Source.Price}");

response = client.Get<Product>(3);
Console.WriteLine($"Product: {response.Source.Name}, Price: {response.Source.Price}");

//----------------------------Full - Text Search

var searchResponse = client.Search<Product>(s => s
    .Query(q => q
        .Match(m => m
            .Field(f => f.Name)
            .Query("foo") // Search term
        )
    )
);
foreach (var hit in searchResponse.Hits)
{
    Console.WriteLine($"Found: {hit.Source.Name} - {hit.Source.Price}");
}


Console.WriteLine("Hello, World!");
