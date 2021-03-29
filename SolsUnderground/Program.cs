using System;

namespace SolsUnderground
{
    public static class Program
    {
        public static Random rng = new Random();

        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}
