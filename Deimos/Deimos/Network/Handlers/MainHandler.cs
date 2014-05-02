using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deimos.Facades;

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

        // METHODS
        public void Distribute(byte[] buf)
        {
            switch (buf[1])
            {
                case 0x00:
                    Handshakes.InterpretHS(buf);
                    break;
                case 0x01:
                    Connections.InterpretCN(buf);
                    break;
                case 0x02:
                    Disconnections.InterpretDC(buf);
                    break;
                case 0x03:
                    Chats.Handle(buf);
                    break;
                case 0x04:
                    Broadcasts.Handle(buf);
                    break;
                default:
                    return;
            }
        }

        public void Process()
        {
            Broadcasts.ProcessBC();
            Chats.ProcessCH();
        }
    }
}
