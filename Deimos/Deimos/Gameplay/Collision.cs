using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Deimos
{
	class Collision
	{
		// Attributes
		Vector3 PlayerDimention;

		List<BoundingBox> CollisionBoxes = new List<BoundingBox>();
		BoundingBox[] CollisionBoxesArray;

		List<BoundingSphere> CollisionSpheres = new List<BoundingSphere>();
		BoundingSphere[] CollisionSpheresArray;


		// Constructor
		public Collision(Vector3 playerDimension)
		{
			PlayerDimention = playerDimension;
		}


		// Methods
		public BoundingBox AddCollisionBox(Vector3 coords1, Vector3 coords2)
		{
			Vector3[] boxPoints = new Vector3[2];
			boxPoints[0] = coords1;
			boxPoints[1] = coords2;
			BoundingBox thisNewBox = BoundingBox.CreateFromPoints(boxPoints);
			CollisionBoxes.Add(thisNewBox);

			FinishedAddingCollisions();

			return thisNewBox;
		}
		// Adding a box directly helps for the ModelManager class, as we're
		// Creating a box directly from its methods when loading a model
		public BoundingBox AddCollisionBoxDirectly(BoundingBox box)
		{
			CollisionBoxes.Add(box);

			FinishedAddingCollisions();

			return box;
		}

		public BoundingSphere AddCollisionSphere(Vector3 coords, float radius)
		{
			BoundingSphere thisNewSphere = new BoundingSphere(coords, radius);
			CollisionSpheres.Add(thisNewSphere);

			FinishedAddingCollisions();

			return thisNewSphere;
		}
		// Same here
		public BoundingSphere AddCollisionSphereDirectly(BoundingSphere sphere)
		{
			CollisionSpheres.Add(sphere);

			FinishedAddingCollisions();

			return sphere;
		}

		// Convert all our lists to arrays
		public void FinishedAddingCollisions()
		{
			CollisionBoxesArray = CollisionBoxes.ToArray();
			CollisionSpheresArray = CollisionSpheres.ToArray();
		}


		public Boolean CheckCollision(Vector3 cameraPosition)
		{
			return false;
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

			// Let's check for collision with our boxes
			if (CollisionBoxesArray != null)
			{
				// Looping through all the boudingboxes included previously
				for (int i = 0; i < CollisionBoxesArray.Length; i++) 
				{
					// If our player is inside the collision region
					if (CollisionBoxesArray[i].Contains(cameraBox) != 
						ContainmentType.Disjoint) 
						return true;
				}
			}

			// Same with spheres
			if (CollisionSpheresArray != null)
			{
				for (int i = 0; i < CollisionSpheresArray.Length; i++) 
				{
					if (CollisionSpheresArray[i].Contains(cameraBox) != 
						ContainmentType.Disjoint)
						return true;
				}
			}

			// If we're here, then no collision has been matched
			return false;
				
		}

	}
}
