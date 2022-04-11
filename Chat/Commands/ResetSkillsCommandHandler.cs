using CoreLib;

namespace TestingUtils.Chat.Commands;

public class ResetSkillsCommandHandler:IChatCommandHandler
{
    public string Execute(string[] parameters)
    {
        var player = Players.GetCurrentPlayer();
        if (player == null) return "\nThere was an issue, try again later.";
        player.ResetAllSkills();
        return "\nSuccessfully reset all skills";
    }

    public string GetDescription()
    {
        return "Resets all skills to 0.";
    }

    public string[] GetTriggerNames()
    {
        return new[] {"resetSkills"};
    }
}