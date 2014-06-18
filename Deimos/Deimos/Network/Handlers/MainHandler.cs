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
        public Sound Sounds = new Sound();

        // METHODS
        public void Distribute(byte[] buf)
        {
            if (ChecksumCorrect(buf))
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
                    case 0x08:
                        Sounds.Interpret(buf);
                        break;
                    default:
                        return;
                }
            }
        }

        public bool ChecksumCorrect(byte[] buf)
        {
            byte supposed = buf[0];
            byte computed = 0;

            for (int i = 1; i < 576; i++)
            {
                byte current = buf[i];

                for (int j = 0; current > 0; j++)
                {
                    computed += (byte)((j % 2 + 1) * (current % 2));
                    current = (byte)(current >> 1);
                }
            }

            return (supposed == computed);
        }

        public void Process()
        {
            Broadcasts.Process();
            Chats.Process();
        }
    }
}
