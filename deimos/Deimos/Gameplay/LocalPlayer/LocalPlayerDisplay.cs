using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class LocalPlayerDisplay
    {
        LevelModel weapon;

        public LocalPlayerDisplay()
        {
            //
        }

        // To load the model of our current weapon when spawned
        public void LoadCurrentWeaponModel()
        {
            if (!GeneralFacade.Game.SceneManager.ModelManager.LevelModelExists(
                GeneralFacade.Game.ThisPlayer.CurrentWeapon.Name))
            {
                GeneralFacade.Game.SceneManager.ModelManager.LoadModel(
                    GeneralFacade.Game.ThisPlayer.CurrentWeapon.Name,
                    GeneralFacade.Game.ThisPlayer.CurrentWeapon.Path,
                    Vector3.Zero,
                    Vector3.Zero,
                    GeneralFacade.Game.ThisPlayer.CurrentWeapon.W_Scaling,
                    LevelModel.CollisionType.None
                );

                weapon = GeneralFacade.Game.SceneManager.ModelManager.GetLevelModel(
                        GeneralFacade.Game.ThisPlayer.CurrentWeapon.Name);
            }
        }

        // to switch between loadad weapon models according to player's will
        public void SetCurrentWeaponModel()
        {
            if (GeneralFacade.Game.SceneManager.ModelManager.LevelModelExists(
                GeneralFacade.Game.ThisPlayer.CurrentWeapon.Name))
            {
                weapon = GeneralFacade.Game.SceneManager.ModelManager.GetLevelModel(
                    GeneralFacade.Game.ThisPlayer.CurrentWeapon.Name);
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
            if (GeneralFacade.Game.SceneManager.ModelManager.LevelModelExists("Pistol"))
            {
                GeneralFacade.Game.SceneManager.ModelManager.RemoveLevelModel(
                "Pistol");
            }
            if (GeneralFacade.Game.SceneManager.ModelManager.LevelModelExists("Assault Rifle"))
            {
                GeneralFacade.Game.SceneManager.ModelManager.RemoveLevelModel(
                "Assault Rifle");
            }
            if (GeneralFacade.Game.SceneManager.ModelManager.LevelModelExists("Bazooka"))
            {
                GeneralFacade.Game.SceneManager.ModelManager.RemoveLevelModel(
                "Bazooka");
            }
            if (GeneralFacade.Game.SceneManager.ModelManager.LevelModelExists("Arbiter"))
            {
                GeneralFacade.Game.SceneManager.ModelManager.RemoveLevelModel(
                    "Arbiter");
            }
            if (GeneralFacade.Game.SceneManager.ModelManager.LevelModelExists("Carver"))
            {
                GeneralFacade.Game.SceneManager.ModelManager.RemoveLevelModel(
                    "Carver");
            }
        }

        // To get proper weapon display, attached to player
        public void DisplayCurrentWeapon(Weapon wep)
        {
            Vector3 Offset = Vector3.Zero;

            if (wep.AimState == AimState.True)
            {
                Offset = GeneralFacade.Game.ThisPlayer.CurrentWeapon.W_Offset_Aim;
            }
            else
            {
                switch (wep.State)
                {
                    case WeaponState.AtEase:
                        Offset = GeneralFacade.Game.ThisPlayer.CurrentWeapon.W_Offset;
                        break;

                    case WeaponState.Firing:
                        Offset = GeneralFacade.Game.ThisPlayer.CurrentWeapon.W_Offset;
                        break;

                    case WeaponState.Reloading:
                        HideWeapon();
                        break;

                    case WeaponState.Switching:
                        HideWeapon();
                        break;
                }
            }

            Matrix cameraWorld = Matrix.Invert(GeneralFacade.Game.Camera.View);

            Matrix weaponWorld = cameraWorld;
            weapon.Position = GeneralFacade.Game.ThisPlayer.Position;
            weapon.WorldMatrix =
                Matrix.CreateScale(GeneralFacade.Game.ThisPlayer.CurrentWeapon.W_Scaling) *
                Matrix.CreateRotationY(GeneralFacade.Game.ThisPlayer.CurrentWeapon.W_Direct) *
                weaponWorld *
                Matrix.CreateTranslation(
                    (cameraWorld.Forward * Offset.X) +
                    (cameraWorld.Down * Offset.Y) +
                    (cameraWorld.Right * Offset.Z)
                );
        }

        public void HideWeapon()
        {
            weapon.show = false;
        }

        public void ShowWeapon()
        {
            weapon.show = true;
        }
    }
}
