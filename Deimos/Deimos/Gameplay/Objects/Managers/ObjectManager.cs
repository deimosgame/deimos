﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    public class ObjectManager
    {
        // Attributes

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
            // mystery weapon management (eventual)
        private MysteryManager Mystery;


        // Constructor

        public ObjectManager()
        {
            //
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
            PickupWeapon w = GameplayFacade.Objects.GetWeaponObject(name);
            w.Manager = this;
            w.Ammo = initial_ammo;
            w.Position = position;
            w.Status = state;
            w.T_Respawn = respawn;
            w.Rotation = rotation;
            w.Token = t;

            PickupWeapons.Add(t, w);

            GeneralFacade.SceneManager.ModelManager.LoadModel(t, w.ModelPath,
                position, rotation, w.Scale, LevelModel.CollisionType.None,
                w);

            w.Model = GeneralFacade.SceneManager.ModelManager.GetLevelModel(t);

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
            PickupEffect e = GameplayFacade.Objects.GetEffectObject(name);
            e.Manager = this;
            e.Position = position;
            e.Status = state;
            e.Intensity = intensity;
            e.T_Respawn = respawn;
            e.Rotation = rotation;
            e.E_Duration = duration;
            e.Token = t;

            PickupEffects.Add(t, e);

            GeneralFacade.SceneManager.ModelManager.LoadModel(t, e.ModelPath,
                position, rotation, e.Scale, LevelModel.CollisionType.None,
                e);

            e.Model = GeneralFacade.SceneManager.ModelManager.GetLevelModel(t);

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
                GeneralFacade.SceneManager.ModelManager.RemoveLevelModel(token);
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
                GeneralFacade.SceneManager.ModelManager.RemoveLevelModel(token);
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
        public void TreatWeapon(PickupWeapon w, string token)
        {
            if (GameplayFacade.ThisPlayer.Inventory.Contains(
                GameplayFacade.Weapons.GetName(w.Represents)))
            {
                if (GameplayFacade.ThisPlayer.Inventory.IsAtMaxAmmo(
                    GameplayFacade.Weapons.GetName(w.Represents)))
                {
                    return;
                }
                else
                {
                    // let's give the ammo to the player
                    GameplayFacade.ThisPlayer.ammoPickup =
                        (uint)w.Ammo;
                    GameplayFacade.ThisPlayer.Inventory.PickupAmmo(
                        GameplayFacade.Weapons.GetName(w.Represents));

                    GeneralFacade.SceneManager.SoundManager.Play("w_pu");
                    if (!NetworkFacade.Local)
                    {
                        NetworkFacade.MainHandling.Sounds.SendWithPos(GeneralFacade.SceneManager.SoundManager.GetSoundByte("w_pu"),
                            GameplayFacade.ThisPlayer.Position);
                    }

                    // let's deactivate the weapon object
                    w.Status = PickupObject.State.Inactive;

                    // and let's set its respawn timer to 0
                    w.respawn_timer = 0;

                    // hiding the inactive object from the world
                    GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                        token).show = false;
                }
            }
            else
            {
                // if the player does not have the weapon,
                // we will give it to them
                GameplayFacade.ThisPlayer.Inventory.PickupWeapon(
                    GameplayFacade.Weapons.GetWeapon(
                    GameplayFacade.Weapons.GetName(w.Represents)));

                // and now the ammo for the new weapon
                GameplayFacade.ThisPlayer.ammoPickup =
                    (uint)w.Ammo;
                GameplayFacade.ThisPlayer.Inventory.PickupAmmo(
                    GameplayFacade.Weapons.GetName(w.Represents));

                GeneralFacade.SceneManager.SoundManager.Play("w_pu");
                if (!NetworkFacade.Local)
                {
                    NetworkFacade.MainHandling.Sounds.SendWithPos(GeneralFacade.SceneManager.SoundManager.GetSoundByte("w_pu"),
                        GameplayFacade.ThisPlayer.Position);
                }

                // let's now deactivate the weapon object
                w.Status = PickupObject.State.Inactive;

                // and the timer to 0
                w.respawn_timer = 0;

                // hiding the inactive object from the world
                GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                    token).show = false;
            }
        }

            // Treating the effects of an effect pickup object
        public void TreatEffect(PickupEffect e, string token)
        {
            switch (e.O_Effect)
            {
                case PickupEffect.Effect.Health:
                    {
                        if (GameplayFacade.ThisPlayer.IsAtMaxHealth())
                        {
                            return;
                        }
                        else
                        {
                            int addup = GameplayFacade.ThisPlayer.Health +
                                (int)e.Intensity;

                            if (addup >= GameplayFacade.ThisPlayer.M_Health)
                            {
                                GameplayFacade.ThisPlayer.Health =
                                    (int)GameplayFacade.ThisPlayer.M_Health;
                            }
                            else
                            {
                                GameplayFacade.ThisPlayer.Health = addup;
                            }

                            GeneralFacade.SceneManager.SoundManager.Play("heal");
                            if (!NetworkFacade.Local)
                            {
                                NetworkFacade.MainHandling.Sounds.SendWithPos(GeneralFacade.SceneManager.SoundManager.GetSoundByte("heal"),
                                    GameplayFacade.ThisPlayer.Position);
                            }

                            e.Status = PickupObject.State.Inactive;
                            e.respawn_timer = 0;

                            // hiding the inactive object from the world
                            GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                                token).show = false;
                        }
                    }
                    break;

                case PickupEffect.Effect.Speed:
                    {
                        if (GameplayFacade.ThisPlayer.Speedboosted)
                        {
                            return;
                        }
                        else
                        {
                            // Let's update the player's boolean
                            GameplayFacade.ThisPlayer.Speedboosted = true;

                            // Let's add the effect to active effects list
                            ActiveEffects.Add(e);

                            GeneralFacade.SceneManager.SoundManager.Play("speed");
                            if (!NetworkFacade.Local)
                            {
                                NetworkFacade.MainHandling.Sounds.SendWithPos(GeneralFacade.SceneManager.SoundManager.GetSoundByte("speed"),
                                    GameplayFacade.ThisPlayer.Position);
                            }

                            // let's deactivate the world object
                            e.Status = PickupObject.State.Inactive;
                            e.respawn_timer = 0;

                            // hiding the inactive object from the world
                            GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                                token).show = false;

                            // let's give the boost to the player!
                            GameplayFacade.ThisPlayer.Speed += e.Intensity;
                        }
                    }
                    break;

                case PickupEffect.Effect.Gravity:
                    if (GameplayFacade.ThisPlayer.Gravityboosted)
                    {
                        return;
                    }
                    else
                    {
                        // Let's update the player's boolean
                        GameplayFacade.ThisPlayer.Gravityboosted = true;

                        GeneralFacade.SceneManager.SoundManager.Play("gravity");
                        if (!NetworkFacade.Local)
                        {
                            NetworkFacade.MainHandling.Sounds.SendWithPos(GeneralFacade.SceneManager.SoundManager.GetSoundByte("gravity"),
                                GameplayFacade.ThisPlayer.Position);
                        }

                        // Let's add the effect to the active effects list
                        ActiveEffects.Add(e);

                        // let's deactivate the world object
                        e.Status = PickupObject.State.Inactive;
                        e.respawn_timer = 0;

                        // hiding the inactive object from the world
                        GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                            token).show = false;

                        // let's boost the player!
                        GameplayFacade.ThisPlayerPhysics.JumpVelocity += e.Intensity;
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
                    thisWeapon.Value.CheckCollision(thisWeapon.Value.Position);
                }
                else
                {
                    if (NetworkFacade.Local)
                    {
                        if (thisWeapon.Value.respawn_timer >= thisWeapon.Value.T_Respawn)
                        {
                            // activating the respawned object
                            thisWeapon.Value.Status = PickupObject.State.Active;

                            // showing once again the now active object
                            GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                                thisWeapon.Key).show = true;

                            // resetting its respawn timer
                            thisWeapon.Value.respawn_timer = 0;
                        }
                        else
                        {
                            // incrementing the respawn timer
                            thisWeapon.Value.respawn_timer += dt;
                        }
                    }
                }
            }

            foreach (KeyValuePair<string, PickupEffect> thisEffect in
                PickupEffects)
            {
                if (thisEffect.Value.Status == PickupObject.State.Active)
                {
                    thisEffect.Value.CheckCollision(thisEffect.Value.Position);
                }
                else
                {
                    if (NetworkFacade.Local)
                    {
                        if (thisEffect.Value.respawn_timer >= thisEffect.Value.T_Respawn)
                        {
                            // activating the respawned object
                            thisEffect.Value.Status = PickupObject.State.Active;

                            // showing once again the now active object
                            GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                                thisEffect.Key).show = true;

                            // resetting its respawn timer
                            thisEffect.Value.respawn_timer = 0;
                        }
                        else
                        {
                            // incrementing the respawn timer
                            thisEffect.Value.respawn_timer += dt;
                        }
                    }
                }
            }

            HandleBoostTimers(dt);
            UpdateMystery(dt);
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
                                GameplayFacade.ThisPlayer.Speed -= effect.Intensity;
                                
                                // since the effect is no longer active,
                                // we will remove it
                                toBeRemoved.Add(effect);

                                // and we set the player's boolean
                                GameplayFacade.ThisPlayer.Speedboosted = false;

                                GeneralFacade.SceneManager.SoundManager.Play("effectoff");
                                if (!NetworkFacade.Local)
                                {
                                    NetworkFacade.MainHandling.Sounds.SendWithPos(GeneralFacade.SceneManager.SoundManager.GetSoundByte("effectoff"),
                                        GameplayFacade.ThisPlayer.Position);
                                }
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
                                GameplayFacade.ThisPlayerPhysics.JumpVelocity -= effect.Intensity;

                                // we remove the effect since its gone
                                toBeRemoved.Add(effect);

                                // Resetting the player's boolean
                                GameplayFacade.ThisPlayer.Gravityboosted = false;

                                GeneralFacade.SceneManager.SoundManager.Play("effectoff");
                                if (!NetworkFacade.Local)
                                {
                                    NetworkFacade.MainHandling.Sounds.SendWithPos(GeneralFacade.SceneManager.SoundManager.GetSoundByte("effectoff"),
                                        GameplayFacade.ThisPlayer.Position);
                                }
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


        // Mystery weapon management

            // Instantiating a mystery weapon
        public void CreateMystery(List<Vector3> spawns, int t_min, int t_max,
            int initial_ammo, Vector3 rotation = default(Vector3))
        {
            Mystery = new MysteryManager(spawns,
                t_min, t_max, initial_ammo, rotation);
        }

        public void UpdateMystery(float dt)
        {
            if (Mystery != null)
            {
                Mystery.Update(dt);
            }
        }

        public PickupWeapon GetMysteryPickup()
        {
            return Mystery.MysteryWeapon;
        }

        public void SetMysteryRespawn()
        {
            Mystery.SetRespawn();
        }
    }
}
