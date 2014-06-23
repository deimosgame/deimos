using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class BulletCreate : Subhandler
    {
        public void Send(Bullet bullet)
        {
            Packet p = new Packet(Packet.PacketType.BulletCreate);

            p.Packet_ID = 0x0B;

            // Writing the representation char
            p.AddData(bullet.WeaponRep);
            // Writing the initial bullet position
            p.AddData(bullet.Position.X);
            p.AddData(bullet.Position.Y);
            p.AddData(bullet.Position.Z);
            // Writing the direction vector3
            p.AddData(bullet.Direction.X);
            p.AddData(bullet.Direction.Y);
            p.AddData(bullet.Direction.Z);
            // Writing the current player rotation
            p.AddData(GameplayFacade.ThisPlayer.Rotation.X);
            p.AddData(GameplayFacade.ThisPlayer.Rotation.Y);
            p.AddData(GameplayFacade.ThisPlayer.Rotation.Z);
            // Writing velocity
            p.AddData(bullet.speed);
            // Writing lifespan
            p.AddData(bullet.lifeSpan);

            p.Encode();

            NetworkFacade.TCP_Sending.Enqueue(p);
        }

        public void Interpret(byte[] buf)
        {
            if (GameplayFacade.BulletManager == null)
                return;

            GameplayFacade.BulletManager.SpawnOtherBullet(
                new Vector3(
                    ExtractFloat32(buf, 5),
                    ExtractFloat32(buf, 9),
                    ExtractFloat32(buf, 13)
                    ),
                new Vector3(
                    ExtractFloat32(buf, 17),
                    ExtractFloat32(buf, 21),
                    ExtractFloat32(buf, 25)
                    ),
                ExtractPrefix(buf, 4),
                new Vector3(
                    ExtractFloat32(buf, 29),
                    ExtractFloat32(buf, 33),
                    ExtractFloat32(buf, 37)
                    ),
                ExtractFloat32(buf, 41),
                ExtractFloat32(buf, 45)
            );
        }
    }
}
