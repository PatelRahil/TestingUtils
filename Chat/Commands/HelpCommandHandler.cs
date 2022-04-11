using System;
using System.Linq;
using PlayerCommand;

namespace TestingUtils.Chat.Commands;

public class HelpCommandHandler: IChatCommandHandler
{
    public string Execute(string[] parameters)
    {
        switch (parameters.Length)
        {
            case 0:
                var commandsString = CommandRegistry.CommandHandlers.Aggregate("\n", (str, handler) => handler.GetTriggerNames().Length > 0 ? $"{str}\n{handler.GetTriggerNames()[0]}" : str);
                return $"\n\nUse /help {{command}} for more information.\nCommands:{commandsString}";
            case 1:
                try
                {
                    var validCommandHandler = Array.Find(CommandRegistry.CommandHandlers,
                        element => element.GetTriggerNames().Contains(parameters[0]));
                    return "\n" + validCommandHandler.GetDescription();
                } catch { return "This command does not exist. Do /help to view all commands.";}

            default:
                return "\nInvalid arguments. Do /help to view all commands.";
        }
    }

    public string GetDescription()
    {
        return "Use /help {command} for more information on a command.";
    }

    public string[] GetTriggerNames()
    {
        return new[] {"help"};
    }
}