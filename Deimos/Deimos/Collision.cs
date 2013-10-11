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
		private Vector3 PlayerDimention;

		private List<Dictionary<string, Vector3>> CollisionBoxesVectors = new List<Dictionary<string, Vector3>>();
		private Dictionary<string, Vector3>[] CollisionBoxesVectorsArray;

		private List<BoundingBox> CollisionBoxes = new List<BoundingBox>();
		private BoundingBox[] CollisionBoxesArray;


		// Constructor
		public Collision(float height, float width, float depth)
		{
			PlayerDimention.X = width;
			PlayerDimention.Y = height;
			PlayerDimention.Z = depth;
		}



		// Methods
		public void AddCollisionBox(Vector3 coords1, Vector3 coords2)
		{
			Vector3[] boxPoints = new Vector3[2];
			boxPoints[0] = coords1;
			boxPoints[1] = coords2;
			Dictionary<string, Vector3> newCoords = new Dictionary<string, Vector3>();
			newCoords.Add("Vector1", coords1);
			newCoords.Add("Vector2", coords2);
			CollisionBoxesVectors.Add(newCoords);
			CollisionBoxes.Add(BoundingBox.CreateFromPoints(boxPoints));
		}

		public void FinishedAddingCollisions()
		{
			CollisionBoxesArray = CollisionBoxes.ToArray();
			CollisionBoxesVectorsArray = CollisionBoxesVectors.ToArray();
		}


		public Boolean CheckCollision(Vector3 cameraPosition)
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

			// Let's check for collision with our boxes
			if (CollisionBoxesArray != null)
			{
				for (int i = 0; i < CollisionBoxesArray.Length; i++)
				{
					if (CollisionBoxesArray[i].Contains(cameraBox) != ContainmentType.Disjoint) // If our player is inside the collision region
						return true;
				}
			}

			return false;
				
		}

		public Vector2 GetCollisionVector(Vector3 cameraPosition, Vector3 cameraRotation)
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

			// Let's check for collision with our boxes
			if (CollisionBoxesArray != null)
			{
				for (int i = 0; i < CollisionBoxesArray.Length; i++)
				{
					if (CollisionBoxesArray[i].Contains(cameraBox) != ContainmentType.Disjoint) // If our player is inside the collision region
					{
						Vector2 newMovementVector;

						if (cameraPosition.X > CollisionBoxesVectorsArray[i]["Vector1"].X 
							&& cameraPosition.X > CollisionBoxesVectorsArray[i]["Vector2"].X) // Then the player is above the collision
						{
							newMovementVector = new Vector2(0, 1);
						}
						else if(cameraPosition.X < CollisionBoxesVectorsArray[i]["Vector1"].X 
							&& cameraPosition.X < CollisionBoxesVectorsArray[i]["Vector2"].X) // Then he is below
						{
							newMovementVector = new Vector2(0, 1);
						}
						else if (cameraPosition.Y > CollisionBoxesVectorsArray[i]["Vector1"].Y
							&& cameraPosition.Y > CollisionBoxesVectorsArray[i]["Vector2"].Y) // Then at the right
						{
							newMovementVector = new Vector2(1, 0);
						}
						else // He's obviously at the right
						{
							newMovementVector = new Vector2(1, 0);
						}

						return newMovementVector;
					}
				}
			}

			return Vector2.Zero;
		}

	}
}
