using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deimos
{
	class DirectionalLight
	{
		public Vector3 Direction;
		public Color Color;

		public DirectionalLight(Vector3 direction, Color color)
		{
			Direction = direction;
			Color = color;
		}

		public void SetDirection(Vector3 direction)
		{
			Direction = direction;
		}
		public void SetColor(Color color)
		{
			Color = color;
		}
	}
}
