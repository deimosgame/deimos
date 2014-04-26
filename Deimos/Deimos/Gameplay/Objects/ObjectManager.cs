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
        public string AddWeapon(string name, string path, 
            string weaponToken, int initial_ammo,
            Vector3 position, PickupObject.State state, float respawn,
            Vector3 rotation = default(Vector3))
        {
            string t = GenerateWeaponToken();
            n_weapontoken++;

            PickupWeapons.Add(t, new PickupWeapon(
                name, path, position, t, weaponToken, initial_ammo,
                state, respawn, rotation));

            Game.SceneManager.ModelManager.LoadModel(t, path,
                position, rotation, 1, LevelModel.CollisionType.None);

            return t;
        }

            // Creating and inserting a unique world effect object
            // according to parameters
        public string AddEffect(string name, string path,
            Vector3 position, PickupEffect.Effect effect,
            PickupObject.State state, float respawn,
            Vector3 rotation = default(Vector3))
        {
            string t = GenerateEffectToken();
            n_effecttoken++;

            PickupEffects.Add(t, new PickupEffect(
                name, path, position, effect, t, state, respawn, rotation));

            Game.SceneManager.ModelManager.LoadModel(t, path,
                position, rotation, 1, LevelModel.CollisionType.None);

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
                    // Manu can you help me complete this code that lets
                    // players pick up the object?
                    // Perhaps a method in the PickupWeapon class
                    // that handles it, and triggers the Inventory
                }
                else
                {
                    if (thisWeapon.Value.respawn_timer >= thisWeapon.Value.T_Respawn)
                    {
                        // activating the respawned object
                        thisWeapon.Value.Status = PickupObject.State.Active;
                        
                        // showing once again the now active object
                        Game.SceneManager.ModelManager.GetLevelModel(
                            thisWeapon.Value.Token).show = true;

                        // resetting its respawn timer
                        thisWeapon.Value.respawn_timer = 0;
                    }
                    else
                    {
                        // hiding the inactive object from the world
                        Game.SceneManager.ModelManager.GetLevelModel(
                            thisWeapon.Value.Token).show = false;

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
                    // same thing for these effects
                }
                else
                {
                    if (thisEffect.Value.respawn_timer >= thisEffect.Value.T_Respawn)
                    {
                        // activating the respawned object
                        thisEffect.Value.Status = PickupObject.State.Active;

                        // showing once again the now active object
                        Game.SceneManager.ModelManager.GetLevelModel(
                            thisEffect.Value.Token).show = true;

                        // resetting its respawn timer
                        thisEffect.Value.respawn_timer = 0;
                    }
                    else
                    {
                        // hiding the inactive object from the world
                        Game.SceneManager.ModelManager.GetLevelModel(
                            thisEffect.Value.Token).show = false;

                        // incrementing the respawn timer
                        thisEffect.Value.respawn_timer += dt;
                    }
                }
            }
        }

    }
}
