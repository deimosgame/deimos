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
		private BoundingSphere CameraSphere;

		private BoundingBox[] CollisionBoxes;
		private BoundingSphere[] CollisionSpheres;

		// Constructor
		public Collision()
		{
			//
		}



		// Methods
		public void AddCollisionBox(Vector3 coords1, Vector3 coords2)
		{
			Vector3[] boxPoints = new Vector3[2];
            boxPoints[0] = coords1;
            boxPoints[1] = coords2;
			CollisionBoxes[CollisionBoxes.Count() + 1] = BoundingBox.CreateFromPoints(boxPoints);
		}

		public void AddCollisionSphere(Vector3 coords, float diameter)
		{
			CollisionSpheres[CollisionSpheres.Count() + 1] = new BoundingSphere(coords, diameter);
		}


		public Boolean CheckCollision(Vector3 cameraPosition)
		{
			// Creating the sphere of the camera for later collisions checks
			BoundingSphere cameraSphere = new BoundingSphere(cameraPosition, 0.04f);

			// Let s check for collision with our boxes
			if (CollisionBoxes != null)
			{
				for (int i = 0; i < CollisionBoxes.Length; i++)
				{
					if (CollisionBoxes[i].Contains(cameraSphere) != ContainmentType.Disjoint)
						return true;
				}
			}
			if (CollisionSpheres != null)
			{
				// And with our spheres
				for (int i = 0; i < CollisionBoxes.Length; i++)
				{
					if (CollisionBoxes[i].Contains(cameraSphere) != ContainmentType.Disjoint)
						return true;
				}
			}

			return false;
				
		}

	}
}
