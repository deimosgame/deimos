using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class Sound : Subhandler
    {
        public void SendWithPos(byte sound, Vector3 position)
        {
            Packet M = new Packet(Packet.PacketType.Sound);

            M.Packet_ID = 0x08;
            M.AddData(sound);
            M.AddData(0x01);
            M.AddData(position.X);
            M.AddData(position.Y);
            M.AddData(position.Z);
            M.Encode();

            NetworkFacade.UDP_Sending.Enqueue(M);
        }

        public void Send(byte sound)
        {
            Packet M = new Packet(Packet.PacketType.Sound);

            M.Packet_ID = 0x08;
            M.AddData(sound);
            M.AddData(0x00);

            M.Encode();

            NetworkFacade.UDP_Sending.Enqueue(M);
        }

        public void Interpret(byte[] buf)
        {
            if (GeneralFacade.SceneManager.SoundManager != null)
                return;

                string sound = GeneralFacade.SceneManager.SoundManager.GetSoundName(buf[4]);
                DisplayFacade.DebugScreen.Debug(sound);

                if (buf[5] == 0x00)
                {
                    DisplayFacade.DebugScreen.Debug("Local");
                    GeneralFacade.SceneManager.SoundManager.Play(sound);
                }
                else if (buf[5] == 0x01)
                {
                    Vector3 position = Vector3.Zero;
                    position.X = ExtractFloat32(buf, 6);
                    position.Y = ExtractFloat32(buf, 10);
                    position.Z = ExtractFloat32(buf, 14);

                    GeneralFacade.SceneManager.SoundManager.SetEmiter(sound, position);
                    GeneralFacade.SceneManager.SoundManager.SetListener(sound, GameplayFacade.ThisPlayer.Position);
                    GeneralFacade.SceneManager.SoundManager.Play3D(sound, GameplayFacade.ThisPlayer.Position, position);
                    DisplayFacade.DebugScreen.Debug(position.X.ToString() + position.Y.ToString() + position.Z.ToString());
                }
        }
    }
}
