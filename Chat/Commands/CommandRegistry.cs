namespace TestingUtils.Chat.Commands;

public static class CommandRegistry
{
    public static readonly IChatCommandHandler[] CommandHandlers =
    {
        new GiveCommandHandler(),
        new FeedCommandHandler(),
        new HealCommandHandler(),
        new KillCommandHandler(),
        new SetSkillCommandHandler(),
        new MaxSkillsCommandHandler(),
        new ResetSkillsCommandHandler(),
        new ClearInvCommandHandler(),
        new HelpCommandHandler(),
        new ToggleInvincibleCommandHandler()
    };
}