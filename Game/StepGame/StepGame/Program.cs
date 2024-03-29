using System;

namespace Step_Game
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Principal game = new Principal())
            {
                game.Run();
            }
        }
    }
#endif
}

