using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class ModelAnimationManager
    {
        protected Dictionary<string, AnimatedModel> AnimatedModels = new Dictionary<string, AnimatedModel>();

        public AnimatedModel Add(string name, LevelModel model, int milliseconds, 
                            Vector3 endPos, Vector3 endRotation,
                            AnimationLoop loop = AnimationLoop.None)
        {
            AnimatedModel tAnimation = new AnimatedModel();
            tAnimation.Model = model;
            tAnimation.Milliseconds = milliseconds;
            tAnimation.RemainingMilliseconds = milliseconds;
            tAnimation.EndPosition = endPos;
            tAnimation.EndRotation = endRotation;
            tAnimation.Loop = loop;

            AnimatedModels.Add(name, tAnimation);

            return tAnimation;
        }

        public AnimatedModel Get(string name)
        {
            return AnimatedModels[name];
        }

        public void Animate(float dt)
        {
            List<string> elementsToDelete = new List<string>();
            foreach (KeyValuePair<string, AnimatedModel> item in AnimatedModels)
            {
                AnimatedModel tAnimation = item.Value;

                tAnimation.Model.Position = new Vector3(
                    tAnimation.Model.Position.X + tAnimation.CoeffPX,
                    tAnimation.Model.Position.Y + tAnimation.CoeffPY,
                    tAnimation.Model.Position.Z + tAnimation.CoeffPZ
                );
                tAnimation.Model.Rotation = new Vector3(
                    tAnimation.Model.Rotation.X + tAnimation.CoeffRX,
                    tAnimation.Model.Rotation.Y + tAnimation.CoeffRY,
                    tAnimation.Model.Rotation.Z + tAnimation.CoeffRZ
                );


                tAnimation.RemainingMilliseconds -= (int) (dt * 1000);

                if (tAnimation.RemainingMilliseconds <= 0)
                {
                    switch (tAnimation.Loop)
                    {
                        case AnimationLoop.Loop:
                            tAnimation.RemainingMilliseconds = tAnimation.Milliseconds;
                        break;
                        default:
                            elementsToDelete.Add(item.Key);
                        break;
                    }
                }
            }

            // Removing old animations
            foreach (string key in elementsToDelete)
            {
                AnimatedModels.Remove(key);
            }
        }
    }
}
