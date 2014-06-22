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
        private Dictionary<string, ScreenImage> ElementsImage =
            new Dictionary<string, ScreenImage>();
        private Dictionary<string, ScreenText> ElementsText =
            new Dictionary<string, ScreenText>();
        private Dictionary<string, ScreenLine> ElementsLine =
            new Dictionary<string, ScreenLine>();
        private Dictionary<string, ScreenTable> ElementsTable =
            new Dictionary<string, ScreenTable>();

        public Texture2D DummyTexture;

        private Vector2 MousePosition;
        public bool IsMouseVisible = false;

        protected int CooldownMilliseconds = 500;
        protected int CooldownTimer = 0;


        // Constructor
        public ScreenElementManager()
        {
            DummyTexture = new Texture2D(GeneralFacade.Game.GraphicsDevice, 1, 1);
            DummyTexture.SetData(new Color[] { Color.White });
        }

        // Methods
        public ScreenRectangle AddRectangle(string name, int posX, int posY,
            int zIndex, int width, int height, Color color,
            Action<ScreenElement, DeimosGame> onClick, 
            Action<ScreenElement, DeimosGame> onHover, 
            Action<ScreenElement, DeimosGame> onOut)
        {
            ScreenRectangle element =
                new ScreenRectangle(posX, posY, zIndex, width, height, color, onClick, onHover, onOut);
            ElementsRectangle.Add(
                name,
                element
            );

            return element;
        }
        public ScreenRectangle AddRectangle(string name, int posX, int posY,
            int zIndex, int width, int height, Color color)
        {
            return AddRectangle(name, posX, posY, zIndex, width, height, color, null, null, null);
        }
        public ScreenRectangle GetRectangle(string name)
        {
            return ElementsRectangle[name];
        }
        public void RemoveRectangle(string name)
        {
            ElementsRectangle.Remove(name);
        }

        public ScreenImage AddImage(string name, int posX, int posY, float scaleX,
           float scaleY, int zIndex, Texture2D image,
            Action<ScreenElement, DeimosGame> onClick,
            Action<ScreenElement, DeimosGame> onHover,
            Action<ScreenElement, DeimosGame> onOut,
            Action<ScreenElement, DeimosGame> onKeyPress)
        {
            ScreenImage element =
                new ScreenImage(posX, posY, scaleX, scaleY, zIndex, image);
            ElementsImage.Add(
                name,
                element
            );

            ElementsImage = ElementsImage.OrderBy(kvp => kvp.Value.ZIndex).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if(onClick != null || onHover != null || onOut != null || onKeyPress != null)
            {
                AddRectangle(name + "RectangleEvent", posX, posY, zIndex, 
                    (int)(image.Width * scaleX), (int)(image.Height * scaleY), 
                    Color.Transparent, onClick, onHover, onOut);
            }
            

            return element;
        }
        public ScreenImage AddImage(string name, int posX, int posY, float scaleX,
           float scaleY, int zIndex, Texture2D image,
            Action<ScreenElement, DeimosGame> onClick,
            Action<ScreenElement, DeimosGame> onHover,
            Action<ScreenElement, DeimosGame> onOut)
        {
            return AddImage(name, posX, posY, scaleX, scaleY, zIndex, image, onClick, onHover, onOut, null);
        }
        public ScreenImage AddImage(string name, int posX, int posY, float scale,
            int zIndex, Texture2D image)
        {
            return AddImage(name, posX, posY, scale, scale, zIndex, image, null, null, null, null);
        }
        public ScreenImage GetImage(string name)
        {
            return ElementsImage[name];
        }
        public void RemoveImage(string name)
        {
            ElementsImage.Remove(name);
            ElementsRectangle.Remove(name + "RectangleEvent");
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

            ElementsText = ElementsText.OrderBy(kvp => kvp.Value.ZIndex).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

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

        public ScreenLine AddLine(string name, Vector2 start, Vector2 end, 
            int zIndex, Color color)
        {
            ScreenLine element =
                new ScreenLine(start, end, zIndex, color);
            ElementsLine.Add(name, element);
            return element;
        }
        public ScreenLine GetLine(string name)
        {
            return ElementsLine[name];
        }
        public void RemoveLine(string name)
        {
            ElementsLine.Remove(name);
        }

        public ScreenTable AddTable(string name, int posX, int posY, int zIndex, 
            Color bgColor, Color borderHeaderColor, Color borderColor, Color headerFontColor,
            Color fontColor, SpriteFont font, int columnSize, int padding,
            List<string> headers, List<TableRow> content)
        {
            ScreenTable element = new ScreenTable(posX, posY, zIndex, bgColor,
                borderHeaderColor, borderColor, headerFontColor, fontColor, 
                font, columnSize, padding, headers, content);

            ElementsTable.Add(name, element);

            return element;
        }
        public ScreenTable GetTable(string name)
        {
            return ElementsTable[name];
        }
        public void RemoveTable(string name)
        {
            ElementsTable.Remove(name);
        }

        public void HandleMouse(float dt)
        {
            MouseState mouseState = Mouse.GetState();

            MousePosition = new Vector2(mouseState.X, mouseState.Y);

            if (CooldownTimer > 0)
            {
                CooldownTimer -= (int)(dt * 1000);
                return;
            }

            Rectangle mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
            for (int i = 0; i < ElementsImage.Count; i++)
            {
                KeyValuePair<string, ScreenImage> keyValuePair = ElementsImage.ElementAt(i);
                ScreenImage thisRectangle = keyValuePair.Value;
                if (CooldownTimer > 0)
                {
                    continue;
                }
                if (thisRectangle.HandleEvent(mouseRectangle, mouseState))
                {
                    CooldownTimer = CooldownMilliseconds;
                }
            }
            for (int i = 0; i < ElementsRectangle.Count; i++)
            {
                KeyValuePair<string, ScreenRectangle> keyValuePair = ElementsRectangle.ElementAt(i);
                ScreenRectangle thisRectangle = keyValuePair.Value;
                if (CooldownTimer > 0)
                {
                    continue;
                }
                if (thisRectangle.HandleEvent(mouseRectangle, mouseState))
                {
                    CooldownTimer = CooldownMilliseconds;
                }
            }
        }
        public void DrawElements(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointWrap,
                DepthStencilState.DepthRead,
                RasterizerState.CullNone
            );


            for (int i = 0; i < ElementsImage.Count; i++)
            {
                KeyValuePair<string, ScreenImage> keyValuePair = ElementsImage.ElementAt(i);
                keyValuePair.Value.Draw(spriteBatch);
            }
            for (int i = 0; i < ElementsRectangle.Count; i++)
            {
                KeyValuePair<string, ScreenRectangle> keyValuePair = ElementsRectangle.ElementAt(i);
                keyValuePair.Value.Draw(spriteBatch);
            }
            for (int i = 0; i < ElementsLine.Count; i++)
            {
                KeyValuePair<string, ScreenLine> keyValuePair = ElementsLine.ElementAt(i);
                keyValuePair.Value.Draw(spriteBatch);
            }
            for (int i = 0; i < ElementsText.Count; i++)
            {
                KeyValuePair<string, ScreenText> keyValuePair = ElementsText.ElementAt(i);
                keyValuePair.Value.Draw(spriteBatch);
            }
            for (int i = 0; i < ElementsTable.Count; i++)
            {
                KeyValuePair<string, ScreenTable> keyValuePair = ElementsTable.ElementAt(i);
                keyValuePair.Value.Draw(spriteBatch);
            }

            if (IsMouseVisible)
            {
                spriteBatch.Draw(
                    DisplayFacade.MISCImages["cursor"],
                    MousePosition,
                    null,
                    Color.White,
                    0,
                    Vector2.Zero,
                    0.5f,
                    SpriteEffects.None,
                    0f
                );
            }

            spriteBatch.End();
        }
    }
}
