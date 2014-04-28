using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CollidableModel;


namespace Deimos
{
    class LocalPlayerCollision
    {
        // Attributes
        public Vector3 PlayerDimension;

        List<CollisionElement> CollisionElements =
            new List<CollisionElement>();


        // Constructor
        public LocalPlayerCollision(float playerHeight, float playerWidth, float playerDepth)
        {
            // These dimensions will be used to check for the camera collision:
            // a player/human isn't a cube but a box; taller than larger
            PlayerDimension.X = playerWidth;
            PlayerDimension.Y = playerHeight;
            PlayerDimension.Z = playerDepth;
        }


        // Methods
        public void AddCollisionBox(Vector3 coords1, Vector3 coords2, 
            Action<CollisionElement, DeimosGame> onCollision)
        {
            CollisionElement element = new CollisionElement();
            element.Box = new BoundingBox(coords1, coords2);
            element.ElementType = CollisionElement.CollisionType.Box;
            element.Event = onCollision;
            CollisionElements.Add(element);
        }
        // Adding a box directly helps for the ModelManager class, as we're
        // Creating a box directly from its methods when loading a model
        public void AddCollisionBoxDirectly(BoundingBox box,
            Action<CollisionElement, DeimosGame> onCollision)
        {
            CollisionElement element = new CollisionElement();
            element.Box = box;
            element.ElementType = CollisionElement.CollisionType.Box;
            element.Event = onCollision;
            CollisionElements.Add(element);
        }

        public void AddCollisionSphere(Vector3 coords, float radius,
            Action<CollisionElement, DeimosGame> onCollision)
        {
            CollisionElement element = new CollisionElement();
            element.Sphere = new BoundingSphere(coords, radius);
            element.ElementType = CollisionElement.CollisionType.Sphere;
            element.Event = onCollision;
            CollisionElements.Add(element);
        }
        // Same here
        public void AddCollisionSphereDirectly(BoundingSphere sphere,
            Action<CollisionElement, DeimosGame> onCollision)
        {
            CollisionElement element = new CollisionElement();
            element.Sphere = sphere;
            element.ElementType = CollisionElement.CollisionType.Sphere;
            element.Event = onCollision;
            CollisionElements.Add(element);
        }

        public void AddLevelModel(LevelModel model,
            Action<CollisionElement, DeimosGame> onCollision)
        {
            CollisionElement element = new CollisionElement();
            element.Model = model;
            element.ElementType = CollisionElement.CollisionType.Model;
            element.Event = onCollision;
            CollisionElements.Add(element);
        }


        public Boolean CheckCollision(Vector3 cameraPosition)
        {
            if (GeneralFacade.Game.CurrentPlayingState == DeimosGame.PlayingStates.NoClip)
            {
                return false;
            }

            // Creating the sphere of the camera for later collisions checks
            Vector3 bbTop = new Vector3(
                cameraPosition.X - (PlayerDimension.X / 2),
                cameraPosition.Y - (PlayerDimension.Y / 5) * 3,
                cameraPosition.Z - (PlayerDimension.Z / 2)
            );
            Vector3 bbBottom = new Vector3(
                cameraPosition.X + (PlayerDimension.X / 2),
                cameraPosition.Y + (PlayerDimension.Y / 5) * 2,
                cameraPosition.Z + (PlayerDimension.Z / 2)
            );
            BoundingBox cameraBox = new BoundingBox(
                bbTop,
                bbBottom
            );

            // And finally with our models collisions
            foreach (CollisionElement thisElement in 
                CollisionElements)
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
                    return true;
                }
            }

            // If we're here, then no collision has been matched
            return false;
        }

    }
}
