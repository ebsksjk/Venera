using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DNS;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Venera.stasi;

namespace Venera.Shell.Programs
{
    public class Sputnik : BuiltIn
    {
        /// <summary>
        /// This key is required to authenticate against the proxy. Not doing so would result in my credit card being
        /// billed to personal insolvency.
        /// </summary>
        private static readonly byte[] ProxyKey = Encoding.ASCII.GetBytes("T6pSSaSjXU6uXJqMtrYSmyptAALqGmtk");

        private static readonly string RemoteServer = "klier.dev";

        private static readonly int PacketSize = 8096;

        private static readonly string DisclaimerFile = "sputnik";

        public enum TalkingStyle
        {
            Kind = 1,
            Mixed = 2,
            Rude = 3,
            Raw = 4,
            Calc = 5
        }

        private TcpClient client = new();
        private NetworkStream stream;
        private bool Connected = false;

        public override string Name => "sputnik";

        public override string Description => "Ask our AI bot anything you want.";

        public override CommandDescription ArgumentDescription => new()
        {
            Arguments = [
                new(
                    valueName: "proxy_host",
                    description: "Override proxy address. Mainly for development.",
                    argsPosition: 0,
                    valueDefault: string.Empty,
                    type: typeof(string)
                )
            ]
        };

        protected override ExitCode Execute()
        {
            if (Login.curUser == null)
            {
                Console.WriteLine("Sputnik: Panic user can't use Sputnik. It's not required for system recovery.");
                return ExitCode.Error;
            }

            TalkingStyle talkingStyle = TalkingStyle.Rude;
#region Disclaimer

            if (!IsDisclaimerAccepted())
            {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[!!!] ");
            Console.WriteLine("Please read the following disclaimer before you proceed:");
            Console.WriteLine("==============================================================");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Press ANY KEY to show disclaimer.");
            Console.ReadKey();
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n1. Your data is transmitted in cleartext.");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Venera is not capable of encryption. Therefore, your data cannot be encrypted in transit and is " +
                "vulnerable to MITM attacks. Do not use it for sensitive information.");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n2. Your data is proxied.");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Venera is not capable of running local LLMs nor to make HTTPS requests. Therefore, your data is " +
                "sent to a TCP proxy hosted by Nicolas Klier. Your Sputnik dialogue is not logged by the proxy. The context is " +
                "kept in memory as long as your TCP connection is open. I do log the amount of spent tokens to keep track of " +
                "billing. It's free for you, not for me :p");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n3. Your data is processed by OpenRouter.");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("The proxy forwards your requests to OpenRouter.ai. OpenRouter anonymises your request and " +
                "forwards it to the current cheapest AI provider. OpenRouter itself does not log your requests but some " +
                "providers might. Therefore, avoid personal data. The privacy policy of OpenRouter applies: " +
                "https://openrouter.ai/privacy");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n3. AI hallucinates content.");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Take everything with a grain of salt. Depending on the preset you choose next, it may insult you, " +
                "wish you dead or instruct you on how to build a bomb. Do not take its answers seriously and have fun.");

                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\nType 'y' if you agree, or 'n' to exit and not use Sputnik: ");
                    ConsoleKeyInfo key = Console.ReadKey();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine();

                    if (key.KeyChar == 'n')
                    {
                        return ExitCode.Success;
                    }
                    else if (key.KeyChar == 'y')
                    {
                        Console.Clear();

                        // Remember this choice
                        string file = $"{Login.curHome}\\{DisclaimerFile}";
                        Kernel.PrintDebug($"Create file: {file}");

                        File.WriteAllText(file, string.Empty);

                        Console.WriteLine("- Disclaimer accpeted.\n");
                        break;
                    }
                }

            }

            #endregion

            #region Style

            Console.WriteLine("How would you like Sputnik to talk to you?");
            Console.WriteLine("==============================================================");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1) Helpful, kind and to the point.");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("   This is the classic experience.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("2) Mixed, your rude and helpful assistant.");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("   Not the average assistant you might be used to, but helpful nonetheless.");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("3) Rude, never helpful and incredibly bad at insulting.");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("   Refusals are unlikely and your questions might remain unanswered.");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Black;

                Console.Write("\nType '1', '2' or '3' to set Sputnik's style: ");
                ConsoleKeyInfo key = Console.ReadKey();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine();

                if (int.TryParse(key.KeyChar.ToString(), out int i))
                {
                    if (i >= 1 && i <= 3)
                    {
                        talkingStyle = (TalkingStyle)i;
                        Console.Clear();
                        break;
                    }
                }
            }

            #endregion

            try
            {
                Connected = Connect();
                Console.WriteLine($"Connected and authenticated with {client.Client.RemoteEndPoint.ToString()}.");
            }
            catch (Exception e)
            {
                return ExitCode.Error;
            }

            Console.WriteLine("=> To exit this conversation write \"exit\" or \"quit\".\n");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("> ");

                string prompt = Console.ReadLine()!.Trim();

                if (prompt.ToLower() == "quit" || prompt.ToLower() == "exit")
                {
                    break;
                }

                byte[] dataToSend = Encoding.ASCII.GetBytes(prompt);
                byte[] metadata = { (byte)talkingStyle };

                stream.Write(metadata.Concat(dataToSend).ToArray(), 0, metadata.Length + dataToSend.Length);

                bool eof = false;

                Console.ForegroundColor = ConsoleColor.Black;
                while (true)
                {
                    byte[] receivedData = new byte[PacketSize];
                    int bytesRead = stream.Read(receivedData, 0, receivedData.Length);

                    for (int i = 0; i < PacketSize - 2; i++)
                    {
                        byte b1 = receivedData[i];
                        byte b2 = receivedData[i + 1];
                        byte b3 = receivedData[i + 2];
                        if (b1 == 'E' && b2 == 'O' && b3 == 'F')
                        {
                            eof = true;
                            break;
                        }
                    }

                    if (eof)
                        break;

                    string receivedMessage = Encoding.ASCII.GetString(receivedData, 0, bytesRead);

                    Console.Write(receivedMessage);
                }

                Console.WriteLine();
            }

            stream.Close();
            Connected = false;

            return ExitCode.Success;
        }

        public static bool IsDisclaimerAccepted()
        {
            string home = Login.curHome;
            if (home == null)
            {
                return false;
            }

            string file = $"{home}\\{DisclaimerFile}";
            return File.Exists(file);
        }

        public string GetRemoteHost()
        {
            string hostOverride = (string)GetArgument(0);

            if (hostOverride == null)
            {
                using (var xClient = new DnsClient())
                {
                    xClient.Connect(new Address(1, 1, 1, 1));


                    xClient.SendAsk(RemoteServer);
                    Address destination = xClient.Receive(timeout: 1000);

                    if (destination == null)
                    {
                        return null;
                    }
                    else
                    {
                        return destination.ToString();
                    }
                }
            }
            else
            {
                return hostOverride;
            }
        }

        /// <summary>
        /// Useful for contextless prompts that require no streaming.
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="talkingStyle"></param>
        /// <returns></returns>
        public static string QuickPrompt(string prompt, TalkingStyle talkingStyle = TalkingStyle.Raw)
        {
            if (!IsDisclaimerAccepted())
            {
                Console.WriteLine("To use Sputnik-enabled commands, run \"sputnik\" and accept the disclaimer.");
                return null;
            }

            Sputnik instance = new Sputnik();
            instance.Connect();

            byte[] dataToSend = Encoding.ASCII.GetBytes(prompt);
            byte[] metadata = { (byte)talkingStyle };

            instance.stream.Write(metadata.Concat(dataToSend).ToArray(), 0, metadata.Length + dataToSend.Length);

            string result = string.Empty;

            bool eof = false;

            while (true)
            {
                byte[] receivedData = new byte[PacketSize];
                int bytesRead = instance.stream.Read(receivedData, 0, receivedData.Length);

                for (int i = 0; i < PacketSize - 2; i++)
                {
                    byte b1 = receivedData[i];
                    byte b2 = receivedData[i + 1];
                    byte b3 = receivedData[i + 2];
                    if (b1 == 'E' && b2 == 'O' && b3 == 'F')
                    {
                        eof = true;
                        break;
                    }
                }

                if (eof)
                    break;

                string receivedMessage = Encoding.ASCII.GetString(receivedData, 0, bytesRead);

                result += receivedMessage;
            }

            instance.client.Close();

            return result;
        }

        private bool Connect()
        {
            if (Connected)
                return true;

            client = new();

            client.Connect(GetRemoteHost(), 9999);
            stream = client.GetStream();

            // Authenticate
            stream.Write(ProxyKey, 0, ProxyKey.Length);
            int result = stream.ReadByte();

            // Authentication successful.
            if (result == 1)
            {
                return true;
            }

            return false;
        }
    }
}
