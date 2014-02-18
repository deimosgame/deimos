using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    public class WeaponsList
    {
        DeimosGame Game;

        // This is the static list of predefined Weapons
        public Dictionary<string, Weapon> WeaponList =
            new Dictionary<string, Weapon>();

        // Constructor
        public WeaponsList(DeimosGame game)
        {
            Game = game;
        }

        // Methods
        public void SetWeapon(Weapon w)
        {
            if (!WeaponList.ContainsValue(w))
            {
                WeaponList.Add(w.Name, w);
            }
        }

        public void Initialise()
        {
            SetWeapon(new Weapon(
                Game,
                new Vector3(3.8f, 0.5f, 0.7f), 0.05f,
                "Models/Weapons/PP19/PP19Model", "Assault Rifle", 2,
                0.1f,
                31, 147, 60,
                2.2f, 10, 25,
                2f, 500f
                )
            );
        }

        public Weapon GetWeapon(string name)
        {
            return WeaponList[name];
        }
    }
}
