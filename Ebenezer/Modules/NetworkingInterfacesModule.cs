using System.Net.NetworkInformation;

namespace Ebenezer;

public static class NetworkingInterfacesModule
{
    public static List<NetworkInterface> ListAllNetworkingInterfaces() => NetworkInterface.GetAllNetworkInterfaces().ToList();
}