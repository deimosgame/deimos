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
		// Constants
		const int numberOfRows = 129;
		const int numberOfCols = 129;
		const int numberOfVertices = numberOfRows * numberOfCols;
		const int numberOfIndices = (numberOfRows - 1) 
			* (numberOfCols - 1) * 2 * 3;


		// Attributes
		List<DirectionalLight> DirectionLights;
		List<PointLight> PointLights;
		List<SpotLight> SpotLights;

		Effect Effect;
		
		

		public LightManager()
		{
			//
		}

		public void SetEffect(Effect effect)
		{
			effect.Parameters["lightColor"].SetValue(Color.White.ToVector3());
			effect.Parameters["globalAmbient"].SetValue(Color.White.ToVector3());
			effect.Parameters["Ke"].SetValue(0.0f);
			effect.Parameters["Ka"].SetValue(0.01f);
			effect.Parameters["Kd"].SetValue(1.0f);
			effect.Parameters["Ks"].SetValue(0.3f);
			effect.Parameters["specularPower"].SetValue(100);

			Effect = effect;
		}
	}
}
