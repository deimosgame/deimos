using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Deimos
{
	static class DebugScreen
	{
		// Properties
		static SpriteBatch SpriteBatch;
		static SpriteFont SpriteFont;

		static Camera Camera;

		static private List<String> LogStrings = new List<String>();

		static int FrameRate = 0;
		static int FrameCounter = 0;
		static TimeSpan ElapsedTime = TimeSpan.Zero;

		// Methods
		static public void SetCamera(Camera camera)
		{
			Camera = camera;
		}

		static public void LoadFont(SpriteBatch batch, SpriteFont font)
		{
			SpriteBatch = batch;
			SpriteFont = font;
		}

		static public void Log(String logText)
		{
			LogStrings.Add(logText);
		}

		static public void Update(GameTime gameTime)
		{
			ElapsedTime += gameTime.ElapsedGameTime;

			if (ElapsedTime > TimeSpan.FromSeconds(1))
			{
				ElapsedTime -= TimeSpan.FromSeconds(1);
				FrameRate = FrameCounter;
				FrameCounter = 0;
			}
		}

		static public void Draw(GameTime gameTime)
		{
			SpriteBatch.Begin();

			FrameCounter++;

			SpriteBatch.DrawString(SpriteFont, "Coords:", new Vector2(10, 10), Color.Black);
			SpriteBatch.DrawString(SpriteFont, "X: " + Camera.CameraPosition.X.ToString(), new Vector2(5, 20), Color.Black);
			SpriteBatch.DrawString(SpriteFont, "Y: " + Camera.CameraPosition.Y.ToString(), new Vector2(5, 30), Color.Black);
			SpriteBatch.DrawString(SpriteFont, "Z: " + Camera.CameraPosition.Z.ToString(), new Vector2(5, 40), Color.Black);

			SpriteBatch.DrawString(SpriteFont, "FPS:", new Vector2(10, 50), Color.Black);
			SpriteBatch.DrawString(SpriteFont, FrameRate.ToString(), new Vector2(5, 60), Color.Black);


			// Console logs
			SpriteBatch.DrawString(SpriteFont, "Console logs:", new Vector2(10, 70), Color.Black);

			String[] logStrings = LogStrings.ToArray();
			int iLimit = 0;
			int position = 0;
			if (logStrings.Length > 10)
			{
				iLimit = logStrings.Length - 10;
			}
			for (int i = logStrings.Length; i > iLimit; i--)
			{
				SpriteBatch.DrawString(SpriteFont, logStrings[i - 1], new Vector2(5, 70 + ((position + 1) * 10)), Color.Black);
				position++;
			}

			SpriteBatch.End();
		}
	}
}
