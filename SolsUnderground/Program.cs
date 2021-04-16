using System;

namespace SolsUnderground
{
    public static class Program
    {
        public static Random rng = new Random();
        public static bool godMode = false;

        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}
