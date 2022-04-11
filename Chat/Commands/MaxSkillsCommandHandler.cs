using CoreLib;

namespace TestingUtils.Chat.Commands;

public class MaxSkillsCommandHandler: IChatCommandHandler
{
    public string Execute(string[] parameters)
    {
        var player = Players.GetCurrentPlayer();
        if (player == null) return "\nThere was an issue, try again later.";
        player.MaxOutAllSkills();
        return "\nSuccessfully maxed all skills";
    }

    public string GetDescription()
    {
        return "Maxes out all skills.";
    }

    public string[] GetTriggerNames()
    {
        return new[] {"maxSkills"};
    }
}