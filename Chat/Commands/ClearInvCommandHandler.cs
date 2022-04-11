using CoreLib;

namespace TestingUtils.Chat.Commands;

public class ClearInvCommandHandler:IChatCommandHandler
{
    public string Execute(string[] parameters)
    {
        var player = Players.GetCurrentPlayer();
        if (player == null) return "\nThere was an issue, try again later.";
        var handler = player.playerInventoryHandler;
        if (handler == null) return "\nThere was an issue, try again later.";

        for (var i = 0; i < handler.size; i++)
        {
            var objId = handler.GetObjectData(i).objectID;
            handler.DestroyObject(i, objId);
        }
        return "\nSuccessfully cleared inventory";
    }

    public string GetDescription()
    {
        return "Clear player inventory.";
    }

    public string[] GetTriggerNames()
    {
        return new[] {"clearInv", "clearInventory"};
    }
}