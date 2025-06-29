using System.Net;
using ChatServer;

var hostName = Dns.GetHostName();
IPHostEntry localhost = await Dns.GetHostEntryAsync(hostName);
// This is the IP address of the local machine
IPAddress localIpAddress = localhost.AddressList[0];

Server chatServer = new Server(localIpAddress);

await chatServer.ListenForClients();

