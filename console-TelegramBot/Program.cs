namespace console_TelegramBot_test 
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bot = Bot.CreateInstance();
            var stopMessage = string.Empty;
            while (stopMessage != "stop")
            {
                stopMessage = Console.ReadLine();
            }
            bot.Cts.Cancel();
        }
    }
}
