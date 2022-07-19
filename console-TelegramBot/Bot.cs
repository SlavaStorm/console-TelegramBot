using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace console_TelegramBot_test;

public class Bot
{
    private readonly ReceiverOptions receiverOptions = new()
    {
        AllowedUpdates = Array.Empty<UpdateType>()
    };

    private readonly BotLogic _botLogic = new(); 
    public TelegramBotClient BotClient;

    private static Bot _bot;

    public CancellationTokenSource Cts = new();
    /// <summary>
    /// Конструктор иницализации бота
    /// </summary>
    private Bot()
    {
        BotClient = new TelegramBotClient("Введите id бота");
        BotClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: Cts.Token
        );

        Task.Run(Start).ConfigureAwait(false);
    }

    private async Task Start()
    {
        var me = await BotClient.GetMeAsync();

        Console.WriteLine($"Start listening for @{me.Username}");
    }

    public static Bot CreateInstance()
    {
        return _bot ??= new Bot();
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var messageText = await _botLogic.Reply(botClient, update, cancellationToken);
    }

    Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}