using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    public class ObjectsList
    {
        // The Pre-set objects list
        private Dictionary<string, PickupWeapon> WeaponObjects =
            new Dictionary<string, PickupWeapon>();
        private Dictionary<string, PickupEffect> EffectObjects =
            new Dictionary<string, PickupEffect>();

        // Methods
        private void SetWeaponObj(PickupWeapon w)
        {
            if (!WeaponObjects.ContainsValue(w))
            {
                WeaponObjects.Add(w.Name, w);
            }
        }

        private void SetEffectObj(PickupEffect e)
        {
            if (!EffectObjects.ContainsValue(e))
            {
                EffectObjects.Add(e.Name, e);
            }
        }

        public void Initialize()
        {
            // Preset Weapon Objects to be picked up
            SetWeaponObj(new PickupWeapon("Carver",
                "Models/Weapons/Knife/knife",
                0.1f, 
                "A")
            );
            SetWeaponObj(new PickupWeapon("Pistol",
                "Models/Weapons/M9/Handgun",
                0.0035f,
                "B")
            );
            SetWeaponObj(new PickupWeapon("Assault Rifle",
                "Models/Weapons/PP19/PP19Model",
                0.05f,
                "C")
            );
            SetWeaponObj(new PickupWeapon("Bazooka",
                "Models/Weapons/RPG/RPG",
                0.0025f,
                "D")
            );
            SetWeaponObj(new PickupWeapon("Arbiter",
                "Models/Weapons/Arbiter/arbiter",
                1,
                "E")
            );

            // Preset Effect Objects to be picked up
            SetEffectObj(new PickupEffect("Health Pack",
                "Models/Weapons/Knife/knife",
                1.5f,
                PickupEffect.Effect.Health)
            );
            SetEffectObj(new PickupEffect("Gravity Boost",
                "Models/Weapons/Arbiter/arbiter",
                1,
                PickupEffect.Effect.Speed)
            );
            SetEffectObj(new PickupEffect("Speed Boost",
                "Models/Weapons/PP19/PP19Model",
                0.05f,
                PickupEffect.Effect.Speed)
            );
        }

        public PickupWeapon GetWeaponObject(string n)
        {
            return WeaponObjects[n];
        }

        public PickupEffect GetEffectObject(string n)
        {
            return EffectObjects[n];
        }
    }
}
