using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class Display
    {
        LevelModel weapon;

        public Display()
        {
            //
        }

        // To load the model of our current weapon when spawned
        public void LoadCurrentWeaponModel()
        {
            if (!GeneralFacade.SceneManager.ModelManager.LevelModelExists(
                GameplayFacade.ThisPlayer.CurrentWeapon.Name))
            {
                GeneralFacade.SceneManager.ModelManager.LoadModel(
                    GameplayFacade.ThisPlayer.CurrentWeapon.Name,
                    GameplayFacade.ThisPlayer.CurrentWeapon.Path,
                    Vector3.Zero,
                    Vector3.Zero,
                    GameplayFacade.ThisPlayer.CurrentWeapon.W_Scaling,
                    LevelModel.CollisionType.None
                );

                weapon = GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                        GameplayFacade.ThisPlayer.CurrentWeapon.Name);
            }
        }

        // to switch between loadad weapon models according to player's will
        public void SetCurrentWeaponModel()
        {
            if (GeneralFacade.SceneManager.ModelManager.LevelModelExists(
                GameplayFacade.ThisPlayer.CurrentWeapon.Name))
            {
                weapon = GeneralFacade.SceneManager.ModelManager.GetLevelModel(
                    GameplayFacade.ThisPlayer.CurrentWeapon.Name);
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
            if (GeneralFacade.SceneManager.ModelManager.LevelModelExists("Pistol"))
            {
                GeneralFacade.SceneManager.ModelManager.RemoveLevelModel(
                "Pistol");
            }
            if (GeneralFacade.SceneManager.ModelManager.LevelModelExists("Assault Rifle"))
            {
                GeneralFacade.SceneManager.ModelManager.RemoveLevelModel(
                "Assault Rifle");
            }
            if (GeneralFacade.SceneManager.ModelManager.LevelModelExists("Bazooka"))
            {
                GeneralFacade.SceneManager.ModelManager.RemoveLevelModel(
                "Bazooka");
            }
            if (GeneralFacade.SceneManager.ModelManager.LevelModelExists("Arbiter"))
            {
                GeneralFacade.SceneManager.ModelManager.RemoveLevelModel(
                    "Arbiter");
            }
            if (GeneralFacade.SceneManager.ModelManager.LevelModelExists("Carver"))
            {
                GeneralFacade.SceneManager.ModelManager.RemoveLevelModel(
                    "Carver");
            }
        }

        // To get proper weapon display, attached to player
        public void DisplayCurrentWeapon(Weapon wep)
        {
            Vector3 Offset = Vector3.Zero;

            if (wep.AimState == AimState.True)
            {
                Offset = GameplayFacade.ThisPlayer.CurrentWeapon.W_Offset_Aim;
            }
            else
            {
                switch (wep.State)
                {
                    case WeaponState.AtEase:
                        Offset = GameplayFacade.ThisPlayer.CurrentWeapon.W_Offset;
                        break;

                    case WeaponState.Firing:
                        Offset = GameplayFacade.ThisPlayer.CurrentWeapon.W_Offset;
                        break;

                    case WeaponState.Reloading:
                        HideWeapon();
                        break;

                    case WeaponState.Switching:
                        HideWeapon();
                        break;
                }
            }

            Matrix cameraWorld = Matrix.Invert(DisplayFacade.Camera.View);

            Matrix weaponWorld = cameraWorld;
            weapon.Position = GameplayFacade.ThisPlayer.Position;
            weapon.WorldMatrix =
                Matrix.CreateScale(GameplayFacade.ThisPlayer.CurrentWeapon.W_Scaling) *
                Matrix.CreateRotationY(GameplayFacade.ThisPlayer.CurrentWeapon.W_Direct) *
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

        public void UpdateDisplay()
        {
            if (GameplayFacade.ThisPlayer.IsAlive())
            {
                DisplayFacade.ScreenElementManager.GetText("life").Text =
                    GameplayFacade.ThisPlayer.Health + "/" + GameplayFacade.ThisPlayer.M_Health.ToString();
                DisplayFacade.ScreenElementManager.GetText("ammo").Text =
                    GameplayFacade.ThisPlayer.CurrentWeapon.c_chamberAmmo + "/"
                    + GameplayFacade.ThisPlayer.CurrentWeapon.c_reservoirAmmo;
                DisplayFacade.ScreenElementManager.GetText("current-class").Text =
                    GameplayFacade.ThisPlayer.GetClassName();
            }
        }
    }
}
