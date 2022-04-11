using CoreLib;

namespace TestingUtils.Chat.Commands;

public class ToggleInvincibleCommandHandler:IChatCommandHandler
{
    public string Execute(string[] parameters)
    {
        var player = Players.GetCurrentPlayer();
        if (player == null) return "\nThere was an issue, try again later.";
        player.SetInvincibility(!player.invincible);
        return $"\nSuccessfully set invincibility to {!player.invincible}";
    }

    public string GetDescription()
    {
        return "Toggles invincibility for the player.";
    }

    public string[] GetTriggerNames()
    {
        return new[] {"invincible"};
    }
}