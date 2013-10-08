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

		int FrameRate = 0;
		int FrameCounter = 0;
		TimeSpan ElapsedTime = TimeSpan.Zero;

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
			ElapsedTime += gameTime.ElapsedGameTime;

			if (ElapsedTime > TimeSpan.FromSeconds(1))
			{
				ElapsedTime -= TimeSpan.FromSeconds(1);
				FrameRate = FrameCounter;
				FrameCounter = 0;
			}
		}

		public void Draw(GameTime gameTime)
		{
			SpriteBatch.Begin();

			FrameCounter++;

			SpriteBatch.DrawString(SpriteFont, "Coords:", new Vector2(10, 10), Color.Black);
			SpriteBatch.DrawString(SpriteFont, "X: " + Camera.CameraPosition.X.ToString(), new Vector2(5, 20), Color.Black);
			SpriteBatch.DrawString(SpriteFont, "Y: " + Camera.CameraPosition.Y.ToString(), new Vector2(5, 30), Color.Black);
			SpriteBatch.DrawString(SpriteFont, "Z: " + Camera.CameraPosition.Z.ToString(), new Vector2(5, 40), Color.Black);

			SpriteBatch.DrawString(SpriteFont, "FPS:", new Vector2(10, 50), Color.Black);
			SpriteBatch.DrawString(SpriteFont, FrameRate.ToString(), new Vector2(5, 60), Color.Black);

			SpriteBatch.End();
		}
	}
}
