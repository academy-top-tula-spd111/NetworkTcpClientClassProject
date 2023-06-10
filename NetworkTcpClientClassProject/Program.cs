using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
/*
using TcpClient tcpClient = new TcpClient();

string url = "yandex.ru";
int port = 80;

try
{
    await tcpClient.ConnectAsync(url, port);
    Console.WriteLine("Connected!");
    NetworkStream netStream = tcpClient.GetStream();

    string request = $"GET / HTTP/1.1\r\nHost: {url}\r\nConnection: Close\r\n\r\n";
    byte[] requestBuffer = Encoding.UTF8.GetBytes(request);
    await netStream.WriteAsync(requestBuffer, 0, requestBuffer.Length);

    byte[] responseBuffer = new byte[1024];
    StringBuilder response = new StringBuilder();

    //int bytesRead;
    do
    {
        await netStream.ReadAsync(responseBuffer);
        response.Append(Encoding.UTF8.GetString(responseBuffer));
    } while (netStream.DataAvailable);

    Console.WriteLine(response);
}
catch(SocketException ex)
{
    Console.WriteLine(ex.Message);
}
*/

using TcpClient client = new TcpClient();
Console.WriteLine("Client start!");
await client.ConnectAsync(IPAddress.Loopback, 8888);

if(client.Connected)
    Console.WriteLine($"Connect to server: {client.Client.RemoteEndPoint}");

byte[] buffer = new byte[1024];
NetworkStream stream = client.GetStream();

int bytes = await stream.ReadAsync(buffer);
Console.WriteLine($"Message from server: {Encoding.UTF8.GetString(buffer)}");

//client.Client.Shutdown(SocketShutdown.Both);

/*
string message = "Hello server! " + DateTime.Now.ToLongTimeString();
buffer = Encoding.UTF8.GetBytes(message);

await stream.WriteAsync(buffer);
Console.WriteLine($"Message to server {message} sended");
*/

string[] messages = new[] { "First message", "Second message", "Other message" };

Console.WriteLine();

var streamWriter = client.GetStream();

foreach (string mess in messages)
{
    byte[] bufferMess = Encoding.UTF8.GetBytes(mess + "$");
    await streamWriter.WriteAsync(bufferMess);
    Console.WriteLine($"Multisend. Message {mess} sended");
}

await stream.WriteAsync(Encoding.UTF8.GetBytes("END$"));
