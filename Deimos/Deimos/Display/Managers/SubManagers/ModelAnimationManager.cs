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
                            Vector3 startPos, Vector3 endPos,
                            Vector3 startRotation, Vector3 endRotation,
                            bool loop = false)
        {
            AnimatedModel tAnimation = new AnimatedModel();
            tAnimation.Model = model;
            tAnimation.Milliseconds = milliseconds;
            tAnimation.RemainingMilliseconds = milliseconds;
            tAnimation.StartPosition = startPos;
            tAnimation.EndPosition = endPos;
            tAnimation.StartRotation = startRotation;
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

                float coeffPX = (tAnimation.EndPosition.X - tAnimation.StartPosition.X) / tAnimation.Milliseconds;
                float coeffPY = (tAnimation.EndPosition.Y - tAnimation.StartPosition.Y) / tAnimation.Milliseconds;
                float coeffPZ = (tAnimation.EndPosition.Z - tAnimation.StartPosition.Z) / tAnimation.Milliseconds;

                float coeffRX = (tAnimation.EndRotation.X - tAnimation.StartRotation.X) / tAnimation.Milliseconds;
                float coeffRY = (tAnimation.EndRotation.Y - tAnimation.StartRotation.Y) / tAnimation.Milliseconds;
                float coeffRZ = (tAnimation.EndRotation.Z - tAnimation.StartRotation.Z) / tAnimation.Milliseconds;

                tAnimation.Model.Position = new Vector3(
                    tAnimation.Model.Position.X + coeffPX,
                    tAnimation.Model.Position.Y + coeffPY,
                    tAnimation.Model.Position.Z + coeffPZ
                );
                tAnimation.Model.Rotation = new Vector3(
                    tAnimation.Model.Rotation.X + coeffRX,
                    tAnimation.Model.Rotation.Y + coeffRY,
                    tAnimation.Model.Rotation.Z + coeffRZ
                );


                tAnimation.RemainingMilliseconds -= (int) (dt * 1000);

                if (tAnimation.RemainingMilliseconds <= 0)
                {
                    if (tAnimation.Loop)
                    {
                        Vector3 temp = tAnimation.StartPosition;
                        tAnimation.StartPosition = tAnimation.EndPosition;
                        tAnimation.EndPosition = temp;

                        temp = tAnimation.StartRotation;
                        tAnimation.StartRotation = tAnimation.EndRotation;
                        tAnimation.EndRotation = temp;

                        tAnimation.RemainingMilliseconds = tAnimation.Milliseconds;

                        // prevent from adding it to the delete list
                        continue;
                    }
                    elementsToDelete.Add(item.Key);
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
