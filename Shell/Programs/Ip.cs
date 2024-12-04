using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DNS;
using System;

namespace Venera.Shell.Programs
{
    internal class Ip : BuiltIn
    {
        public override string Name => "ip";

        public override string Description => "Display information about your network configuration.";

        public override CommandDescription ArgumentDescription => new();

        protected override ExitCode Execute()
        {
            WriteLine($"IPv4 address:\t{NetworkConfiguration.CurrentAddress.ToString()}");

            if (NetworkConfiguration.CurrentNetworkConfig.IPConfig == null)
            {
                WriteLine($"Subnet mask:\tN/A");
                WriteLine($"Gateway:\t\tN/A");
            }
            else
            {
                if (NetworkConfiguration.CurrentNetworkConfig.IPConfig.SubnetMask == null)
                {
                    WriteLine($"Subnet mask:\tN/A");
                }
                else
                {
                    WriteLine($"Subnet mask:\t{NetworkConfiguration.CurrentNetworkConfig.IPConfig.SubnetMask.ToString()}");
                }

                WriteLine($"Gateway:\t\t{NetworkConfiguration.CurrentNetworkConfig.IPConfig.DefaultGateway.ToString()}");
            }

            WriteLine($"MAC address:\t{NetworkConfiguration.CurrentNetworkConfig.Device.MACAddress.ToString()} ({NetworkConfiguration.CurrentNetworkConfig.Device.Name.ToString()})");
            Write($"DNS servers:\t");

            foreach (Address dns in DNSConfig.DNSNameservers)
            {
                Write($"{dns.ToString()} ");
            }

            WriteLine("\n\n=== [ CHECKS ] ===");
            Write("Internet connectivity check: ");

            using (var xClient = new ICMPClient())
            {
                xClient.Connect(new Address(1, 1, 1, 1));

                /** Send ICMP Echo **/
                xClient.SendEcho();

                /** Receive ICMP Response **/
                EndPoint endpoint = new EndPoint(Address.Zero, 0);
                int time = xClient.Receive(ref endpoint, timeout: 1000);

                if (time == 1)
                {
                    WriteLine($"successful");
                }
                else
                {
                    WriteLine($"failure");
                }
            }

            Write("DNS test: ");

            using (var xClient = new DnsClient())
            {
                xClient.Connect(new Address(1, 1, 1, 1));

                string[] domains = { "google.com", "github.com", "klier.dev" };

                foreach (var domain in domains)
                {
                    xClient.SendAsk(domain);
                    Address destination = xClient.Receive(timeout: 1000);

                    if (destination == null)
                    {
                        Write($"{domain} (fail) ");
                    }
                    else
                    {
                        Write($"{domain} ({destination.ToString()}) ");
                    }
                }
                WriteLine();
            }


            return ExitCode.Success;
        }
    }
}
