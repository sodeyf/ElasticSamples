// See https://aka.ms/new-console-template for more information
using Aerospike.Client;

Console.WriteLine("Hello, World!");

// Connect to Aerospike server
AerospikeClient client = new AerospikeClient("192.168.48.30", 3000);

// Create a key
Key key = new Key("pre-order", "demo", "myKey");


Dictionary<string, object> userMap = new Dictionary<string, object>
{
    { "Name", "Alice" },
    { "Age", 30 }
};


// Write a record
Bin bin = new Bin("myBin", userMap);
client.Put(null, key, bin);

// Read the record
Record record = client.Get(null, key);
Dictionary<object, object> fetched = (Dictionary<object, object>)record.GetValue("myBin");

//// Read
//Record rec = client.Get(null, key);


Console.WriteLine($"User: {fetched["Name"]}, Age: {fetched["Age"]}");

//Console.WriteLine("Read value: " + record.GetValue("myBin"));

// Close the connection
client.Close();





