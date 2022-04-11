using CoreLib;

namespace TestingUtils.Chat.Commands;

public class FeedCommandHandler: IChatCommandHandler
{
    public string Execute(string[] parameters)
    {
        if (parameters.Length != 1) return Feed();
        try
        {
            var amount = int.Parse(parameters[0]);
            return Feed(amount);
        }
        catch { return Feed(); }
    }

    public string GetDescription()
    {
        return "Use /feed to fully feed player.\n/feed {amount} for a specific amount.";
    }

    public string[] GetTriggerNames()
    {
        return new[] { "feed" };
    }

    static string Feed(int amount = -1)
    {
        PlayerController player = Players.GetCurrentPlayer();
        if (player == null) return "\nThere was an issue, try again later.";
        var hungerAmount = amount < 0 ? (100 - player.hungerComponent.hunger) : amount;
        player.AddHunger(hungerAmount);
        return $"\nSuccessfully fed {hungerAmount} food";
    }
}