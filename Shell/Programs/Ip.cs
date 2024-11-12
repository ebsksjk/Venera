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

        public override ExitCode Execute(string[] args)
        {
            Console.WriteLine($"IPv4 address:\t{NetworkConfiguration.CurrentAddress.ToString()}");

            // THESE ERROR OUT? LIKE WTF?! DO NOT UNCOMMENT UNTIL FURTHER INVESTIGATION.
            if (NetworkConfiguration.CurrentNetworkConfig.IPConfig == null)
            {
                Console.WriteLine($"Subnet mask:\tN/A");
                Console.WriteLine($"Gateway:\t\tN/A");
            }
            else
            {
                if (NetworkConfiguration.CurrentNetworkConfig.IPConfig.SubnetMask == null)
                {
                    Console.WriteLine($"Subnet mask:\tN/A");
                }
                else
                {
                    Console.WriteLine($"Subnet mask:\t{NetworkConfiguration.CurrentNetworkConfig.IPConfig.SubnetMask.ToString()}");
                }

                Console.WriteLine($"Gateway:\t\t{NetworkConfiguration.CurrentNetworkConfig.IPConfig.DefaultGateway.ToString()}");
            }

            Console.WriteLine($"MAC address:\t{NetworkConfiguration.CurrentNetworkConfig.Device.MACAddress.ToString()} ({NetworkConfiguration.CurrentNetworkConfig.Device.Name.ToString()})");
            Console.Write($"DNS servers:\t");

            foreach (Address dns in DNSConfig.DNSNameservers)
            {
                Console.Write($"{dns.ToString()} ");
            }

            Console.WriteLine("\n\n=== [ CHECKS ] ===");
            Console.Write("Internet connectivity check: ");

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
                    Console.WriteLine($"successful");
                }
                else
                {
                    Console.WriteLine($"failure");
                }
            }

            Console.Write("DNS test: ");

            using (var xClient = new DnsClient())
            {
                xClient.Connect(new Address(1, 1, 1, 1)); //DNS Server address. We recommend a Google or Cloudflare DNS, but you can use any you like!

                string[] domains = { "google.com", "github.com", "klier.dev" };

                foreach (var domain in domains)
                {
                    xClient.SendAsk(domain);
                    Address destination = xClient.Receive(timeout: 1000);

                    if (destination == null)
                    {
                        Console.Write($"{domain} (fail) ");
                    }
                    else
                    {
                        Console.Write($"{domain} ({destination.ToString()}) ");
                    }
                }
                Console.WriteLine();
            }


            return ExitCode.Success;
        }
    }
}
