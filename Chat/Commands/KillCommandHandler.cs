using CoreLib;

namespace TestingUtils.Chat.Commands;

public class KillCommandHandler:IChatCommandHandler
{
    public string Execute(string[] parameters)
    {
        var player = Players.GetCurrentPlayer();
        if (player == null) return "\nThere was an issue, try again later.";
        player.Kill();
        return "\nSuccessfully killed player";
    }

    public string GetDescription()
    {
        return "Kills the player. Kinda self explanatory.";
    }

    public string[] GetTriggerNames()
    {
        return new[] {"kill"};
    }
}