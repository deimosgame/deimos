using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Deimos
{
	public class Menu
	{
		SpriteBatch SpriteBatch;
		SpriteFont SpriteFont;

		private bool initialized = false;
		private List<MenuItem> menuItems { get; set; }
		public int Count
		{
			get { return menuItems.Count; }
		}
		public string Title { get; set; }
		public string InfoText { get; set; }
		private int lastNavigated { get; set; }
		private int _selectedIndex;
		public int selectedIndex
		{
			get
			{
				return _selectedIndex;
			}
			protected set
			{
				if (value >= menuItems.Count || value < 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				_selectedIndex = value;
			}
		}
		public MenuItem SelectedItem
		{
			get
			{
				return menuItems[selectedIndex];
			}
		}

		public Menu(string title)
		{
			menuItems = new List<MenuItem>();
			Title = title;
			InfoText = "";
		}

		public Menu(string title, string infoText)
		{
			menuItems = new List<MenuItem>();
			Title = title;
			InfoText = infoText;
		}

		public void LoadFont(SpriteBatch batch, SpriteFont font)
		{
			SpriteBatch = batch;
			SpriteFont = font;
		}

		public virtual void AddMenuItem(string title, Action<Keys> action)
		{
			AddMenuItem(title, action, "");
		}

		public virtual void AddMenuItem(string title, Action<Keys> action, string description)
		{
			menuItems.Add(new MenuItem { Title = title, Description = description, Action = action });
			selectedIndex = 0;
		}

		public void DrawMenu(int screenWidth)
		{
			DrawMenu(screenWidth, 100, new Vector2(1000, 1000), Color.Gray, Color.White);
		}

		public void DrawMenu(int screenWidth, int yPos, Vector2 descriptionPos, Color itemColor, Color selectedColor)
		{
			SpriteBatch.DrawString(SpriteFont, Title, new Vector2(screenWidth / 2 - SpriteFont.MeasureString(Title).X / 2, yPos), Color.White);
			yPos += (int)SpriteFont.MeasureString(Title).Y + 10;
			for (int i = 0; i < Count; i++)
			{
				Color color = itemColor;
				if (i == selectedIndex)
				{
					color = selectedColor;
				}
				SpriteBatch.DrawString(SpriteFont, menuItems[i].Title, new Vector2(screenWidth / 2 - SpriteFont.MeasureString(menuItems[i].Title).X / 2, yPos), color);
				yPos += (int)SpriteFont.MeasureString(menuItems[i].Title).Y + 10;
			}
			SpriteBatch.DrawString(SpriteFont, menuItems[selectedIndex].Description, descriptionPos, selectedColor);
		}

		public void Navigate(KeyboardState ks, GameTime gameTime)
		{
			if (!initialized)
			{
				lastNavigated = (int)gameTime.TotalGameTime.TotalMilliseconds;
				initialized = true;
			}
			if (gameTime.TotalGameTime.TotalMilliseconds - lastNavigated > 250)
			{
				if (ks.IsKeyDown(Keys.Up) && selectedIndex < Count - 1)
				{
					selectedIndex++;
					lastNavigated = (int)gameTime.TotalGameTime.TotalMilliseconds;
				}
				if (ks.IsKeyDown(Keys.Down) && selectedIndex > 0)
				{
					selectedIndex--;
					lastNavigated = (int)gameTime.TotalGameTime.TotalMilliseconds;
				}


				if (ks.IsKeyDown(Keys.Enter))
				{
					SelectedItem.Action(Keys.Enter);
					lastNavigated = (int)gameTime.TotalGameTime.TotalMilliseconds;
				}
				else if (ks.IsKeyDown(Keys.Escape))
				{
					SelectedItem.Action(Keys.Escape);
					lastNavigated = (int)gameTime.TotalGameTime.TotalMilliseconds;
				}
			}
		}
	}
}
