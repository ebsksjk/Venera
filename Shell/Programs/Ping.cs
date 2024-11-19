using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DNS;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Venera.Shell.Programs
{
    internal class Ping : BuiltIn
    {
        public override string Name => "ping";

        public override string Description => "Ping a remote host.";

        public override ExitCode Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Sokolsh: ping: Destination address required");
                return ExitCode.Error;
            }

            string[] split = args[0].Split('.');
            byte[] ip = new byte[4];

            if (split.Length == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (!byte.TryParse(split[i], out byte octet))
                    {
                        Console.WriteLine($"Sokolsh: ping: Destination address contains invalid octet: {split[i]}");
                        return ExitCode.Success;
                    }

                    ip[i] = octet;
                }
            }
            else
            {
                using (var xClient = new DnsClient())
                {
                    xClient.Connect(new Address(1, 1, 1, 1));

                    /** Send DNS ask for a single domain name **/
                    xClient.SendAsk(args[0]);

                    Address d = xClient.Receive();

                    if (d == null)
                    {
                        Console.WriteLine($"ping: {args[0]}: Name or service not known");
                        return ExitCode.Error;
                    }

                    ip = d.ToByteArray();
                }
            }

            Address dest = new(ip[0], ip[1], ip[2], ip[3]);

            string comment = args[0] == "127.0.0.1" ? "// You're pinging localhost? Are you stupid?" : "";
            Console.WriteLine($"PING {args[0]} ({dest.ToString()}) {comment}");

            using (var xClient = new ICMPClient())
            {
                xClient.Connect(dest);

                for (int i = 0; i < 4; i++)
                {
                    /** Send ICMP Echo **/
                    xClient.SendEcho();

                    /** Receive ICMP Response **/
                    EndPoint endpoint = new EndPoint(Address.Zero, 0);
                    int time = xClient.Receive(ref endpoint);

                    if (time == -1)
                    {
                        Console.WriteLine("Destination is not available.");
                    }
                    else
                    {
                        Console.WriteLine($"Received from {endpoint.Address.ToString()}: icmp_seq={i} time={time}ms");
                    }
                }
            }


            return ExitCode.Success;
        }

        private bool IsValidDomain(string target)
        {
            string[] domainSplit = target.Split(".");

            if (domainSplit.Length == 0)
            {
                // No dot found inside string.
                return false;
            }

            if (domainSplit.Last().Length < 2)
            {
                // TLDs can't be empty or contain only one character.
                return false;
            }

            // TODO: Basically we need to check if the "left" side of our domain contains any illegal characters.
            //       But that's a problem for later.

            return true;
        }
    }
}
