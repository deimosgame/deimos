using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    public class ObjectManager
    {
        // Attributes

            // Link to game:
        DeimosGame Game;
            // Lists to manage:
        private Dictionary<string, PickupWeapon> PickupWeapons =
            new Dictionary<string, PickupWeapon>();
        private Dictionary<string, PickupEffect> PickupEffects =
            new Dictionary<string, PickupEffect>();
            // for boost time handling
        private List<PickupEffect> ActiveEffects =
            new List<PickupEffect>();
            // for token management
        int n_weapontoken = 0;
        int n_effecttoken = 0;


        // Constructor

        public ObjectManager(DeimosGame game)
        {
            Game = game;
        }


        // Methods for world objects to be managed one by one

            // Generating tokens. tokens are like indexes in this instance
        public string GenerateWeaponToken()
        {
            string t = "w";
            t += n_weapontoken.ToString();
            return t;
        }

        public string GenerateEffectToken()
        {
            string t = "e";
            t += n_effecttoken.ToString();
            return t;
        }

            // Creating and inserting a unique world weapon object
            // according to parameters.
        public string AddWeapon(string name, int initial_ammo,
            Vector3 position, PickupObject.State state, float respawn,
            Vector3 rotation = default(Vector3))
        {
            string t = GenerateWeaponToken();
            n_weapontoken++;

            // tweaking the object according to parameters
            PickupWeapon w = Game.Objects.GetWeaponObject(name);
            w.Ammo = initial_ammo;
            w.Position = position;
            w.Status = state;
            w.T_Respawn = respawn;
            w.Rotation = rotation;

            PickupWeapons.Add(t, w);

            Game.SceneManager.ModelManager.LoadModel(t, w.Model,
                position, rotation, w.Scale, LevelModel.CollisionType.None);

            return t;
        }

            // Creating and inserting a unique world effect object
            // according to parameters
        public string AddEffect(string name,
            Vector3 position,
            PickupObject.State state, float intensity,
            float respawn,
            float duration = 0,
            Vector3 rotation = default(Vector3))
        {
            string t = GenerateEffectToken();
            n_effecttoken++;

            // tweaking the desired effect object to parameters
            PickupEffect e = Game.Objects.GetEffectObject(name);
            e.Position = position;
            e.Status = state;
            e.Intensity = intensity;
            e.T_Respawn = respawn;
            e.Rotation = rotation;
            e.E_Duration = duration;

            PickupEffects.Add(t, e);

            Game.SceneManager.ModelManager.LoadModel(t, e.Model,
                position, rotation, e.Scale, LevelModel.CollisionType.None);

            return t;
        }

            // returns the only weapon object associated with the token
        public PickupWeapon GetWeapon(string token)
        {
            return PickupWeapons[token];
        }

            // returns the only effect object associated with the tokem
        public PickupEffect GetEffect(string token)
        {
            return PickupEffects[token];
        }

            // if we are forced to remove said weapon object
            // this method does it, and takes into account the shift
            // in the tokens' numbering
        public void RemoveWeapon(string token)
        {
            if (PickupWeapons.ContainsKey(token))
            {
                PickupWeapons.Remove(token);
                n_weapontoken--;

                // unloading the models
                Game.SceneManager.ModelManager.RemoveLevelModel(token);
            }
        }

            // same, but for effect objects
        public void RemoveEffect(string token)
        {
            if (PickupEffects.ContainsKey(token))
            {
                PickupEffects.Remove(token);
                n_effecttoken--;

                //unloading the models
                Game.SceneManager.ModelManager.RemoveLevelModel(token);
            }
        }

        // Methods to iterate on all existing objects and treat them

            // to set the state of a weapon object
        public void SetStateWeapon(string token, PickupObject.State state)
        {
            if (PickupWeapons.ContainsKey(token))
            {
                PickupWeapons[token].Status = state;
                if (state == PickupObject.State.Inactive)
                {
                    PickupWeapons[token].respawn_timer = 0;
                }
            }
        }

            // to set the state of an effect object
        public void SetStateEffect(string token, PickupObject.State state)
        {
            if (PickupEffects.ContainsKey(token))
            {
                PickupEffects[token].Status = state;
                if (state == PickupObject.State.Inactive)
                {
                    PickupEffects[token].respawn_timer = 0;
                }
            }
        }

        // Effect treating methods

            // Treating the effects of a weapon pickup object
        public void TreatWeapon(PickupWeapon w)
        {
            if (Game.ThisPlayer.Inventory.Contains(
                Game.Weapons.GetName(w.Represents)))
            {
                if (Game.ThisPlayer.Inventory.IsAtMaxAmmo(
                    Game.Weapons.GetName(w.Represents)))
                {
                    return;
                }
                else
                {
                    // let's give the ammo to the player
                    Game.ThisPlayer.ammoPickup =
                        (uint)w.Ammo;
                    Game.ThisPlayer.Inventory.PickupAmmo(
                        Game.Weapons.GetName(w.Represents));

                    // let's deactivate the weapon object
                    w.Status = PickupObject.State.Inactive;

                    // and let's set its respawn timer to 0
                    w.respawn_timer = 0;
                }
            }
            else
            {
                // if the player does not have the weapon,
                // we will give it to them
                Game.ThisPlayer.Inventory.PickupWeapon(
                    Game.Weapons.GetWeapon(
                    Game.Weapons.GetName(w.Represents)));

                // and now the ammo for the new weapon
                Game.ThisPlayer.ammoPickup =
                    (uint)w.Ammo;
                Game.ThisPlayer.Inventory.PickupAmmo(
                    Game.Weapons.GetName(w.Represents));

                // let's now deactivate the weapon object
                w.Status = PickupObject.State.Inactive;

                // and the timer to 0
                w.respawn_timer = 0;
            }
        }

            // Treating the effects of an effect pickup object
        public void TreatEffect(PickupEffect e)
        {
            switch (e.O_Effect)
            {
                case PickupEffect.Effect.Health:
                    {
                        if (Game.ThisPlayer.IsAtMaxHealth())
                        {
                            return;
                        }
                        else
                        {
                            int addup = Game.ThisPlayer.Health +
                                (int)e.Intensity;

                            if (addup >= Game.ThisPlayer.M_Health)
                            {
                                Game.ThisPlayer.Health =
                                    (int)Game.ThisPlayer.M_Health;
                            }
                            else
                            {
                                Game.ThisPlayer.Health = addup;
                            }

                            e.Status = PickupObject.State.Inactive;
                            e.respawn_timer = 0;
                        }
                    }
                    break;

                case PickupEffect.Effect.Speed:
                    {
                        if (Game.ThisPlayer.Speedboosted)
                        {
                            return;
                        }
                        else
                        {
                            // Let's update the player's boolean
                            Game.ThisPlayer.Speedboosted = true;

                            // Let's add the effect to active effects list
                            ActiveEffects.Add(e);

                            // let's deactivate the world object
                            e.Status = PickupObject.State.Inactive;
                            e.respawn_timer = 0;

                            // let's give the boost to the player!
                            Game.ThisPlayer.Speed += e.Intensity;
                        }
                    }
                    break;

                case PickupEffect.Effect.Gravity:
                    if (Game.ThisPlayer.Gravityboosted)
                    {
                        return;
                    }
                    else
                    {
                        // Let's update the player's boolean
                        Game.ThisPlayer.Gravityboosted = true;

                        // Let's add the effect to the active effects list
                        ActiveEffects.Add(e);

                        // let's deactivate the world object
                        e.Status = PickupObject.State.Inactive;
                        e.respawn_timer = 0;

                        // let's boost the player!
                        Game.ThisPlayerPhysics.JumpVelocity += e.Intensity;
                    }
                    break;

                default:
                    break;
            }
        }

            //to update all existing weapon objects in the world
            // according to the game
        public void Update(float dt)
        {
            foreach (KeyValuePair<string, PickupWeapon> thisWeapon in
                PickupWeapons)
            {
                if (thisWeapon.Value.Status == PickupObject.State.Active)
                {

                }
                else
                {
                    if (thisWeapon.Value.respawn_timer >= thisWeapon.Value.T_Respawn)
                    {
                        // activating the respawned object
                        thisWeapon.Value.Status = PickupObject.State.Active;
                        
                        // showing once again the now active object
                        Game.SceneManager.ModelManager.GetLevelModel(
                            thisWeapon.Key).show = true;

                        // resetting its respawn timer
                        thisWeapon.Value.respawn_timer = 0;
                    }
                    else
                    {
                        // hiding the inactive object from the world
                        Game.SceneManager.ModelManager.GetLevelModel(
                            thisWeapon.Key).show = false;

                        // incrementing the respawn timer
                        thisWeapon.Value.respawn_timer += dt;
                    }
                }
            }

            foreach (KeyValuePair<string, PickupEffect> thisEffect in
                PickupEffects)
            {
                if (thisEffect.Value.Status == PickupObject.State.Active)
                {

                }
                else
                {
                    if (thisEffect.Value.respawn_timer >= thisEffect.Value.T_Respawn)
                    {
                        // activating the respawned object
                        thisEffect.Value.Status = PickupObject.State.Active;

                        // showing once again the now active object
                        Game.SceneManager.ModelManager.GetLevelModel(
                            thisEffect.Key).show = true;

                        // resetting its respawn timer
                        thisEffect.Value.respawn_timer = 0;
                    }
                    else
                    {
                        // hiding the inactive object from the world
                        Game.SceneManager.ModelManager.GetLevelModel(
                            thisEffect.Key).show = false;

                        // incrementing the respawn timer
                        thisEffect.Value.respawn_timer += dt;
                    }
                }
            }

            HandleBoostTimers(dt);
        }

        private void HandleBoostTimers(float dt)
        {
            List<PickupEffect> toBeRemoved =
                new List<PickupEffect>();

            foreach (PickupEffect effect in ActiveEffects)
            {
                switch (effect.O_Effect)
                {
                    case PickupEffect.Effect.Speed:
                        {
                            if (effect.t_effect >= effect.E_Duration)
                            {
                                // we reset the timer to 0
                                effect.t_effect = 0;

                                // we reset the player's speed
                                Game.ThisPlayer.Speed -= effect.Intensity;
                                
                                // since the effect is no longer active,
                                // we will remove it
                                toBeRemoved.Add(effect);

                                // and we set the player's boolean
                                Game.ThisPlayer.Speedboosted = false;
                            }
                            else
                            {
                                // if the effect is still running, we just 
                                // increment the timer
                                effect.t_effect += dt;
                            }
                        }
                        break;

                    case PickupEffect.Effect.Gravity:
                        {
                            if (effect.t_effect >= effect.E_Duration)
                            {
                                // reset the timer to 0
                                effect.t_effect = 0;

                                // resetting the player's gravity
                                Game.ThisPlayerPhysics.JumpVelocity -= effect.Intensity;

                                // we remove the effect since its gone
                                toBeRemoved.Add(effect);

                                // Resetting the player's boolean
                                Game.ThisPlayer.Gravityboosted = false;
                            }
                            else
                            {
                                // incremeting the timer if still boosted
                                effect.t_effect += dt;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            // removing the active effects to be removed
            foreach (PickupEffect e in toBeRemoved)
            {
                ActiveEffects.Remove(e);
            }
        }


    }
}
