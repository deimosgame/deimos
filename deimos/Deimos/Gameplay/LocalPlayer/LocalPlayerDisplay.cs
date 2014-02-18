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
        public void DisplayCurrentWeapon(Weapon.WeaponState state)
        {
            switch (state)
            {

                case Weapon.WeaponState.AtEase :
                    Matrix cameraWorld = Matrix.Invert(Game.Camera.View);

                    Matrix weaponWorld = cameraWorld;
                    weapon.Position = Game.ThisPlayer.Position;
                    weapon.WorldMatrix =
                        Matrix.CreateScale(Game.ThisPlayer.CurrentWeapon.W_Scaling) *
                        Matrix.CreateRotationY((float)Math.PI) *
                        weaponWorld *
                        Matrix.CreateTranslation(
                            (cameraWorld.Forward * Game.ThisPlayer.CurrentWeapon.W_Offset.X) +
                            (cameraWorld.Down * Game.ThisPlayer.CurrentWeapon.W_Offset.Y) +
                            (cameraWorld.Right * Game.ThisPlayer.CurrentWeapon.W_Offset.Z)
                        );
                    break;
                   
                case Weapon.WeaponState.Aiming :
                    break;

                case Weapon.WeaponState.Firing :
                    break;

                case Weapon.WeaponState.Reloading :
                    break;

                case Weapon.WeaponState.Switching :
                    break;
            }
        }
    }
}
