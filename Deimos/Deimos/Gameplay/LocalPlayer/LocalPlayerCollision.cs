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
        private DeimosGame MainGame;

        private Vector3 PlayerDimension;

        List<BoundingBox> CollisionBoxes = 
            new List<BoundingBox>();

        List<BoundingSphere> CollisionSpheres = 
            new List<BoundingSphere>();


        // Constructor
        public LocalPlayerCollision(float playerHeight, float playerWidth, float playerDepth,
            DeimosGame game)
        {
            // These dimensions will be used to check for the camera collision:
            // a player/human isn't a cube but a box; taller than larger
            PlayerDimension.X = playerWidth;
            PlayerDimension.Y = playerHeight;
            PlayerDimension.Z = playerDepth;

            MainGame = game;
        }


        // Methods
        public void AddCollisionBox(Vector3 coords1, Vector3 coords2, 
            float scale)
        {
            Vector3[] boxPoints = new Vector3[2];
            boxPoints[0] = coords1 * scale;
            boxPoints[1] = coords2 * scale;
            CollisionBoxes.Add(BoundingBox.CreateFromPoints(boxPoints));
        }
        // Adding a box directly helps for the ModelManager class, as we're
        // Creating a box directly from its methods when loading a model
        public void AddCollisionBoxDirectly(BoundingBox box)
        {
            CollisionBoxes.Add(box);
        }

        public void AddCollisionSphere(Vector3 coords, float radius)
        {
            CollisionSpheres.Add(new BoundingSphere(coords, radius));
        }
        // Same here
        public void AddCollisionSphereDirectly(BoundingSphere sphere)
        {
            CollisionSpheres.Add(sphere);
        }


        public Boolean CheckCollision(Vector3 cameraPosition)
        {
            if (MainGame.CurrentPlayingState == DeimosGame.PlayingStates.NoClip)
            {
                return false;
            }

            // Creating the sphere of the camera for later collisions checks
            BoundingBox cameraBox = new BoundingBox(
                new Vector3(
                    cameraPosition.X - (PlayerDimension.X / 2),
                    cameraPosition.Y - (PlayerDimension.Y / 5) * 3,
                    cameraPosition.Z - (PlayerDimension.Z / 2)
                ),
                new Vector3(
                    cameraPosition.X + (PlayerDimension.X / 2),
                    cameraPosition.Y + (PlayerDimension.Y / 5) * 2,
                    cameraPosition.Z + (PlayerDimension.Z / 2)
                )
            );

            BoundingSphere cameraSphere = new BoundingSphere(
                new Vector3(
                    cameraPosition.X, 
                    cameraPosition.Y, 
                    cameraPosition.Z
                ),
                PlayerDimension.Y
            );

            // Let's check for collision with our boxes
            foreach(BoundingBox collisionBox in CollisionBoxes)
            {
                // If our player is inside the collision region
                if (collisionBox.Contains(cameraBox) !=
                    ContainmentType.Disjoint)
                {
                    return true;
                }
            }

            // Same with spheres
            foreach(BoundingSphere collisionSphere in CollisionSpheres)
            {
                if (collisionSphere.Contains(cameraBox) != 
                    ContainmentType.Disjoint)
                    return true;
            }

            // And finally with our models collisions
            Dictionary<string, LevelModel> levelModels =
                MainGame.SceneManager.ModelManager.GetLevelModels();
            foreach (KeyValuePair<string, LevelModel> thisModel in levelModels)
            {
                LevelModel.CollisionType collisionType 
                    = thisModel.Value.CollisionDetection;
                if (collisionType == LevelModel.CollisionType.None)
                {
                    continue;
                }

                if (collisionType == LevelModel.CollisionType.Accurate)
                {
                    var collidingFaces = new LinkedList<Face>();
                    var collisionPoints = new LinkedList<Vector3>();
                    // This method is used with pointer, so it does change 
                    // our above faces and points
                    thisModel.Value.CollisionModel.collisionData.collisions(
                        cameraBox,
                        collidingFaces,
                        collisionPoints
                    );
                    if (collidingFaces.Count > 0 || collisionPoints.Count > 0)
                    {
                        return true;
                    }
                }

                if (collisionType == LevelModel.CollisionType.Inaccurate)
                {
                    BoundingBox nBB = new BoundingBox(
                        thisModel.Value.CollisionModel.collisionData.geometry.boundingBox.Min + thisModel.Value.Position,
                        thisModel.Value.CollisionModel.collisionData.geometry.boundingBox.Max + thisModel.Value.Position
                    );
                    if (nBB.Contains(cameraBox) != ContainmentType.Disjoint)
                    {
                        return true;
                    }
                }
            }

            // If we're here, then no collision has been matched
            return false;
                
        }

    }
}
