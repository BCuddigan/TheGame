using System;

namespace TheGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Selector game = new Selector())
            {
                game.Run();
            }
        }
    }
#endif
}

