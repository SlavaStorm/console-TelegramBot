using Telegram.Bot;
using Telegram.Bot.Types;

namespace console_TelegramBot_test;

public class BotLogic
{
    private static List<long> _usersList = new () {/*Тех кому отвечать привет*/};
    private static List<string> _blackUsersList = new() {/*Тех кого посылать*/};

    public async Task<Message> Reply(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return null;
        if (message.Text is not { } messageText)
            return null;

        var chatId = message.Chat.Id;
        var username = message.Chat.Username;

        Console.WriteLine($"Received a '{messageText}' message in chat {chatId} user {username}.");
        string replyMessage = null;

        if (_usersList.Contains(chatId))
            replyMessage = "Привет, " + (message.Chat.FirstName ?? message.Chat.LastName) + Environment.NewLine;
        else if (_blackUsersList.Contains(username))
            replyMessage = (message.Chat.FirstName ?? message.Chat.LastName) + ", пошел в жопу :D" + Environment.NewLine;

        switch (messageText)
        {
            case "/start":
                //replyMessage += "Отправте список ингридиентов через пробел чтобы получить рецепты";
                break;
            default:
                break;
        }

        var sentMessage = await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: replyMessage ?? "You said:\n" + messageText,
            cancellationToken: cancellationToken);
        return sentMessage;
    }
}