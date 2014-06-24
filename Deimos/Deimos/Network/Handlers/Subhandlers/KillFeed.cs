using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class KillFeed : Subhandler
    {
        public void Interpret(byte[] buf)
        {
            if (NetworkFacade.Players.List.ContainsKey(buf[4])
                && NetworkFacade.Players.List.ContainsKey(buf[5]))
            {
                if (buf[4] == buf[5])
                {
                    GameplayFacade.KillsInterface.Add(NetworkFacade.Players.List[buf[5]].Name,
                        "Gravity...",
                        NetworkFacade.Players.List[buf[5]].Name
                    );
                }
                else
                {
                    GameplayFacade.KillsInterface.Add(
                        NetworkFacade.Players.List[buf[5]].Name,
                        GetNameFromByte(buf[6]),
                        NetworkFacade.Players.List[buf[4]].Name
                    );
                }
            }
        }

        public string GetNameFromByte(byte b)
        {
            switch (b)
            {
                case 0x00:
                    return "Carver";
                case 0x01:
                    return "Pistol";
                case 0x02:
                    return "Rifle";
                case 0x03:
                    return "Arbiter";
                case 0x04:
                    return "RPG";
                default:
                    return " ";
            }
        }
    }
}
