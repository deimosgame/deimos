using System;

namespace Deimos
{
#if WINDOWS
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (DeimosGame game = new DeimosGame())
            {
                game.Run();
            }
        }
    }
#endif
}

