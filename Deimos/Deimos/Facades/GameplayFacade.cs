using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    static class GameplayFacade
    {
        public static LocalPlayer ThisPlayer;
        public static LocalPlayerPhysics ThisPlayerPhysics;
        public static LocalPlayerDisplay ThisPlayerDisplay;
        public static WeaponsList Weapons;
        public static ObjectsList Objects;
        public static BulletManager BulletManager;
        public static Constants Constants;
    }
}
