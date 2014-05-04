using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class MainHandler
    {
        // Packet Handling attributes
        public Handshake Handshakes = new Handshake();
        public Connection Connections = new Connection();
        public Disconnection Disconnections = new Disconnection();
        public Chat Chats = new Chat();
        public Broadcast Broadcasts = new Broadcast();
        public Move Moves = new Move();
        public PlayerList Players = new PlayerList();
        public UpdateInfo PlayerInfoUpdate = new UpdateInfo();

        // METHODS
        public void Distribute(byte[] buf)
        {
            switch (buf[1])
            {
                case 0x00:
                    Handshakes.Interpret(buf);
                    break;
                case 0x01:
                    Connections.Interpret(buf);
                    break;
                case 0x02:
                    Disconnections.Interpret(buf);
                    break;
                case 0x03:
                    Chats.Handle(buf);
                    break;
                case 0x04:
                    Broadcasts.Handle(buf);
                    break;
                case 0x06:
                    Players.Interpret(buf);
                    break;
                default:
                    return;
            }
        }

        public void Process()
        {
            Broadcasts.Process();
            Chats.Process();
        }
    }
}
