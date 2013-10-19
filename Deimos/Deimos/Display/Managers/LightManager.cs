using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Deimos
{
	static class LightManager
	{
		// Attributes
		static int numberOfRows = 129;
		static int numberOfCols = 129;
		static int numberOfVertices = numberOfRows * numberOfCols;
		static int numberOfIndices = (numberOfRows - 1) 
			* (numberOfCols - 1) * 2 * 3;

		static List<DirectionalLight> DirectionalLights = new List<DirectionalLight>();
		static List<PointLight> PointLights =  new List<PointLight>();
		static List<SpotLight> SpotLights =  new List<SpotLight>();

		static Effect Effect;


		// Methods
		static public Effect SetEffect(Effect effect)
		{
			effect.Parameters["lightColor"].SetValue(Color.White.ToVector3());
			effect.Parameters["globalAmbient"].SetValue(Color.White.ToVector3());
			effect.Parameters["Ke"].SetValue(0.0f);
			effect.Parameters["Ka"].SetValue(0.01f);
			effect.Parameters["Kd"].SetValue(1.0f);
			effect.Parameters["Ks"].SetValue(0.3f);
			effect.Parameters["specularPower"].SetValue(100);
			effect.Parameters["spotPower"].SetValue(17);

			Effect = effect;

			return Effect;
		}


		static public void AddDirectionalLight(Vector3 direction, Color color)
		{
			DirectionalLights.Add(new DirectionalLight(direction, color));
		}

		static public void AddPointLight(Vector3 position, Color color)
		{
			PointLights.Add(new PointLight(position, color));
		}

		static public void AddSpotLight(Vector3 position, Vector3 direction,
			Color color, float power)
		{
			SpotLights.Add(new SpotLight(position, direction, color, power));
		}

		static public void ApplyLights(ModelMesh mesh, Matrix world, 
			Texture2D modelTexture, Camera camera, Effect effect, 
			GraphicsDevice graphicsDevice)
		{
			graphicsDevice.BlendState = BlendState.Opaque;
			//graphicsDevice.DepthStencilState = DepthStencilState.Default;

			effect.CurrentTechnique.Passes["Ambient"].Apply();

			foreach (ModelMeshPart part in mesh.MeshParts)
			{
				graphicsDevice.SetVertexBuffer(part.VertexBuffer);
				graphicsDevice.Indices = part.IndexBuffer;

				// Texturing
				graphicsDevice.BlendState = BlendState.AlphaBlend;
				if (modelTexture != null)
				{
					effect.Parameters["Texture"].SetValue(
						modelTexture
					);
					graphicsDevice.DrawIndexedPrimitives(
						PrimitiveType.TriangleList,
						part.VertexOffset,
						0,
						part.NumVertices,
						part.StartIndex,
						part.PrimitiveCount
					);
				}

				graphicsDevice.BlendState = BlendState.Additive;
				// Applying our shader to all the mesh parts
				effect.Parameters["WVP"].SetValue(
					world *
					camera.View *
					camera.Projection
				);
				effect.Parameters["World"].SetValue(world);
				effect.Parameters["eyePosition"].SetValue(
						camera.Position
				);


				//Vector3 cameraView = camera.ViewVector;
				//cameraView = new Vector3(
				//	-cameraView.X,
				//	-cameraView.Y,
				//	-cameraView.Z
				//);
				//effect.Parameters["lightDirection"].SetValue(
				//	Vector3.TransformNormal(
				//		cameraView,
				//		Matrix.Invert(world)
				//	)
				//);
				//effect.Parameters["lightPosition"].SetValue(
				//	Vector3.Transform(
				//		camera.Position,
				//		Matrix.Invert(world)
				//	)
				//);

				

				// Drawing lights
				foreach (DirectionalLight light in DirectionalLights)
				{
					effect.Parameters["lightColor"].SetValue(light.Color.ToVector3());
					effect.Parameters["lightDirection"].SetValue(light.Direction);

					// Applying changes and drawing them
					effect.CurrentTechnique.Passes["Directional"].Apply();
					graphicsDevice.DrawIndexedPrimitives(
						PrimitiveType.TriangleList, 
						part.VertexOffset, 
						0, 
						part.NumVertices, 
						part.StartIndex, 
						part.PrimitiveCount
					); 
				}

				foreach (PointLight light in PointLights)
				{
					effect.Parameters["lightColor"].SetValue(light.Color.ToVector3());
					effect.Parameters["lightPosition"].SetValue(light.Position);

					effect.CurrentTechnique.Passes["Point"].Apply();
					graphicsDevice.DrawIndexedPrimitives(
						PrimitiveType.TriangleList,
						part.VertexOffset,
						0,
						part.NumVertices,
						part.StartIndex,
						part.PrimitiveCount
					); 
				}

				foreach (SpotLight light in SpotLights)
				{
					effect.Parameters["lightColor"].SetValue(light.Color.ToVector3());
					effect.Parameters["lightPosition"].SetValue(light.Position);
					effect.Parameters["lightDirection"].SetValue(light.Direction);
					effect.Parameters["spotPower"].SetValue(light.Power);

					effect.CurrentTechnique.Passes["Spot"].Apply();
					graphicsDevice.DrawIndexedPrimitives(
						PrimitiveType.TriangleList,
						part.VertexOffset,
						0,
						part.NumVertices,
						part.StartIndex,
						part.PrimitiveCount
					); 

				}


				//part.Effect = effect;
			}
		}
	}
}
