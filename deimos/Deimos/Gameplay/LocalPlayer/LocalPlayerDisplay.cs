using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Deimos
{
    class LocalPlayerDisplay
    {
        DeimosGame Game;

        private Vector3 WeaponPosition;

        private Vector3 dl = new Vector3(1.3f, 0.9f, 1.3f);

        private float dx;
        private float dy;

        public LocalPlayerDisplay(DeimosGame game)
        {
            Game = game;
        }

        private void SetWepRotation()
        {
            Vector3 t = Game.ThisPlayer.Rotation;
            t.Z = Game.ThisPlayer.Position.Z;
            Game.SceneManager.ModelManager.GetLevelModel("PP19").Rotation = Game.ThisPlayer.Rotation;

        }

        public void CatchRotationDl(float dlX, float dlY)
        {
            dx = dlX;
            dy = dlY;
        }

        //private void SetWepPosition()
        //{
        //    //if (Game.ThisPlayer.Rotation.Y < 0f)
        //    //{
        //    //    WeaponPosition.X += Game.ThisPlayer.Rotation.Y;
        //    //    WeaponPosition.Z += Game.ThisPlayer.Rotation.Y;
        //    //}
        //    //else if (Game.ThisPlayer.Rotation.Y > 0f)
        //    //{
        //    //    WeaponPosition.X += Game.ThisPlayer.Rotation.Y;
        //    //    WeaponPosition.Z += Game.ThisPlayer.Rotation.Y;
        //    //}

        //    //if (Game.ThisPlayer.Rotation.X < 0f)
        //    //{
        //    //    WeaponPosition.Y += Math.Abs(Game.ThisPlayer.Rotation.X);
        //    //}
        //    //else if (Game.ThisPlayer.Rotation.X > 0f)
        //    //{
        //    //    WeaponPosition.Y -= Game.ThisPlayer.Rotation.X;
        //    //}

        //    WeaponPosition.X += dx;
        //    WeaponPosition.Z += dx;

        //    WeaponPosition.Y += dy;

        //    if (Game.ThisPlayer.Rotation.Y < 0f)
        //    {
        //        WeaponPosition.X *= -dl.X;
        //        WeaponPosition.Z *= -dl.Z;

        //        if (Game.ThisPlayer.Rotation.X > 0f) 
        //        {
        //            WeaponPosition.Y *= -dl.Y;
        //        }
        //        else if (Game.ThisPlayer.Rotation.X < 0f)
        //        {
        //            WeaponPosition.Y *= dl.Y;
        //        }

        //        Game.SceneManager.ModelManager.GetLevelModel("PP19").Position = WeaponPosition;
        //    }

        //    else if (Game.ThisPlayer.Rotation.Y > 0f)
        //    {
        //        WeaponPosition.X *= dl.X;
        //        WeaponPosition.Z *= dl.Z;

        //        if (Game.ThisPlayer.Rotation.X > 0f)
        //        {
        //            WeaponPosition.Y *= -dl.Y;
        //        }
        //        else if (Game.ThisPlayer.Rotation.X < 0f)
        //        {
        //            WeaponPosition.Y *= -dl.Y;
        //        }

        //        Game.SceneManager.ModelManager.GetLevelModel("PP19").Position = WeaponPosition;

        //    }
            
        //}
        
        public void Update()
        {//   
        //    Vector3 t = new Vector3();
        //    t = Game.ThisPlayer.Position;
        //WeaponPosition = Vector3.Add(t, dl);
        

        //Game.SceneManager.ModelManager.GetLevelModel("PP19").Position = WeaponPosition;

            SetWepRotation();

            //SetWepPosition();

        }
    }
}
