using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deimos
{
	class Floor
	{
		// Attributes
		private int FloorHeight;
		private int FloorWidth;
		private VertexBuffer FloorBuffer;
		private GraphicsDevice Device;
		private Color[] FloorColors = new Color[2] { Color.White, Color.Black };

		// Constructor
		public Floor(GraphicsDevice device, int width, int height)
		{
			this.Device      = device;
			this.FloorHeight = height;
			this.FloorWidth  = width;

			buildFloorBuffer();
		}



		// Build our vertex buffer
		private void buildFloorBuffer()
		{
			List<VertexPositionColor> vertexList = new List<VertexPositionColor>();
			int counter = 0;

			// Creating the floor
			for (int x = 0; x < FloorWidth; x++)
			{
				counter++;

				for (int z = 0; z < FloorHeight; z++) // z because y would be the altitude
				{
					counter++;

					foreach(VertexPositionColor vertex in FloorTile(x, z, FloorColors[counter % 2]))
					{
						vertexList.Add(vertex);
					}
				}
			}

			// Create our buffer
			FloorBuffer = new VertexBuffer(Device, VertexPositionColor.VertexDeclaration, vertexList.Count, BufferUsage.None);
			FloorBuffer.SetData<VertexPositionColor>(vertexList.ToArray());
		}

		// Defines a single tile in our floor
		private List<VertexPositionColor> FloorTile(int xOffSet, int zOffSet, Color tileColor)
		{
			// Kinda complicated here. Basically, we create our squares, but to do so, we need to create some triangles.
			// But we also need to draw them clockwise, otherwise it won't work. That's why we need 6 vertexes.
			List<VertexPositionColor> vList = new List<VertexPositionColor>();
			vList.Add(new VertexPositionColor(new Vector3(0 + xOffSet, 0, 0 + zOffSet), tileColor)); //a
			vList.Add(new VertexPositionColor(new Vector3(1 + xOffSet, 0, 0 + zOffSet), tileColor)); //b
			vList.Add(new VertexPositionColor(new Vector3(0 + xOffSet, 0, 1 + zOffSet), tileColor)); //c
			vList.Add(new VertexPositionColor(new Vector3(1 + xOffSet, 0, 0 + zOffSet), tileColor)); //d
			vList.Add(new VertexPositionColor(new Vector3(1 + xOffSet, 0, 1 + zOffSet), tileColor)); //e
			vList.Add(new VertexPositionColor(new Vector3(0 + xOffSet, 0, 1 + zOffSet), tileColor)); //f
			// Basically, at d we are going back to the b state to be able to draw it clockwise.

			return vList;
		}


		// Draw method
		public void Draw(Camera camera, BasicEffect effect) // Basic effect: Shader
		{
			effect.VertexColorEnabled = true;
			effect.View = camera.View;
			effect.Projection = camera.Projection;
			effect.World = Matrix.Identity;

			// Loop through and draw each vertex (tiles of our floor)
			foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				Device.SetVertexBuffer(FloorBuffer);
				// Below we devide it by 3 because 6 vertexes / 3 = 2 triangles, and a square has 2 triangles.
				Device.DrawPrimitives(PrimitiveType.TriangleList, 0, FloorBuffer.VertexCount / 3);
			}
		}
	}
}
