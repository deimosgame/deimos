using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deimos
{
    class BotManager
    {
        // In-game bots list
        public Dictionary<string, Bot> BotList;
        private float dt;

        // Constructor
        public BotManager()
        {
            BotList = new Dictionary<string, Bot>();
        }

        // Methods
        public void AddBot(Bot bot, string name)
        {
            if (!BotList.ContainsKey(name))
            {
                BotList.Add(name, bot);
                BotList[name].Name = name;
            }
        }

        public void RemoveBot(string name)
        {
            if (BotList.ContainsKey(name))
            {
                BotList.Remove(name);
            }
        }

        public Bot GetBot(string name)
        {
            if (BotList.ContainsKey(name))
            {
                return BotList[name];
            }
            else
            {
                return null;
            }
        }

        public void Update(GameTime gameTime)
        {
            dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            BotList["Padawan"].Update(dt);
        }
    }
}
