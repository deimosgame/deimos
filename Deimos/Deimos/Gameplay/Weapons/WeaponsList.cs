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
                new Vector3(3.8f, 0.5f, 0.7f),
                new Vector3(3.5f, 0.5f, 0f), 0.05f, (float)Math.PI,
                "Models/Weapons/PP19/PP19Model", "Assault Rifle", 2, 'C',
                0.1f,
                31, 147, 60,
                2.2f, 10, 25,
                30f, 500f
                )
            );
            SetWeapon(new Weapon(
                Game,
                new Vector3(4f, 1.2f, 1.6f),
                new Vector3(3.5f, 1.5f, 0f), 0.0035f, (float)((Math.PI) / -2),
                "Models/Weapons/M9/Handgun", "Pistol", 1, 'B',
                0.2f,
                7, 56, 14,
                1.2f, 5, 15,
                30f, 300f
                )
            );
            SetWeapon(new Weapon(
                Game,
                new Vector3(0.6f, 1.3f, 0.5f),
                new Vector3(0.6f, 1.3f, 0.5f), 0.0025f, (float)((Math.PI) / -2),
                "Models/Weapons/RPG/RPG", "Bazooka", 4 , 'E',
                3f,
                1, 5, 1,
                3f, 40, 60,
                20f, 200f
                )
            );
            SetWeapon(new Weapon(
                Game,
                new Vector3(3.8f, 4f, 1.5f),
                new Vector3(3.8f, 4f, 1.5f), 1f, 1.8f,
                "Models/Weapons/Arbiter/arbiter", "Arbiter", 3, 'D',
                1.5f,
                0, 0, 0,
                0, 30, 50,
                0, 15,
                Type.Melee, ActionOnHit.Damage
                )
            );
            SetWeapon(new Weapon(
                Game,
                new Vector3(1.5f, 0.8f, 0.4f),
                new Vector3(1.5f, 0.8f, 0.4f), 0.1f, -1.2f,
                "Models/Weapons/Knife/knife", "Carver", 0, 'A',
                0.2f,
                0, 0, 0,
                0, 30, 50,
                0, 15,
                Type.Melee, ActionOnHit.Damage
                )
            );
        }

        public Weapon GetWeapon(string name)
        {
            return WeaponList[name];
        }
    }
}
