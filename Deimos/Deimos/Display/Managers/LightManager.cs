using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Deimos
{
	class LightManager
	{
		// Attributes
		List<DirectionalLight> DirectionalLights = new List<DirectionalLight>();
		List<PointLight> PointLights =  new List<PointLight>();
		List<SpotLight> SpotLights =  new List<SpotLight>();

		Vector3 AmbientColor;
		float SpecularPower;


		// Constructor
		public LightManager(Color ambientColor, float specularPower)
		{
			AmbientColor = ambientColor.ToVector3();
			SpecularPower = specularPower;
		}


		// Methods
		public Effect SetEffect(Effect effect)
		{
			effect.Parameters["lightColor"].SetValue(Color.White.ToVector3());
			effect.Parameters["globalAmbient"].SetValue(AmbientColor);
			effect.Parameters["Ke"].SetValue(0.0f);
			effect.Parameters["Ka"].SetValue(0.01f);
			effect.Parameters["Kd"].SetValue(1.0f);
			effect.Parameters["Ks"].SetValue(0.3f);
			effect.Parameters["specularPower"].SetValue(SpecularPower);

			return effect;
		}


		public DirectionalLight AddDirectionalLight(Vector3 direction, Color color)
		{
			DirectionalLight thisLight = new DirectionalLight(direction, color);
			DirectionalLights.Add(thisLight);

			return thisLight;
		}

		public PointLight AddPointLight(Vector3 position, Color color)
		{
			PointLight thisLight = new PointLight(position, color);
			PointLights.Add(thisLight);

			return thisLight;
		}

		public SpotLight AddSpotLight(Vector3 position, Vector3 direction,
			Color color, float power)
		{
			SpotLight thisLight = new SpotLight(
				position, 
				direction, 
				color, 
				power
			);
			SpotLights.Add(thisLight);

			return thisLight;
		}

		public void ApplyLights(ModelMesh mesh, Matrix world, 
			Texture2D modelTexture, Camera camera, GraphicsDevice graphicsDevice)
		{
			// Looping thorugh all of its meshparts
			foreach (ModelMeshPart part in mesh.MeshParts)
			{
				// Setting our effect from the one stored in the meshPart
				Effect effect = part.Effect;

				// Setting the blendstate to opaque for the ambient light
				graphicsDevice.BlendState = BlendState.Opaque;
				// Applying the ambient light
				effect.CurrentTechnique.Passes["Ambient"].Apply();

				// Setting our buffers to be able to draw the other lights
				graphicsDevice.SetVertexBuffer(part.VertexBuffer);
				graphicsDevice.Indices = part.IndexBuffer;

				// Setting up the default settings
				effect.Parameters["WVP"].SetValue(
					world *
					camera.View *
					camera.Projection
				);
				effect.Parameters["World"].SetValue(world);
				effect.Parameters["eyePosition"].SetValue(
					camera.Position
				);

				// Texturing
				graphicsDevice.BlendState = BlendState.AlphaBlend;
				if (modelTexture != null)
				{
					effect.Parameters["Texture"].SetValue(
						modelTexture
					);
				}

				// Drawing the changes
				graphicsDevice.DrawIndexedPrimitives(
					PrimitiveType.TriangleList,
					part.VertexOffset,
					0,
					part.NumVertices,
					part.StartIndex,
					part.PrimitiveCount
				);



				// Setting it up to additive because we'll be adding mutliple
				// lights
				graphicsDevice.BlendState = BlendState.Additive;
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
					//DebugScreen.Log(light.Direction.ToString());

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
			}
		}
	}
}
