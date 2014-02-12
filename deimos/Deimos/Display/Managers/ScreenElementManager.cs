using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private Dictionary<string, ScreenImage> ElementsImage =
            new Dictionary<string, ScreenImage>();
        private Dictionary<string, ScreenText> ElementsText =
            new Dictionary<string, ScreenText>();

        private Texture2D DummyTexture;

        // Constructor
        public ScreenElementManager(GraphicsDevice graphicsDevice)
        {
            DummyTexture = new Texture2D(graphicsDevice, 1, 1);
            DummyTexture.SetData(new Color[] { Color.White });
        }

        // Methods
        public ScreenRectangle AddRectangle(string name, int posX, int posY,
            int zIndex, int width, int height, Color color)
        {
            ScreenRectangle element =
                new ScreenRectangle(posX, posY, zIndex, width, height, color);
            ElementsRectangle.Add(
                name,
                element
            );
            return element;
        }
        public ScreenRectangle GetRectangle(string name)
        {
            return ElementsRectangle[name];
        }

        public ScreenImage AddImage(string name, int posX, int posY, float scale,
            int zIndex, Texture2D image)
        {
            ScreenImage element =
                new ScreenImage(posX, posY, scale, zIndex, image);
            ElementsImage.Add(
                name,
                element
            );
            return element;
        }
        public ScreenImage GetImage(string name)
        {
            return ElementsImage[name];
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
                    new Rectangle(element.PosX, element.PosY, element.Height, element.Width), element.Color
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
                    new Vector2(element.PosX, element.PosY),
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    element.Scale,
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
                    new Vector2(element.PosX, element.PosY),
                    element.Color
                );
            }

            spriteBatch.End();
        }
    }
}
