using dotenv.net;
using dotenv.net.Utilities;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Sputnik.Proxy
{
    internal class Program
    {
        public static string OR_API_KEY = "";

        static void Main(string[] args)
        {
            DotEnv.Load(options: new(probeForEnv: true));

            IDictionary<string, string> settings = DotEnv.Read();

            if (!EnvReader.TryGetStringValue("OPENROUTER_API_KEY", out string apiKey))
            {
                Console.WriteLine("OpenRouter API key is required to run this proxy. Make sure you provide the OPENROUTER_API_KEY environment variable.");
                Environment.Exit(1);
            }

            OR_API_KEY = apiKey;

            TcpListener server = new TcpListener(IPAddress.Any, 9999);

            server.Start();

            Console.WriteLine("Wait for incoming requests ...");
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();

                NetworkStream ns = client.GetStream();

                byte[] hello = new byte[100];
                hello = Encoding.Default.GetBytes("hello world");

                while (client.Connected)  //while the client is connected, we look for incoming messages
                {
                    byte[] msg = new byte[1024];     //the messages arrive as byte array
                    ns.Read(msg, 0, msg.Length);   //the same networkstream reads the message sent by the client
                    Console.WriteLine(Encoding.Default.GetString(msg).Trim()); //now , we write the message as string
                }
            }

        }
    }
}
