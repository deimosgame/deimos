using CollidableModel;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    abstract class Collidable
    {

        // Attributes
        protected Vector3 Dimensions;

        public Collidable(Vector3 dimensions)
        {
            Dimensions = dimensions;
        }

        private BoundingBox GenerateBox(Vector3 position, Vector3 dimension)
        {
            Vector3 bbTop = new Vector3(
                position.X - (dimension.X / 2),
                position.Y - (dimension.Y / 5) * 3,
                position.Z - (dimension.Z / 2)
            );
            Vector3 bbBottom = new Vector3(
                position.X + (dimension.X / 2),
                position.Y + (dimension.Y / 5) * 2,
                position.Z + (dimension.Z / 2)
            );
            return new BoundingBox(
                bbTop,
                bbBottom
            );
        }

        private bool PreCollisionBypass()
        {
            if (GeneralFacade.Game.CurrentPlayingState == DeimosGame.PlayingStates.NoClip)
            {
                return true;
            }

            return false;
        }


        // Methods
        public Boolean CheckCollision(Vector3 position)
        {
            if (PreCollisionBypass())
            {
                return false;
            }

            // Creating the sphere of the camera for later collisions checks
            BoundingBox cameraBox = GenerateBox(position, Dimensions);
            Vector3 bbTop = cameraBox.Min;
            Vector3 bbBottom = cameraBox.Max;

            // And finally with our models collisions
            foreach (CollisionElement thisElement in
                GeneralFacade.SceneManager.CollisionManager.GetElements())
            {
                bool isCollision = false;
                switch (thisElement.ElementType)
                {
                    case CollisionElement.CollisionType.Box:
                        if (thisElement.Box.Contains(cameraBox) !=
                            ContainmentType.Disjoint)
                        {
                            isCollision = true;
                        }
                        break;
                    case CollisionElement.CollisionType.Sphere:
                        if (thisElement.Sphere.Contains(cameraBox) !=
                            ContainmentType.Disjoint)
                        {
                            isCollision = true;
                        }
                        break;
                    case CollisionElement.CollisionType.Model:
                        if (thisElement.Model.CollisionDetection ==
                            LevelModel.CollisionType.None)
                        {
                            continue;
                        }
                        var collidingFaces = new LinkedList<Face>();
                        var collisionPoints = new LinkedList<Vector3>();
                        // This method is used with pointer, so it does change 
                        // our above faces and points
                        thisElement.Model.CollisionModel.collisionData.collisions(
                            new BoundingBox(
                                (bbTop - thisElement.Model.Position) / thisElement.Model.Scale,
                                (bbBottom - thisElement.Model.Position) / thisElement.Model.Scale
                            ),
                            collidingFaces,
                            collisionPoints
                        );
                        if (collidingFaces.Count > 0 || collisionPoints.Count > 0)
                        {
                            isCollision = true;
                        }
                        break;
                }
                if (isCollision)
                {
                    thisElement.Event(thisElement, GeneralFacade.Game);
                    DisplayFacade.DebugScreen.Debug("             collision");
                    return true;
                }
            }

            DisplayFacade.DebugScreen.Debug("no collision");

            // If we're here, then no collision has been matched
            return false;
        }

    }
}
