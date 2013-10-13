using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace PerMeshBoundingBox
{
	[ContentProcessor(DisplayName = "CustomModelProcessor")]
	public class CustomModelProcessor :
		ModelProcessor
	{
		public override ModelContent Process(NodeContent input, ContentProcessorContext context)
		{
			ModelContent output = base.Process(input, context);

			// stores a bounding box per mesh inside the model - we link the bounding box to the mesh using it's name
			Dictionary<string, BoundingBox> boundingBoxes = new Dictionary<string, BoundingBox>();

			foreach (NodeContent currentNodeContent in input.Children)
			{
				// data needed to build a bounding box
				float minX = float.MaxValue;
				float minY = float.MaxValue;
				float minZ = float.MaxValue;
				float maxX = float.MinValue;
				float maxY = float.MinValue;
				float maxZ = float.MinValue;

				if (currentNodeContent is MeshContent)
				{
					MeshContent meshContent = (MeshContent)currentNodeContent;

					foreach (Vector3 basev in meshContent.Positions)
					{
						Vector3 v = basev;
						if (v.X < minX)
							minX = v.X;

						if (v.Y < minY)
							minY = v.Y;

						if (v.Z < minZ)
							minZ = v.Z;

						if (v.X > maxX)
							maxX = v.X;

						if (v.Y > maxY)
							maxY = v.Y;

						if (v.Z > maxZ)
							maxZ = v.Z;
					}

					// creates a new bounding box associated to the mesh name
					string meshName = currentNodeContent.Name;
					BoundingBox boundingBox =
						new BoundingBox(new Vector3((float)minX, (float)minY, -(float)minZ),
							new Vector3((float)maxX, (float)maxY, -(float)maxZ));

					boundingBoxes.Add(meshName, boundingBox);
				}
			}
			// stores the bounding box inside the tag property
			output.Tag = boundingBoxes;

			return output;
		}
	}
}