using CoreLib;

namespace TestingUtils.Chat.Commands;

public class SetSkillCommandHandler:IChatCommandHandler
{
    public string Execute(string[] parameters)
    {
        if (parameters.Length != 2)
            return "\nInvalid arguments for command. Correct format:\n/setSkill {skillName} {level}";
        try
        {
            var level = int.Parse(parameters[1]);
            if (level is < 0 or > 100) return "\nInvalid level provided. Should be a number 0-100.";
            var player = Players.GetCurrentPlayer();
            if (player == null) return "\nThere was an issue, try again later.";

            switch (parameters[0])
            {
                case "Vitality":
                    player.SetSkillLevel(SkillID.Vitality, level);
                    break;
                case "Fishing":
                    player.SetSkillLevel(SkillID.Fishing, level);
                    break;
                case "Running":
                    player.SetSkillLevel(SkillID.Running, level);
                    break;
                case "Cooking":
                    player.SetSkillLevel(SkillID.Cooking, level);
                    break;
                case "Mining":
                    player.SetSkillLevel(SkillID.Mining, level);
                    break;
                case "Gardening":
                    player.SetSkillLevel(SkillID.Gardening, level);
                    break;
                case "Crafting":
                    player.SetSkillLevel(SkillID.Crafting, level);
                    break;
                case "Melee":
                    player.SetSkillLevel(SkillID.Melee, level);
                    break;
                case "Range":
                    player.SetSkillLevel(SkillID.Range, level);
                    break;
                default:
                    return "\nInvalid skill provided.";
            }
            return $"\n{parameters[0]} successfully set to level {level}";

        }
        catch { return "\nInvalid level provided. Should be a number 0-100."; }
    }

    public string GetDescription()
    {
        return "Sets the given skill to the given level.\n/setSkill {skillName} {level}";
    }

    public string[] GetTriggerNames()
    {
        return new[] {"setSkill"};
    }
}