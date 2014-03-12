using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    static class DisplayFacade
    {
        public static SpriteBatch SpriteBatch;
        public static Camera Camera;
        public static DeferredRenderer Renderer;
        public static DebugScreen DebugScreen;
        public static ScreenElementManager ScreenElementManager;
    }
}
