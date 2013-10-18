using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Deimos
{
	public partial class Game1
	{
		#region Constants
		const int number_of_rows = 129;
		const int number_of_cols = 129;
		const int number_of_vertices = number_of_rows * number_of_cols;
		const int number_of_indices = (number_of_rows - 1) * 
										(number_of_cols - 1) * 2 * 3;
		#endregion

		#region Backreference 1 - Creating a basic VertexBuffer


		VertexBuffer vertexBuffer;

		void CreateVertexBuffer()
		{
			VertexPositionNormalTexture[] vertices = 
				new VertexPositionNormalTexture[number_of_vertices];

			float halfWidth = (number_of_cols - 1) * 0.5f;
			float halfDepth = (number_of_rows - 1) * 0.5f;

			float du = 1.0f / (number_of_cols - 1);
			float dv = 1.0f / (number_of_rows - 1);
			for (int i = 0; i < number_of_rows; ++i)
			{
				float z = halfDepth - i;
				for (int j = 0; j < number_of_cols; ++j)
				{
					float x = -halfWidth + j;

					float y = getHeight(x, z);

					vertices[i * number_of_cols + j].Position = 
						new Vector3(x, y, z);

					vertices[i * number_of_cols + j].TextureCoordinate = 
						new Vector2(j * du, i * dv);

					Vector3 normal = new Vector3();
					normal.X = -0.03f * z * (float)Math.Cos(0.1f * x) 
						- 0.3f * (float)Math.Cos(0.1f * z);

					normal.Y = 1;

					normal.Z = -0.3f * (float)Math.Sin(0.1f * x) 
						+ 0.03f * x * (float)Math.Sin(0.1f * z);

					normal.Normalize();
					vertices[i * number_of_cols + j].Normal = normal;
				}
			}

			vertexBuffer = new VertexBuffer(
				GraphicsDevice, 
				VertexPositionNormalTexture.VertexDeclaration, 
				number_of_vertices, 
				BufferUsage.WriteOnly
			);
			vertexBuffer.SetData<VertexPositionNormalTexture>(vertices);
		}

		private float getHeight(float x, float z)
		{
			return 0.3f * (z * (float)Math.Sin(0.1f * x) 
				+ x * (float)Math.Cos(0.1f * z));
		}
		#endregion

		#region Backreference 2 - Creating a basic IndexBuffer

		IndexBuffer indexBuffer;

		void CreateIndexBuffer()
		{
			int[] indices = new int[number_of_indices];
			int k = 0;
			for (int i = 0; i < number_of_rows - 1; ++i)
			{
				for (int j = 0; j < number_of_cols - 1; ++j)
				{
					indices[k] = i * number_of_cols + j;
					indices[k + 1] = (i + 1) * number_of_cols + j;
					indices[k + 2] = i * number_of_cols + j + 1;

					indices[k + 3] = (i + 1) * number_of_cols + j;
					indices[k + 4] = (i + 1) * number_of_cols + j + 1;
					indices[k + 5] = i * number_of_cols + j + 1;

					k += 6;
				}
			}

			indexBuffer = new IndexBuffer(
				GraphicsDevice, 
				IndexElementSize.ThirtyTwoBits, 
				number_of_indices, 
				BufferUsage.WriteOnly
			);
			indexBuffer.SetData<int>(indices);

		}
		#endregion
	}
}
