using System.Net;
using System.Net.Sockets;
using System.Text;

//IPAddress localAddress = IPAddress.Parse("127.0.0.1");
IPAddress localAddress = IPAddress.Loopback;
int localPort = 8888;

IPEndPoint localEndPoint = new IPEndPoint(localAddress, localPort);

//TcpListener listener = new TcpListener(localAddress, localPort);
TcpListener listener = new TcpListener(localEndPoint);

try
{
    listener.Start();
    Console.WriteLine("Server start!");

    while(true)
    {
        using TcpClient client = await listener.AcceptTcpClientAsync();
        //using Socket socketClient = await listener.AcceptSocketAsync();
        Console.WriteLine($"Remote client: {client.Client.RemoteEndPoint}");
        //Console.WriteLine($"Remote client: {socketClient.RemoteEndPoint}");
        NetworkStream stream = client.GetStream();

        string message = "Hello client! " + DateTime.Now.ToLongTimeString();

        
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        await stream.WriteAsync(buffer);
        Console.WriteLine($"Message to client: {message} sended");
        /*
        StringBuilder response = new StringBuilder();

        do
        {
            buffer = new byte[1024];
            await stream.ReadAsync(buffer);
            response.Append(Encoding.UTF8.GetString(buffer));
        } while (stream.DataAvailable);

        Console.WriteLine($"Message from client {response.ToString()} received");
        */
        List<byte> bufferList = new List<byte>();
        int bufferByte;

        while(true)
        {
            while ((bufferByte = stream.ReadByte()) != '$')
                bufferList.Add((byte)bufferByte);

            string mess = Encoding.UTF8.GetString(bufferList.ToArray());
            if (mess == "END") break;
            Console.WriteLine($"MultiReceive. Message {mess} received");
            bufferList.Clear();
        }
        


    }
    
}
finally
{
    listener.Stop();
}