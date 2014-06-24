using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class ScoresUI
    {
        private Dictionary<string, Dictionary<string, PlayerScore>> Scores 
            = new Dictionary<string, Dictionary<string, PlayerScore>>();

        private string Map = "Default map";

        public bool Show = false;

        const float WidthPercentage = 0.6f;
        const int MarginTop = 50;
        const int Padding = 5;

        private Color[] TeamColors = new Color[]{
            Color.Red,
            Color.LightBlue,
            Color.Green
        };

        public ScoresUI()
        {
            AddTeam("FFA");
        }


        public void AddTeam(string name)
        {
            Scores.Add(name, new Dictionary<string, PlayerScore>());
        }

        public void RemoveTeam(string name)
        {
            Scores.Remove(name);
        }

        public void AddPlayer(string teamName, string playerName)
        {
            PlayerScore playerScore = new PlayerScore();

            Scores[teamName].Add(playerName, playerScore);
        }

        public PlayerScore GetPlayerScore(string teamName, string playerName)
        {
            return Scores[teamName][playerName];
        }

        public void ChangeMap(string name)
        {
            Map = name;
        }

        public void RemovePlayer(string team, string name)
        {
            if (Scores.ContainsKey(team))
            {
                if (Scores[team].ContainsKey(name))
                {
                    Scores[team].Remove(name);
                }
            }
        }

        public void AddKill(string team, string name)
        {
            if (Scores.ContainsKey(team))
            {
                if (Scores[team].ContainsKey(name))
                {

                    Scores[team][name].Kills++;
                }
            }
        }

        public void AddDeath(string team, string name)
        {
            if (Scores.ContainsKey(team))
            {
                if (Scores[team].ContainsKey(name))
                {
                    Scores[team][name].Deaths++;
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Show)
            {
                return;
            }

            int width = GeneralFacade.Game.GraphicsDevice.Viewport.Width;
            int height = GeneralFacade.Game.GraphicsDevice.Viewport.Height;
            int fontWidth = (int)DisplayFacade.TableFont.MeasureString("a").X;
            int fontHeight = (int)DisplayFacade.TableFont.MeasureString("jL").Y;

            int availableWidth = (int)(width * WidthPercentage);
            int availableHeight = (int)(height - MarginTop * 2);
            int marginLeft = (width - availableWidth) / 2;
            int killsWidth = (int)DisplayFacade.TableFont.MeasureString("99").X + 20;
            int nameWidth = availableWidth - (killsWidth * 2);

            spriteBatch.Begin();


            spriteBatch.Draw(
                DisplayFacade.ScreenElementManager.DummyTexture,
                new Rectangle(
                    marginLeft,
                    MarginTop,
                    availableWidth,
                    availableHeight
                ),
                new Color(0, 0, 0, 0.5f)
            );

            spriteBatch.DrawString(
                    DisplayFacade.TableFont,
                    "-- " + Map,
                    new Vector2(
                        marginLeft + Padding,
                        MarginTop + Padding
                    ),
                    Color.LightGray
             );

            spriteBatch.DrawString(
                DisplayFacade.TableFont,
                    "K",
                    new Vector2(
                        marginLeft + nameWidth + Padding,
                        MarginTop + Padding
                    ),
                    Color.LightGray
            );
            spriteBatch.DrawString(
                DisplayFacade.TableFont,
                    "D",
                    new Vector2(
                        marginLeft + nameWidth + killsWidth + Padding,
                        MarginTop + Padding
                    ),
                    Color.LightGray
            );

            ScreenLine line = new ScreenLine(
                new Vector2(marginLeft, MarginTop + fontHeight + Padding * 2),
                new Vector2(marginLeft + availableWidth, MarginTop + fontHeight + Padding * 2),
                1,
                Color.Gray,
                3
            );
            line.Draw(spriteBatch);
            line.LineWidth = 1;

            int startHeight = MarginTop + fontHeight + Padding * 2 + 10;
            int lastI = 0;

            for (int i = 0; i < Scores.Count(); i++)
            {
                var thisElement = Scores.ElementAt(i);

                spriteBatch.DrawString(
                    DisplayFacade.TableFont,
                    "   " + thisElement.Key,
                    new Vector2(marginLeft + Padding, startHeight + (Padding * 2 + fontHeight) * (i + lastI)),
                    TeamColors[i % 3]
                );

                line.Start = new Vector2(marginLeft, startHeight + (Padding * 2 + fontHeight) * (i + 1 + lastI) - Padding);
                line.End = new Vector2(marginLeft + availableWidth, startHeight + (Padding * 2 + fontHeight) * (i + 1 + lastI) - Padding);
                line.Draw(spriteBatch);


                for (int i2 = 0; i2 < thisElement.Value.Count(); i2++)
                {
                    var thisPlayer = thisElement.Value.ElementAt(i2);

                    spriteBatch.DrawString(
                        DisplayFacade.TableFont,
                        thisPlayer.Key,
                        new Vector2(marginLeft + Padding, startHeight + (Padding * 2 + fontHeight) * (i + lastI + 1 + i2)),
                        Color.White
                    );
                    spriteBatch.DrawString(
                        DisplayFacade.TableFont,
                        thisPlayer.Value.Kills.ToString(),
                        new Vector2(marginLeft + Padding + nameWidth, startHeight + (Padding * 2 + fontHeight) * (i + lastI + 1 + i2)),
                        Color.White
                    );
                    spriteBatch.DrawString(
                        DisplayFacade.TableFont,
                        thisPlayer.Value.Deaths.ToString(),
                        new Vector2(marginLeft + Padding + nameWidth + killsWidth, startHeight + (Padding * 2 + fontHeight) * (i + lastI + 1 + i2)),
                        Color.White
                    );
                }

                lastI += thisElement.Value.Count();

                if (thisElement.Value.Count() == 0)
                {
                    spriteBatch.DrawString(
                        DisplayFacade.TableFont,
                        "There is no player in this team.",
                        new Vector2(marginLeft + Padding, startHeight + (Padding * 2 + fontHeight) * (i + lastI + 1)),
                        Color.White
                    );

                    lastI += 1;
                }
            }

            spriteBatch.End();
        }
    }
}
