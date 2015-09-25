using System;

namespace Step_Creator
{
#if WINDOWS || XBOX
    static class Program
    {
        
        [STAThread]
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

