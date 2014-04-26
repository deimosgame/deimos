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
            return (n_weapontoken.ToString());
        }

        public string GenerateEffectToken()
        {
            return (n_effecttoken.ToString());
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
            PickupObject.State state,
            float respawn,
            Vector3 rotation = default(Vector3))
        {
            string t = GenerateEffectToken();
            n_effecttoken++;

            // tweaking the desired effect object to parameters
            PickupEffect e = Game.Objects.GetEffectObject(name);
            e.Position = position;
            e.Status = state;
            e.T_Respawn = respawn;
            e.Rotation = rotation;

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

            //to update all existing weapon objects in the world
            // according to the game
        public void Update(float dt)
        {
            foreach (KeyValuePair<string, PickupWeapon> thisWeapon in
                PickupWeapons)
            {
                if (thisWeapon.Value.Status == PickupObject.State.Active)
                {
                    thisWeapon.Value.CheckPickup();
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
                    thisEffect.Value.CheckPickup();
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
        }

    }
}
