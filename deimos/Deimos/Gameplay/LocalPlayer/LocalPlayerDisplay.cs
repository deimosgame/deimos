using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class LocalPlayerDisplay
    {
        DeimosGame Game;

        LevelModel weapon;

        public LocalPlayerDisplay(DeimosGame game)
        {
            Game = game;
        }

        // To load the model of our current weapon when spawned
        public void LoadCurrentWeaponModel()
        {
            if (!Game.SceneManager.ModelManager.LevelModelExists(
                Game.ThisPlayer.CurrentWeapon.Name))
            {
                Game.SceneManager.ModelManager.LoadModel(
                    Game.ThisPlayer.CurrentWeapon.Name,
                    Game.ThisPlayer.CurrentWeapon.Path,
                    Vector3.Zero,
                    Vector3.Zero,
                    Game.ThisPlayer.CurrentWeapon.W_Scaling,
                    LevelModel.CollisionType.None
                );

                weapon = Game.SceneManager.ModelManager.GetLevelModel(
                        Game.ThisPlayer.CurrentWeapon.Name);
            }
        }

        // to switch between loadad weapon models according to player's will
        public void SetCurrentWeaponModel()
        {
            if (Game.SceneManager.ModelManager.LevelModelExists(
                Game.ThisPlayer.CurrentWeapon.Name))
            {
                weapon = Game.SceneManager.ModelManager.GetLevelModel(
                    Game.ThisPlayer.CurrentWeapon.Name);
            }
            else
            {
                LoadCurrentWeaponModel();
            }
        }

        // To unload our weapon when it is no longer the current one
        public void UnloadCurrentWeaponModel()
        {
           
        }

        // when dying, unloading all weapon models
        public void UnloadAllWeapons()
        {
            if (Game.SceneManager.ModelManager.LevelModelExists("Pistol"))
            {
            Game.SceneManager.ModelManager.RemoveLevelModel(
                "Pistol");
            }
            if (Game.SceneManager.ModelManager.LevelModelExists("Assault Rifle"))
            {
            Game.SceneManager.ModelManager.RemoveLevelModel(
                "Assault Rifle");
            }
            if (Game.SceneManager.ModelManager.LevelModelExists("Bazooka"))
            {
            Game.SceneManager.ModelManager.RemoveLevelModel(
                "Bazooka");
            }
            if (Game.SceneManager.ModelManager.LevelModelExists("Arbiter"))
            {
                Game.SceneManager.ModelManager.RemoveLevelModel(
                    "Arbiter");
            }
            if (Game.SceneManager.ModelManager.LevelModelExists("Carver"))
            {
                Game.SceneManager.ModelManager.RemoveLevelModel(
                    "Carver");
            }
        }

        // To get proper weapon display, attached to player
        public void DisplayCurrentWeapon(Weapon wep)
        {
            Vector3 Offset = Vector3.Zero;

            if (wep.AimState == AimState.True)
            {
                Offset = Game.ThisPlayer.CurrentWeapon.W_Offset_Aim;
            }
            else
            {
                switch (wep.State)
                {
                    case WeaponState.AtEase:
                        Offset = Game.ThisPlayer.CurrentWeapon.W_Offset;
                        break;

                    case WeaponState.Firing:
                        break;

                    case WeaponState.Reloading:
                        break;

                    case WeaponState.Switching:
                        break;
                }
            }

            Matrix cameraWorld = Matrix.Invert(Game.Camera.View);

            Matrix weaponWorld = cameraWorld;
            weapon.Position = Game.ThisPlayer.Position;
            weapon.WorldMatrix =
                Matrix.CreateScale(Game.ThisPlayer.CurrentWeapon.W_Scaling) *
                Matrix.CreateRotationY(Game.ThisPlayer.CurrentWeapon.W_Direct) *
                weaponWorld *
                Matrix.CreateTranslation(
                    (cameraWorld.Forward * Offset.X) +
                    (cameraWorld.Down * Offset.Y) +
                    (cameraWorld.Right * Offset.Z)
                );
        }
    }
}
