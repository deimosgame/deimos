using System;
using System.Threading;

namespace Deimos
{
#if WINDOWS
    static class Program
    {
        public static string PlayerEmail = "test@mail.com";
        public static string PlayerToken = "23601116db467d50c6d8722dffb378b9";
        public static string PlayerTokenRefresh = "q9weiuashd";
        public static string Username;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "main";

            try
            {
                PlayerEmail = args[0];
                PlayerToken = args[1];
                PlayerTokenRefresh = args[2];
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

