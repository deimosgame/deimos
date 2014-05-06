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
            SetWeaponObj(new PickupWeapon("CarverPickup",
                "Models/Weapons/Knife/knife",
                0.1f, 
                'A')
            );
            SetWeaponObj(new PickupWeapon("PistolPickup",
                "Models/Weapons/M9/Handgun",
                0.0035f,
                'B')
            );
            SetWeaponObj(new PickupWeapon("AssaultRiflePickup",
                "Models/Weapons/PP19/PP19Model",
                0.05f,
                'C')
            );
            SetWeaponObj(new PickupWeapon("ArbiterPickup",
                "Models/Weapons/Arbiter/arbiter",
                1,
                'D')
            );
            SetWeaponObj(new PickupWeapon("BazookaPickup",
                "Models/Weapons/RPG/RPG",
                0.0025f,
                'E')
            );

            // Preset Effect Objects to be picked up
            SetEffectObj(new PickupEffect("Health Pack",
                "Models/Weapons/Knife/knife",
                0.5f,
                PickupEffect.Effect.Health)
            );
            SetEffectObj(new PickupEffect("Gravity Boost",
                "Models/Weapons/Arbiter/arbiter",
                2,
                PickupEffect.Effect.Gravity)
            );
            SetEffectObj(new PickupEffect("Speed Boost",
                "Models/Weapons/PP19/PP19Model",
                0.1f,
                PickupEffect.Effect.Speed)
            );

            // Essence of Phobia pickup
            SetWeaponObj(new PickupWeapon("MysteryPickup",
                "Models/Weapons/PP19/PP19Model",
                0.5f,
                'P')
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
