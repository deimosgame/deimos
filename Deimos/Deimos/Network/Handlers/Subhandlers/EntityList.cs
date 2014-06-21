using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class EntityList : Subhandler
    {
        public void Interpret(byte[] buf)
        {
            int i = 4;

            while (i < buf.Length && buf[i] != 0)
            {
                // Identifying the type and creating entity in the world
                if (Unmask(buf[i], 8))
                {
                    if (Unmask(buf[i], 7))
                    {
                        // Weapon entity
                        string t = GeneralFacade.SceneManager.ObjManager().AddWeapon(
                            GetWNameFromChar(ExtractPrefix(buf, i+1)),
                            ExtractInt32(buf, i+2),
                            new Vector3(ExtractFloat32(buf, i+6), ExtractFloat32(buf, i+10), ExtractFloat32(buf, i+14)),
                            PickupObject.State.Inactive,
                            2000,
                            new Vector3(ExtractFloat32(buf, i+18), ExtractFloat32(buf, i+22), 0)
                        );

                        // Checking whether our entity is currently active or not
                        if (Unmask(buf[i], 6))
                        {
                            // Entity active
                            GeneralFacade.SceneManager.ObjManager().GetWeapon(t).Status = PickupObject.State.Active;
                        }
                    }
                    else
                    {
                        // Bullet, so we do nothing
                    }
                }
                else
                {
                    if (Unmask(buf[i], 7))
                    {
                        // Effect entity
                        string t = GeneralFacade.SceneManager.ObjManager().AddEffect(
                            GetENameFromChar(ExtractPrefix(buf, i + 1)),
                            new Vector3(ExtractFloat32(buf, i + 6), ExtractFloat32(buf, i + 10), ExtractFloat32(buf, i + 14)),
                            PickupObject.State.Inactive,
                            ExtractInt32(buf, i + 2),
                            2000,
                            5000,
                            new Vector3(ExtractFloat32(buf, i + 18), ExtractFloat32(buf, i + 22), 0)
                        );

                        // Checking whether our entity is currently active or not
                        if (Unmask(buf[i], 6))
                        {
                            // Entity active
                            GeneralFacade.SceneManager.ObjManager().GetEffect(t).Status = PickupObject.State.Active;
                        }
                    }
                }

                i += 23;
            }
        }

        public bool Unmask(byte b, int i)
        {
            var bit = b & (1 << i-1);
            bool a = bit != 0;
            
            return a;
        }

        public string GetWNameFromChar(string c)
        {
            switch (c)
            {
                case "A":
                    return "Carver";
                case "B":
                    return "Pistol";
                case "C":
                    return "Assault Rifle";
                case "D":
                    return "Arbiter";
                case "E":
                    return "Bazooka";
                case "P":
                    return "Essence of Phobia";
                default:
                    return "";
            }
        }

        public string GetENameFromChar(string c)
        {
            switch (c)
            {
                case "A":
                    return "Health Pack";
                case "B":
                    return "Speed Boost";
                case "C":
                    return "Gravity Boost";
                default:
                    return "";
            }
        }
    }
}
