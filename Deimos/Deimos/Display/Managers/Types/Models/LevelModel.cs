using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
	class LevelModel
	{
		private Vector3 position;
		public Vector3 Position
		{
			get { return position; }
			set { position = value; }
		}

		private float scale;
		public float Scale
		{
			get { return scale; }
			set { scale = value; }
		}

		private Model model;
		public Model Model
		{
			get { return model; }
			set { model = value; }
		}

		private bool collisionDetection = false;
		public bool CollisionDetection
		{
			get { return collisionDetection; }
			set { collisionDetection = value; }
		}

		private Model collisionModel;
		public Model CollisionModel
		{
			get { return collisionModel; }
			set { collisionModel = value; }
		}

	}
}
