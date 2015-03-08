namespace CounterNinja
{
    internal static class Program
    {
        // The main entry point for the application.
        private static void Main(string[] args)
        {
            using (CounterNinjaGame game = new CounterNinjaGame())
            {
                game.Run();
            }
        }
    }
}