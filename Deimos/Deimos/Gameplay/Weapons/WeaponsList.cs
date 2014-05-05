using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    public class WeaponsList
    {
        // This is the static list of predefined Weapons
        public Dictionary<string, Weapon> WeaponList =
            new Dictionary<string, Weapon>();

        // Constructor
        public WeaponsList()
        {
            //
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
                new Vector3(1.5f, 0.8f, 0.4f),
                new Vector3(1.5f, 0.8f, 0.4f), 0.1f, -1.2f,
                "Models/Weapons/Knife/knife", "Carver", 0, 'A',
                0.4f,
                0, 0, 0,
                1, 30, 50,
                0, 15,
                Type.Melee, ActionOnHit.Damage
                )
            );
            SetWeapon(new Weapon(
                new Vector3(1.38f, 0.4f, 0.4f),
                new Vector3(1.4f, 0.35f, 0f), 0.001f, (float)((Math.PI) / -2),
                "Models/Weapons/M9/Handgun", "Pistol", 1, 'B',
                0.2f,
                7, 56, 14,
                1.2f, 5, 15,
                300f, 300f
                )
            );
            SetWeapon(new Weapon(
                new Vector3(3.8f, 0.5f, 0.7f),
                new Vector3(3.5f, 0.5f, 0f), 0.05f, (float)Math.PI,
                "Models/Weapons/PP19/PP19Model", "Assault Rifle", 2, 'C',
                0.1f,
                31, 147, 60,
                2.2f, 10, 25,
                300, 500f
                )
            );
            SetWeapon(new Weapon(
                new Vector3(3.8f, 4f, 1.5f),
                new Vector3(3.8f, 4f, 1.5f), 1f, 1.8f,
                "Models/Weapons/Arbiter/arbiter", "Arbiter", 3, 'D',
                1.5f,
                0, 0, 0,
                1.5f, 30, 50,
                0, 15,
                Type.Melee, ActionOnHit.Damage
                )
            );
            SetWeapon(new Weapon(
                new Vector3(0.6f, 1.3f, 0.5f),
                new Vector3(0.6f, 1.3f, 0.5f), 0.0025f, (float)((Math.PI) / -2),
                "Models/Weapons/RPG/RPG", "Bazooka", 4 , 'E',
                3f,
                1, 5, 1,
                3f, 40, 60,
                150f, 200f
                )
            );

            // Our Mystery Weapon!
            SetWeapon(new Weapon(
                new Vector3(3.8f, 0.5f, 0.7f),
                new Vector3(3.5f, 0.5f, 0f), 0.05f, (float)Math.PI,
                "Models/Weapons/PP19/PP19Model", "Essence of Phobia",
                10, 'P', 1, 1, 2,
                0, 2, 0, 0, 25, 75,
                Type.Firearm, ActionOnHit.Event)
            );
        }

        public string GetName(char c)
        {
            switch (c)
            {
                case 'A':
                    return "Carver";
                case 'B':
                    return "Pistol";
                case 'C':
                    return "Assault Rifle";
                case 'D':
                    return "Arbiter";
                case 'E':
                    return "Bazooka";
                case 'P':
                    return "Essence of Phobia";
                default:
                    return "hands";
            }
        }

        public char GetRep(string name)
        {
            switch (name)
            {
                case "Carver":
                    return 'A';
                case "Pistol":
                    return 'B';
                case "Assault Rifle":
                    return 'C';
                case "Arbiter":
                    return 'D';
                case "Bazooka":
                    return 'E';
                case "Essence of Phobia":
                    return 'P';
                default:
                    return '0';
            }
        }

        public Weapon GetWeapon(string name)
        {
            return WeaponList[name];
        }
    }
}
