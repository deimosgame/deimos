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
            Game.SceneManager.ModelManager.LoadModel(
                Game.ThisPlayer.CurrentWeapon.Name,
                Game.ThisPlayer.CurrentWeapon.Path,
                Vector3.Zero,
                Vector3.Zero,
                Game.ThisPlayer.CurrentWeapon.W_Scaling,
                LevelModel.CollisionType.None
            );

            weapon = Game.SceneManager.ModelManager.GetLevelModel("Assault Rifle");
        }

        // To unload our weapon when it is no longer the current one
        public void UnloadCurrentWeaponModel()
        {
            
        }

        // To get proper weapon display, attached to player
        public void DisplayCurrentWeapon(WeaponState state)
        {
            Vector3 Offset = Vector3.Zero;

            switch (state)
            {
                case WeaponState.AtEase :
                    Offset = Game.ThisPlayer.CurrentWeapon.W_Offset;
                    break;
                   
                case WeaponState.Aiming :
                    Offset = Vector3.Zero;
                    break;

                case WeaponState.Firing :
                    break;

                case WeaponState.Reloading :
                    break;

                case WeaponState.Switching :
                    break;
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
