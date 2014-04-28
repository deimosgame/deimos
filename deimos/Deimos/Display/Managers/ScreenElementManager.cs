using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class ScreenElementManager
    {
        private Dictionary<string, ScreenRectangle> ElementsRectangle =
            new Dictionary<string, ScreenRectangle>();
        private List<string> ElementsRectangleList = new List<string>();
        private Dictionary<string, ScreenImage> ElementsImage =
            new Dictionary<string, ScreenImage>();
        private List<string> ElementsImageList = new List<string>();
        private Dictionary<string, ScreenText> ElementsText =
            new Dictionary<string, ScreenText>();
        private List<string> ElementsTextList = new List<string>();

        private Texture2D DummyTexture;

        // Constructor
        public ScreenElementManager()
        {
            DummyTexture = new Texture2D(GeneralFacade.Game.GraphicsDevice, 1, 1);
            DummyTexture.SetData(new Color[] { Color.White });
        }

        // Methods
        public ScreenRectangle AddRectangle(string name, int posX, int posY,
            int zIndex, int width, int height, Color color,
            Action<ScreenElement, DeimosGame> onClick)
        {
            ScreenRectangle element =
                new ScreenRectangle(posX, posY, zIndex, width, height, color);
            element.OnClick = onClick;
            ElementsRectangle.Add(
                name,
                element
            );
            ElementsRectangleList.Add(name);
            return element;
        }
        public ScreenRectangle AddRectangle(string name, int posX, int posY,
            int zIndex, int width, int height, Color color)
        {
            return AddRectangle(name, posX, posY, zIndex, width, height, color, null);
        }
        public ScreenRectangle GetRectangle(string name)
        {
            return ElementsRectangle[name];
        }
        public void RemoveRectangle(string name)
        {
            ElementsRectangle.Remove(name);
            ElementsRectangleList.Remove(name);
        }

        public ScreenImage AddImage(string name, int posX, int posY, float scaleX,
           float scaleY, int zIndex, Texture2D image)
        {
            ScreenImage element =
                new ScreenImage(posX, posY, scaleX, scaleY, zIndex, image);
            ElementsImage.Add(
                name,
                element
            );
            ElementsImageList.Add(name);

            return element;
        }
        public ScreenImage AddImage(string name, int posX, int posY, float scale,
            int zIndex, Texture2D image)
        {
            return AddImage(name, posX, posY, scale, scale, zIndex, image);
        }
        public ScreenImage GetImage(string name)
        {
            return ElementsImage[name];
        }
        public void RemoveImage(string name)
        {
            ElementsImage.Remove(name);
            ElementsImageList.Remove(name);
        }

        public ScreenText AddText(string name, int posX, int posY,
            int zIndex, SpriteFont font, string text, Color color)
        {
            ScreenText element =
                new ScreenText(posX, posY, zIndex, font, text, color);
            ElementsText.Add(
                name,
                element
            );
            return element;
        }
        public ScreenText GetText(string name)
        {
            return ElementsText[name];
        }
        public void RemoveText(string name)
        {
            ElementsText.Remove(name);
        }

        public void HandleMouse()
        {
            MouseState mouseState = Mouse.GetState();
            Rectangle mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
            for (int i = 0; i < ElementsRectangleList.Count; i++)
            {
                ScreenRectangle thisRectangle = ElementsRectangle[ElementsRectangleList[i]];
                Rectangle rectangle = new Rectangle(
                    (int)thisRectangle.Pos.X,
                    (int)thisRectangle.Pos.Y,
                    thisRectangle.Height,
                    thisRectangle.Width
                );
                HandleEvent(thisRectangle, mouseRectangle, rectangle, mouseState);
            }
            for (int i = 0; i < ElementsImageList.Count; i++)
            {
                ScreenImage thisRectangle = ElementsImage[ElementsImageList[i]];
                Rectangle rectangle = new Rectangle(
                    (int)thisRectangle.Pos.X,
                    (int)thisRectangle.Pos.Y,
                    thisRectangle.Image.Height,
                    thisRectangle.Image.Width
                );
                HandleEvent(thisRectangle, mouseRectangle, rectangle, mouseState);
            }
        }
        private void HandleEvent(ScreenElement el, Rectangle mouse, Rectangle elRect,
            MouseState mouseState)
        {
            if (mouse.Intersects(elRect))
            {
                if (el.LastState != ScreenElement.ElState.Hover)
                {
                    el.OnHover(el, GeneralFacade.Game);
                    el.LastState = ScreenElement.ElState.Hover;
                }
                if (mouseState.LeftButton == ButtonState.Pressed
                    && el.LastState != ScreenElement.ElState.Click)
                {
                    el.OnClick(el, GeneralFacade.Game);
                    el.LastState = ScreenElement.ElState.Click;
                }
            }
            else
            {
                if (el.LastState != ScreenElement.ElState.Out)
                {
                    el.OnOut(el, GeneralFacade.Game);
                    el.LastState = ScreenElement.ElState.Out;
                }
            }
        }

        public void DrawElements(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Opaque,
                SamplerState.PointWrap,
                DepthStencilState.DepthRead,
                RasterizerState.CullNone
            );
            foreach (KeyValuePair<string, ScreenRectangle> elementKeyVal in
                ElementsRectangle)
            {
                ScreenRectangle element = elementKeyVal.Value;
                if (!element.Show)
                {
                    continue;
                }
                spriteBatch.Draw(
                    DummyTexture,
                    new Rectangle((int)element.Pos.X, (int)element.Pos.Y, element.Height, element.Width), element.Color
                );
            }
            foreach (KeyValuePair<string, ScreenImage> elementKeyVal in
                ElementsImage)
            {
                ScreenImage element = elementKeyVal.Value;
                if (!element.Show)
                {
                    continue;
                }
                spriteBatch.Draw(
                    element.Image,
                    element.Pos,
                    null,
                    Color.White,
                    0,
                    Vector2.Zero,
                    new Vector2(element.ScaleX, element.ScaleY),
                    SpriteEffects.None,
                    0f
                );
            }
            foreach (KeyValuePair<string, ScreenText> elementKeyVal in
                ElementsText)
            {
                ScreenText element = elementKeyVal.Value;
                if (!element.Show)
                {
                    continue;
                }
                spriteBatch.DrawString(
                    element.Font,
                    element.Text,
                    element.Pos,
                    element.Color
                );
            }

            spriteBatch.End();
        }
    }
}
