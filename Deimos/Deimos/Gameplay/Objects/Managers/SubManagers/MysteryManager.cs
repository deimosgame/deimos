using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    public class MysteryManager
    {
        // ATTRIBUTES

            // The world object for the mystery weapon
        public PickupWeapon MysteryWeapon;

            // Different Spawn Locations
        List<Vector3> SpawnLocations =
            new List<Vector3>();
            // number of spawn locations
        int n_spawn;

            // Minimum respawn timer in minutes
        int T_MinSpawn;

            // Maximum respawn timer in minutes
        int T_MaxSpawn;

            // next respawn time
        float T_NextSpawn;

            // respawn timer
        float respawn_t = 0;

        // CONSTRUCTOR

        public MysteryManager(List<Vector3> spawns,
            int minimumspawn, int maximumspawn,
            int initial_ammo, Vector3 rotation)
        {
            SpawnLocations = spawns;
            n_spawn = SpawnLocations.Count;

            MysteryWeapon = GeneralFacade.Game.Objects.GetWeaponObject("MysteryPickup");
            MysteryWeapon.Ammo = initial_ammo;
            MysteryWeapon.Rotation = rotation;
            MysteryWeapon.Status = PickupObject.State.Inactive;

            T_MaxSpawn = maximumspawn;
            T_MinSpawn = minimumspawn;

            SetRespawn();

            GeneralFacade.SceneManager.ModelManager.LoadModel(
                MysteryWeapon.Name,
                MysteryWeapon.Model,
                MysteryWeapon.Position,
                MysteryWeapon.Rotation,
                MysteryWeapon.Scale,
                LevelModel.CollisionType.None
            );
            GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                MysteryWeapon.Name).show = false;
        }

        // METHODS

            // update method
        public void Update(float dt)
        {
            if (MysteryWeapon.Status == PickupObject.State.Active)
            {

            }
            else
            {
                if (respawn_t >= T_NextSpawn)
                {
                    // respawns
                    Spawn();
                }
                else
                {
                    // incrementing the respawn timer
                    respawn_t += dt;
                }
            }
        }

            // spawn method
        public void Spawn()
        {
            MysteryWeapon.Status = PickupObject.State.Active;
            respawn_t = 0;

            // showing once again the now active object
            GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                MysteryWeapon.Name).Position = MysteryWeapon.Position;
            GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                MysteryWeapon.Name).show = true;
        }

            // to get next respawn timer
        public void SetRespawn()
        {
            Random rng = new Random();

            int max_interval = (T_MaxSpawn - T_MinSpawn) * 2;
            int x = rng.Next(0, max_interval + 1);
            T_NextSpawn = T_MinSpawn + (x * 0.5f);

            x = rng.Next(0, n_spawn);
            MysteryWeapon.Position = SpawnLocations[x];
        }
    }
}
