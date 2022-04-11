
namespace TestingUtils.Chat.Commands;
public interface IChatCommandHandler
{
    /// <summary>
    /// Execute command & return feedback
    /// </summary>
    string Execute(string[] parameters);
    string GetDescription();
    string[] GetTriggerNames();
}