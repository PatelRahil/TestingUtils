using CoreLib;

namespace TestingUtils.Chat.Commands;

public class HealCommandHandler: IChatCommandHandler
{
    public string Execute(string[] parameters)
    {
        if (parameters.Length != 1) return Heal();
        try
        {
            var amount = int.Parse(parameters[0]);
            return Heal(amount);
        }
        catch { return Heal(); }
    }

    public string GetDescription()
    {
        return "Use /heal to fully heal player.\n/heal {amount} for a specific amount.";
    }

    public string[] GetTriggerNames()
    {
        return new[] { "heal" };
    }

    private string Heal(int amount = -1)
    {
        var player = Players.GetCurrentPlayer();
        if (player == null) return "\nThere was an issue, try again later.";
        var healAmount = amount < 0 ? (player.GetMaxHealth() - player.currentHealth) : amount;
        player.HealPlayer(healAmount);
        return $"\nSuccessfully healed {healAmount} HP";
    }
}