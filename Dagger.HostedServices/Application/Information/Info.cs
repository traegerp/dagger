using System;
using System.Net;
using System.Net.Sockets;
using Dagger.HostedServices.Application.Configuration;

namespace Dagger.HostedServices.Application.Information
{
    public class Info
    {

        public Settings Settings {get; set;}
        public string IpAddress {get; set;}
        public string Node {get; set;}

        /// <summary>
        /// Get Ip Address
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPAddress()
        {

            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

    }
}