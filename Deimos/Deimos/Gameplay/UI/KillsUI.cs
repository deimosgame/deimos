using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class KillsUI
    {
        private float countdown = 0;
        private float Countdown
        {
            get { return countdown; }
            set
            {
                if (value <= 0)
                {
                    // Reset
                    RecentDeaths = new List<Tuple<string, string, string, bool>>();
                }
                countdown = value;
            }
        }

        public List<Tuple<string, string, string, bool>> RecentDeaths = new List<Tuple<string, string, string, bool>>();

        const int MarginTop = 20;
        const int MarginRight = 20;
        const int Padding = 5;


        public KillsUI()
        {
            Add("Mandor", "knife", "GeT_NigLo");
            Add("Mandor", "rifle", "Artemis");
            Add("Vomuseid", "pistol", "Mandor", true);
        }


        public void Add(string killer, string weapon, string victim, bool killstreak = false)
        {
            RecentDeaths.Add(new Tuple<string,string, string, bool>(killer, weapon, victim, killstreak));
            Countdown = 5;
        }

        public void Update(GameTime gameTime)
        {
            if (Countdown > 0)
            {
                Countdown -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Countdown <= 0)
            {
                return;
            }

            int width = GeneralFacade.Game.GraphicsDevice.Viewport.Width;
            int height = GeneralFacade.Game.GraphicsDevice.Viewport.Height;
            int fontWidth = (int)DisplayFacade.KillsFont.MeasureString("a").X;
            int fontHeight = (int)DisplayFacade.KillsFont.MeasureString("jL").Y;

            spriteBatch.Begin();


            for (int i = 0; i < RecentDeaths.Count(); i++)
            {
                var kill = RecentDeaths[i];
                string t = kill.Item1 + "  > " + kill.Item2 + " >  " + kill.Item3;
                if (kill.Item4)
                {
                    t = "## " + kill.Item1 + "  > " + kill.Item2 + " >  " + kill.Item3 + " ##";
                }
                spriteBatch.DrawString(
                    DisplayFacade.KillsFont,
                    t,
                    new Vector2(
                        width - (int)DisplayFacade.UIFont.MeasureString(t).X - MarginRight, 
                        MarginTop + (fontHeight * i) + (Padding * i)
                    ),
                    Color.White
                );
            }


            spriteBatch.End();
        }

    }
}
