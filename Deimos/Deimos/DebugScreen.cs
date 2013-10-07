using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Deimos
{
	class DebugScreen
	{
		// Properties
		SpriteBatch SpriteBatch;
		SpriteFont SpriteFont;

		Camera Camera;

		// Constructors
		public DebugScreen(Camera camera)
        {
			Camera = camera;
        }

		public void LoadFont(SpriteBatch batch, SpriteFont font)
        {
			SpriteBatch = batch;
			SpriteFont = font;
        }

		public void Update(GameTime gameTime)
		{
			
		}

		public void Draw(GameTime gameTime)
		{
			SpriteBatch.Begin();

			SpriteBatch.DrawString(SpriteFont, "Coords:", new Vector2(10, 10), Color.Black);
			SpriteBatch.DrawString(SpriteFont, "X: " + Camera.CameraPosition.X.ToString(), new Vector2(5, 20), Color.Black);
			SpriteBatch.DrawString(SpriteFont, "Y: " + Camera.CameraPosition.Y.ToString(), new Vector2(5, 30), Color.Black);
			SpriteBatch.DrawString(SpriteFont, "Z: " + Camera.CameraPosition.Z.ToString(), new Vector2(5, 40), Color.Black);

			SpriteBatch.End();
		}
	}
}
