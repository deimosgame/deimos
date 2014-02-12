using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deimos
{
	class PointLight
	{
		public Vector3 Position;
		public Color Color;
		public float Radius;
		public float Intensity;

		public PointLight(Vector3 position, float radius, float intensity, Color color)
		{
			Position = position;
			Color = color;
			Radius = radius;
			Intensity = intensity;
		}

		public void SetDirection(Vector3 position)
		{
			Position = position;
		}
		public void SetRadius(float radius)
		{
			Radius = radius;
		}
		public void SetIntensity(float intensity)
		{
			Intensity = intensity;
		}
		public void SetColor(Color color)
		{
			Color = color;
		}
	}
}
