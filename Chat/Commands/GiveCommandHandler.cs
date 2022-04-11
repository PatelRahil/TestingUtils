using CoreLib;

namespace TestingUtils.Chat.Commands;

public class GiveCommandHandler: IChatCommandHandler
{
    public string Execute(string[] parameters)
    {
        var argType = parameters[0].Split(':');
        TestingUtilsPlugin.logSource.LogInfo("Give command args: ");
        switch (argType.Length)
        {
            case 2 when argType[0] == "id":
                try
                {
                    var objId = (ObjectID)int.Parse(argType[1]);
                    try
                    {
                        var count = (parameters.Length == 2) ? int.Parse(parameters[1]) : 0;
                        AddToInventory(objId, count);
                        return $"\nSuccessfully added {count} {argType[1]}";
                    }
                    catch
                    {
                        AddToInventory(objId, 1);
                        return $"\nSuccessfully added 1 {argType[1]}";
                    }
                }
                catch { return "\nInvalid object Id"; }
            case 2 when argType[0] == "name":
            {
                ObjectID.TryParse(argType[1], out ObjectID objId);
                try
                {
                    var count = (parameters.Length == 2) ? int.Parse(parameters[1]) : 0;
                    AddToInventory(objId, count);
                    return $"\nSuccessfully added {count} {argType[1]}";
                }
                catch
                {
                    AddToInventory(objId, 1);
                    return $"\nSuccessfully added {1} {argType[1]}";
                }
            }
            default:
                TestingUtilsPlugin.logSource.LogInfo("Invalid command");
                return "\nInvalid command. Try /give name:{itemName} {count?} or /give id:{itemId} {count?}";
        }
    }

    public string GetDescription()
    {
        return
            "Give yourself any item. Options:\n/give name:{itemName} {count?}\n/give id:{itemId} {count?}\nThe count parameter defaults to 1.";
    }

    public string[] GetTriggerNames()
    {
        return new[] {"give"};
    }
    
    private void AddToInventory(ObjectID objId, int amount)
    {
        TestingUtilsPlugin.logSource.LogInfo("Amount: " + amount);
        PlayerController player = GameManagers.GetMainManager().player;
        if (player == null) return;
        InventoryHandler handler = player.playerInventoryHandler;
        if (handler == null) return;
        handler.CreateItem(0, objId, amount, player.WorldPosition, 0);
    }
}