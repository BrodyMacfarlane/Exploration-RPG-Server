using Exploration_RPG_Server.server;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Exploration_RPG_Server;

public class Server
{
    public int tickRate;
    public int maxClients;
    public int port;
    public List<Client> connections;

    public Server(
        int tickRateValue = 10,
        int maxClientsValue = 4,
        int portValue = 9050
        )
    {
        tickRate = tickRateValue;
        maxClients = maxClientsValue;
        port = portValue;
        connections = new List<Client>();

        for (int i = 0; i < maxClientsValue; i++)
        {
            connections.Add(new Client());
        }
    }

    public void start()
    {
        EventBasedNetListener listener = new EventBasedNetListener();
        NetManager server = new NetManager(listener);
        server.Start(port);

        listener.ConnectionRequestEvent += request =>
        {
            if (server.ConnectedPeersCount < maxClients)
                request.AcceptIfKey("SomeConnectionKey");
            else
                request.Reject();
        };

        listener.PeerConnectedEvent += peer =>
        {
            Console.WriteLine("We got connection: {0}", peer.EndPoint);
            NetDataWriter writer = new NetDataWriter();
            writer.Put("Hello client!");
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
        };

        while (!Console.KeyAvailable)
        {
            server.PollEvents();
            Thread.Sleep(15);
        }
        server.Stop();
    }

    public int findFreeClientIndex()
    {
        int index = connections.FindIndex(0, maxClients, c => c.connected == false);
        Console.WriteLine(index);
        return index;
    }
}