using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class Weapon
    {
        public enum WeaponState
        {
            AtEase,
            Firing
        }

        private WeaponState state;
        public WeaponState State
        {
            get { return state; }
            set { state = State; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = Name; }
        }


    }
}
