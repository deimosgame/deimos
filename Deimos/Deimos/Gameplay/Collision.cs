using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CollidableModel;


namespace Deimos
{
	static class Collision
	{
		// Attributes
		static Vector3 PlayerDimention;

		static List<BoundingBox> CollisionBoxes = 
			new List<BoundingBox>();

		static List<BoundingSphere> CollisionSpheres = 
			new List<BoundingSphere>();




		// Methods
		static public void SetPlayerDimensions(float height, float width, float depth)
		{
			// These dimentions will be used to check for the camera collision:
			// a player/human isn't a cube but a box; taller than larger
			PlayerDimention.X = width;
			PlayerDimention.Y = height;
			PlayerDimention.Z = depth;
		}


		static public void AddCollisionBox(Vector3 coords1, Vector3 coords2, 
			float scale)
		{
			Vector3[] boxPoints = new Vector3[2];
			boxPoints[0] = coords1 * scale;
			boxPoints[1] = coords2 * scale;
			CollisionBoxes.Add(BoundingBox.CreateFromPoints(boxPoints));
		}
		// Adding a box directly helps for the ModelManager class, as we're
		// Creating a box directly from its methods when loading a model
		static public void AddCollisionBoxDirectly(BoundingBox box)
		{
			CollisionBoxes.Add(box);
		}

		static public void AddCollisionSphere(Vector3 coords, float radius)
		{
			CollisionSpheres.Add(new BoundingSphere(coords, radius));
		}
		// Same here
		static public void AddCollisionSphereDirectly(BoundingSphere sphere)
		{
			CollisionSpheres.Add(sphere);
		}


		static public Boolean CheckCollision(Vector3 cameraPosition)
		{
			// Creating the sphere of the camera for later collisions checks
			BoundingBox cameraBox = new BoundingBox(
				new Vector3(
					cameraPosition.X - (PlayerDimention.X / 2),
					cameraPosition.Y - (PlayerDimention.Y),
					cameraPosition.Z - (PlayerDimention.Z / 2)
				),
				new Vector3(
					cameraPosition.X + (PlayerDimention.X / 2),
					cameraPosition.Y,
					cameraPosition.Z + (PlayerDimention.Z / 2)
				)
			);

			BoundingSphere cameraSphere = new BoundingSphere(
				new Vector3(
					cameraPosition.X, 
					cameraPosition.Y, 
					cameraPosition.Z
				),
				PlayerDimention.Y
			);

			// Let's check for collision with our boxes
			if (CollisionBoxes.Count > 0)
			{
				// Looping through all the boudingboxes included previously
					foreach(BoundingBox collisionBox in CollisionBoxes)
				{
					// If our player is inside the collision region
					if (collisionBox.Contains(cameraBox) !=
						ContainmentType.Disjoint)
					{
						return true;
					}
				}
			}

			// Same with spheres
			if (CollisionSpheres.Count > 0)
			{
				foreach(BoundingSphere collisionSphere in CollisionSpheres)
				{
					if (collisionSphere.Contains(cameraBox) != 
						ContainmentType.Disjoint)
						return true;
				}
			}

			// And finally with our models collisions
			Dictionary<string, LevelModel> levelModels = 
				SceneManager.GetModelManager().GetLevelModels();
			foreach (KeyValuePair<string, LevelModel> thisModel in levelModels)
			{
				if (!thisModel.Value.CollisionDetection)
				{
					continue;
				}
				
				var collidingFaces = new LinkedList<Face>();
				var collisionPoints = new LinkedList<Vector3>();
				// This method is used with pointer, so it does change 
				// our above faces and points
				thisModel.Value.CollisionModel.collisionData.collisions(
					cameraSphere, 
					collidingFaces, 
					collisionPoints
				);
				if (collidingFaces.Count > 0)
				{
					Console.WriteLine("Collision with " + thisModel.Key);
					return true;
				}
				if (collisionPoints.Count > 0)
				{
					return true;
				}
			}

			// If we're here, then no collision has been matched
			return false;
				
		}

	}
}
