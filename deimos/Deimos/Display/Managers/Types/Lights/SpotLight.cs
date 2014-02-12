using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deimos
{
	class SpotLight
	{
		public Vector3 Position;
		public Vector3 Direction;
		public Color Color;
		public float Power;

		public SpotLight(Vector3 position, Vector3 direction, Color color, 
			float power)
		{
			Position = position;
			Direction = direction;
			Color = color;
			Power = power;
		}

		public void SetPosition(Vector3 position)
		{
			Position = position;
		}
		public void SetDirection(Vector3 direction)
		{
			Direction = direction;
		}
		public void SetColor(Color color)
		{
			Color = color;
		}
		public void SetPower(float power)
		{
			Power = power;
		}
	}
}
