using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    static class GeneralFacade
    {
        public static DeimosGame Game;
        public static ContentManager TempContent;
        public static SceneManager SceneManager;
        public static Config Config;

        public static string Uniqid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
