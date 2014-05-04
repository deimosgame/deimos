using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class ScreenTable : ScreenElement
    {
        List<string> Headers = new List<string>();
        public List<TableRow> Content = new List<TableRow>();

        int ColumnSize;
        int Padding;

        SpriteFont SpriteFont;

        Color BorderHeaderColor;
        Color BorderColor;
        Color BgColor;
        Color HeaderFontColor;
        Color FontColor;

        public ScreenTable(int posX, int posY, int zIndex, Color bgColor,
            Color borderHeaderColor, Color borderColor, Color headerFontColor,
            Color fontColor, SpriteFont font, int columnSize, int padding,
            List<string> headers, List<TableRow> content)
        {
            Pos = new Vector2(posX, posY);
            ZIndex = zIndex;

            Headers = headers; Content = content;

            ColumnSize = columnSize;
            Padding = padding;

            SpriteFont = font;

            BgColor = bgColor;
            HeaderFontColor = headerFontColor;
            FontColor = fontColor;
            BorderHeaderColor = borderHeaderColor;
            BorderColor = borderColor;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }

            if (Content.Count() == 0)
            {
                TableRow row = new TableRow();
                foreach (var item in Headers)
                {
                    row.Content.Add("-");
                }
                Content.Add(row);
            }

            int fontWidth = (int)SpriteFont.MeasureString("a").X;

            int rowHeight = (int)SpriteFont.MeasureString("bq").Y + Padding * 2;
            int tableHeight = rowHeight * (Content.Count() + 1);

            int rowWidth = ColumnSize + Padding * 2;
            int tableWidth = rowWidth * Content[0].Content.Count();

            // Drawing background
            ScreenRectangle bg = new ScreenRectangle((int)Pos.X, (int)Pos.Y, 1, tableWidth, tableHeight, BgColor);
            bg.Draw(spriteBatch);

            // Drawing header borders
            ScreenLine line = new ScreenLine(Pos, new Vector2(Pos.X + tableWidth, Pos.Y), 1, BorderHeaderColor);
            line.Draw(spriteBatch);
            line.End = new Vector2(Pos.X, Pos.Y + rowHeight);
            line.Draw(spriteBatch);
            line.Start = new Vector2(Pos.X + tableWidth, Pos.Y + rowHeight);
            line.Draw(spriteBatch);
            line.End = new Vector2(Pos.X + tableWidth, Pos.Y);
            line.Draw(spriteBatch);
            for (int i = 0; i < Content[0].Content.Count() - 1; i++)
            {
                line.Start = new Vector2(Pos.X + (rowWidth * (i + 1)), Pos.Y);
                line.End = new Vector2(Pos.X + (rowWidth * (i + 1)), Pos.Y + rowHeight);
                line.Draw(spriteBatch);
            }

            // Drawing other borders
            line.Color = BorderColor;
            line.Start = new Vector2(Pos.X + tableWidth, Pos.Y + rowHeight);
            line.End = new Vector2(Pos.X + tableWidth, Pos.Y + tableHeight);
            line.Draw(spriteBatch);
            line.Start = new Vector2(Pos.X, Pos.Y + tableHeight);
            line.Draw(spriteBatch);
            line.End = new Vector2(Pos.X, Pos.Y + rowHeight);
            line.Draw(spriteBatch);
            for (int i = 0; i < Content[0].Content.Count() - 1; i++)
            {
                line.Start = new Vector2(Pos.X + (rowWidth * (i + 1)), Pos.Y + rowHeight);
                line.End = new Vector2(Pos.X + (rowWidth * (i + 1)), Pos.Y + tableHeight);
                line.Draw(spriteBatch);
            }
            for (int i = 1; i < Content.Count(); i++)
            {
                line.Start = new Vector2(Pos.X, Pos.Y + rowHeight * (i + 1));
                line.End = new Vector2(Pos.X + tableWidth, Pos.Y + rowHeight * (i + 1));
                line.Draw(spriteBatch);
            }

            // Drawing header content
            ScreenText text = new ScreenText(0, 0, 1, SpriteFont, "", HeaderFontColor);
            for (int i = 0; i < Content[0].Content.Count(); i++)
            {
                string t = Headers[i];
                // If the text is too long, we'll need to truncate it
                t = HelperFacade.Helpers.Truncate(t, (int)(ColumnSize / fontWidth), "...");
                text.Pos = new Vector2(Pos.X + Padding + (rowWidth * i), Pos.Y + Padding);
                text.Text = t;
                text.Draw(spriteBatch);
            }


            // Drawing the content along with the setting the events
            text.Color = FontColor;
            for (int x = 0; x < Content.Count(); x++)
            {
                TableRow row = Content[x];


                for (int y = 0; y < row.Content.Count(); y++)
                {
                    string t = row.Content[y];
                    // If the text is too long, we'll need to truncate it
                    t = HelperFacade.Helpers.Truncate(t, (int)(ColumnSize / fontWidth), "...");
                    text.Pos = new Vector2(Pos.X + Padding + (rowWidth * y), Pos.Y + Padding + (rowHeight * (x + 1)));
                    text.Text = t;
                    text.Draw(spriteBatch);
                }
            }
        }
    }
}
