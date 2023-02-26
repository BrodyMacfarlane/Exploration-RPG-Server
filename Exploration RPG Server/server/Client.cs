namespace Exploration_RPG_Server.server
{
    public class Client
    {
        public bool connected;
        public int player_id;
        public DateTime? connection_ts;

        public Client(
            bool connectedValue = false,
            int playerIdValue = -1
            )
        {
            connected = connectedValue;
            player_id = playerIdValue;
            connection_ts = DateTime.Now;
        }
    }
}

