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

		public PointLight(Vector3 position, Color color)
		{
			Position = position;
			Color = color;
		}

		public void SetDirection(Vector3 position)
		{
			Position = position;
		}
		public void SetColor(Color color)
		{
			Color = color;
		}
	}
}
