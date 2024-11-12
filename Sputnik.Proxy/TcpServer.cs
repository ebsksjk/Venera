using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Sputnik.Proxy;

public class TcpServer
{
    private TcpListener _listener;
    private bool _isRunning;
    private readonly ConcurrentDictionary<TcpClient, Task> _clients = new ConcurrentDictionary<TcpClient, Task>();

    public TcpServer(string ipAddress, int port)
    {
        _listener = new TcpListener(IPAddress.Parse(ipAddress), port);
    }

    public void Start()
    {
        _listener.Start();
        _isRunning = true;
        Console.WriteLine($"Server started ...");

        Task.Run(() => AcceptClientsAsync());
    }

    private async Task AcceptClientsAsync()
    {
        while (_isRunning)
        {
            try
            {
                var client = await _listener.AcceptTcpClientAsync();
                Console.WriteLine("Client connected...");

                // Handle each client in a separate task
                var clientTask = HandleClientAsync(client);
                _clients.TryAdd(client, clientTask);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accepting client: {ex.Message}");
            }
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[8192];

        try
        {
            while (_isRunning && client.Connected)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break; // Client disconnected

                // Convert data received from the client
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received: {message}");

                await foreach (var str in OpenRouter.Prompt(message))
                {
                    Console.Write(str);

                    byte[] response = Encoding.ASCII.GetBytes(str);
                    await stream.WriteAsync(response, 0, response.Length);
                }

                // Send null-terminator to end stream
                await stream.WriteAsync([((byte)'E'), ((byte)'O'), ((byte)'F')], 0, 3);
                Console.WriteLine("Send EOF");

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client: {ex.Message}");
        }
        finally
        {
            client.Close();
            _clients.TryRemove(client, out _);
            Console.WriteLine("Client disconnected...");
        }
    }

    public void Stop()
    {
        _isRunning = false;
        _listener.Stop();

        // Wait for all clients to disconnect
        Task.WhenAll(_clients.Values).Wait();
        Console.WriteLine("Server stopped.");
    }
}
