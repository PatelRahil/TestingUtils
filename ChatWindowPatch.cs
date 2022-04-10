using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using CoreLib;
using System.Collections;
using UnityEngine;
using TestingUtils;

namespace TestingUtils
{
    [HarmonyPatch]
    internal class ChatWindowPatch
    { 
        
        internal static class Commands
        {
            public const string
                Give = "/give",
                ClearInv = "/clearinv",
                Heal = "/heal",
                Feed = "/feed",
                MaxSkills = "/maxSkills",
                ResetSkills = "/resetSkills",
                SetSkill = "/setSkill",
                ToggleInvincible = "/invincible",
                Kill = "/kill",
                Help = "/help";
        }

        [HarmonyPatch(typeof(ChatWindow), nameof(ChatWindow.AddPugText))]
        [HarmonyPrefix]
        static bool ReadPugText(ChatWindow.MessageTextType type, ref PugText text)
        {
            TestingUtilsPlugin.logSource.LogInfo("Input text: " + text.textString);
            var textStr = text.textString;
            var feedback = "";
            var args = textStr.Split(' ');
            if (args.Length >= 2 && args[0] == Commands.Give)
            {
                feedback = give(args);
            }
            else if (args.Length >= 1 && args[0] == Commands.ClearInv)
            {
                feedback = clearInventory();
            }
            else if (args.Length >= 1 && args[0] == Commands.Heal)
            {
                if (args.Length == 2)
                {
                    try
                    {
                        var amount = int.Parse(args[1]);
                        feedback = heal(amount);
                    }
                    catch { feedback = heal(); }
                }
                else { feedback = heal(); }
            }
            else if (args.Length >= 1 && args[0] == Commands.Feed)
            {
                if (args.Length == 2)
                {
                    try
                    {
                        var amount = int.Parse(args[1]);
                        feedback = feed(amount);
                    }
                    catch { feedback = feed(); }
                }
                else { feedback = feed(); }
            }
            else if (args.Length >= 1 && args[0] == Commands.MaxSkills)
            {
                feedback = maxSkills();
            }
            else if (args.Length >= 1 && args[0] == Commands.ResetSkills)
            {
                feedback = resetSkills();
            }
            else if (args.Length >= 1 && args[0] == Commands.SetSkill)
            {
                feedback = setSkill(args);
            }
            else if (args.Length >= 1 && args[0] == Commands.ToggleInvincible)
            {
                feedback = toggleInvincible();
            }
            else if (args.Length >= 1 && args[0] == Commands.Kill)
            {
                feedback = kill();
            }
            else if (args.Length >= 1 && args[0] == Commands.Help)
            {
                feedback = help(args);
            }
            text.textString = textStr + feedback;
            text.Render();
            return true;
        }

        static string give(string[] args)
        {

            var argType = args[1].Split(':');
            TestingUtilsPlugin.logSource.LogInfo("Give command args: ");
            foreach (var arg in args)
            {
                TestingUtilsPlugin.logSource.LogInfo(arg);
            }
            if (argType.Length == 2 && argType[0] == "id")
            {
                try
                {
                    var objId = (ObjectID)int.Parse(argType[1]);
                    try
                    {
                        var count = (args.Length == 3) ? int.Parse(args[2]) : 0;
                        addToInventory(objId, count);
                        return $"\nSuccessfully added {count} {argType[1]}";
                    }
                    catch
                    {
                        addToInventory(objId, 1);
                        return $"\nSuccessfully added 1 {argType[1]}";
                    }
                }
                catch { return "\nInvalid object Id"; }
            }
            else if (argType.Length == 2 && argType[0] == "name")
            {
                ObjectID.TryParse(argType[1], out ObjectID objId);
                try
                {
                    var count = (args.Length == 3) ? int.Parse(args[2]) : 0;
                    addToInventory(objId, count);
                    return $"\nSuccessfully added {count} {argType[1]}";
                }
                catch
                {
                    addToInventory(objId, 1);
                    return $"\nSuccessfully added {1} {argType[1]}";
                }

            }
            else
            {
                TestingUtilsPlugin.logSource.LogInfo("Invalid command");
                return "\nInvalid command. Try /give name:{itemName} {count?} or /give id:{itemId} {count?}";
            }
        }

        static void addToInventory(ObjectID objId, int amount)
        {
            TestingUtilsPlugin.logSource.LogInfo("Amount: " + amount);
            PlayerController player = GameManagers.GetMainManager().player;
            if (player == null) return;
            InventoryHandler handler = player.playerInventoryHandler;
            if (handler == null) return;
            handler.CreateItem(0, objId, amount, player.WorldPosition, 0);
        }

        static string clearInventory()
        {
            PlayerController player = Players.GetCurrentPlayer();
            if (player == null) return "\nThere was an issue, try again later.";
            InventoryHandler handler = player.playerInventoryHandler;
            if (handler == null) return "\nThere was an issue, try again later.";

            for (int i = 0; i < handler.size; i++)
            {
                var objId = handler.GetObjectData(i).objectID;
                handler.DestroyObject(i, objId);
            }
            return "\nSuccessfully cleared inventory";
        }

        static string heal(int amount = -1)
        {
            PlayerController player = Players.GetCurrentPlayer();
            if (player == null) return "\nThere was an issue, try again later.";
            var healAmount = amount < 0 ? (player.GetMaxHealth() - player.currentHealth) : amount;
            player.HealPlayer(healAmount);
            return $"\nSuccessfully healed {healAmount} HP";
        }

        static string feed(int amount = -1)
        {
            PlayerController player = Players.GetCurrentPlayer();
            if (player == null) return "\nThere was an issue, try again later.";
            var hungerAmount = amount < 0 ? (100 - player.hungerComponent.hunger) : amount;
            player.AddHunger(hungerAmount);
            return $"\nSuccessfully fed {hungerAmount} food";
        }

        static string maxSkills()
        {
            PlayerController player = Players.GetCurrentPlayer();
            if (player == null) return "\nThere was an issue, try again later.";
            player.MaxOutAllSkills();
            return "\nSuccessfully maxed all skills";
        }

        static string resetSkills()
        {
            PlayerController player = Players.GetCurrentPlayer();
            if (player == null) return "\nThere was an issue, try again later.";
            player.ResetAllSkills();
            return "\nSuccessfully reset all skills";
        }

        static string setSkill(string[] args)
        {
            if (args.Length == 3)
            {
                try
                {
                    int level = int.Parse(args[2]);
                    if (level >= 0 && level <= 100)
                    {
                        PlayerController player = Players.GetCurrentPlayer();
                        if (player == null) return "\nThere was an issue, try again later.";

                        switch (args[1])
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
                        return $"\n{args[1]} successfully set to level {level}";
                    }
                    else { return "\nInvalid level provided. Should be a number 0-100."; }
                }
                catch { return "\nInvalid level provided. Should be a number 0-100."; }
            }
            else
            {
                return "\nInvalid arguments for command. Correct format:\n/setSkill {skillName} {level}";
            }
        }

        static string toggleInvincible()
        {
            PlayerController player = Players.GetCurrentPlayer();
            if (player == null) return "\nThere was an issue, try again later.";
            player.SetInvincibility(!player.invincible);
            return $"\nSuccessfully set invinsibility to {!player.invincible}";
        }

        static string kill()
        {
            PlayerController player = Players.GetCurrentPlayer();
            if (player == null) return "\nThere was an issue, try again later.";
            player.Kill();
            return "\nSuccessfully killed player";
        }

        static string help(string[] args)
        {
            if (args.Length == 1)
            {
                return "\n\nUse /help {command} for more information.\nCommands:\n/give\n/clearinv\n/heal\n/feed\n/kill\n/maxSkills\n/resetSkills\n/setSkill\n/invincible";
            }
            else if (args.Length == 2)
            {
                switch ("/" + args[1])
                {
                    case Commands.Give:
                        return "\n\nGive yourself any item. Options:\n/give name:{itemName} {count?}\n/give id:{itemId} {count?}\nThe count parameter defaults to 1.";
                    case Commands.ClearInv:
                        return "\n\nClear player inventory.";
                    case Commands.Heal:
                        return "\n\nUse /heal to fully heal player.\n/heal {amount} for a specific amount.";
                    case Commands.Feed:
                        return "\n\nUse /feed to fully feed player.\n/feed {amount} for a specific amount.";
                    case Commands.Kill:
                        return "\n\nKills the player. Kinda self explanatory.";
                    case Commands.MaxSkills:
                        return "\n\nMaxes out all skills.";
                    case Commands.ResetSkills:
                        return "\n\nResets all skills to 0.";
                    case Commands.SetSkill:
                        return "\n\nSets the given skill to the given level.\n/setSkill {skillName} {level}";
                    case Commands.ToggleInvincible:
                        return "\n\nToggles invincibility for the player.";
                    default:
                        return "\n\nThis command does not exist. Do /help to view all commands.";
                }
            }
            else { return "\nInvalid arguments. Do /help to view all commands."; }
        }
    }
}