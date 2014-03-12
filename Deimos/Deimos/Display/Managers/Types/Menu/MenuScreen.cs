using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class MenuScreen
    {
        private int lastHeight;
        private DeimosGame Game;
        private string Name;
        private List<MenuElement> MenuElements = new List<MenuElement>();
        private SpriteFont Font;

        public MenuScreen(DeimosGame game, string name)
        {
            Game = game;
            Name = name;

            Font = Game.Content.Load<SpriteFont>("Fonts/debug");
        }

        public void AddElement(string title, Action<ScreenElement, DeimosGame> thisEvent)
        {
            lastHeight += 10;
            MenuElement nElement = new MenuElement();
            nElement.Title = title;
            nElement.MarginTop = lastHeight;
            nElement.ClickEvent = thisEvent;
            MenuElements.Add(nElement);
        }

        public void ShowElements()
        {
            lastHeight = 200;
            foreach (var item in MenuElements)
            {
                lastHeight += 50;
                Game.ScreenElementManager.AddText(
                    "MenuElementText" + Name + item.Title,
                    50,
                    lastHeight,
                    1,
                    Font,
                    item.Title,
                    Color.White
                );
                Game.ScreenElementManager.AddRectangle(
                    "MenuElementHitbox" + Name + item.Title,
                    50,
                    lastHeight,
                    1,
                    30,
                    200,
                    Color.Transparent,
                    item.ClickEvent
                );
            }
        }
        public void HideElements()
        {
            foreach (var item in MenuElements)
            {
                Game.ScreenElementManager.RemoveText("MenuElementText" + Name + item.Title);
                Game.ScreenElementManager.RemoveRectangle("MenuElementHitbox" + Name + item.Title);
            }
        }
    }
}
