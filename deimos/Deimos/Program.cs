using System;
using System.Threading;

namespace Deimos
{
#if WINDOWS
    static class Program
    {
        public static string PlayerEmail = "erenus@mail.com";
        public static string PlayerToken = "23601a116dbd467d50c6d8722dffb378b8";
        public static string PlayerTokenRefresh = "q9weiduadhd";
        public static string Username = "vom";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "main";

            NetworkFacade.IsMultiplayer = true;
            try
            {
                PlayerEmail = args[0];
                PlayerTokenRefresh = args[1];
                Username = args[2];
                NetworkFacade.IsMultiplayer = true;
            }
            catch (Exception)
            {
                // This is because it hasn't been executed with enough parameters
                // Exiting
                //return;
            }

            using (DeimosGame game = new DeimosGame())
            {
                game.Run();
            }
        }
    }
#endif
}

