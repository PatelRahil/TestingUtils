using System;
using System.Linq;
using HarmonyLib;
using TestingUtils.Chat.Commands;

namespace TestingUtils.Chat
{
    [HarmonyPatch]
    internal class ChatWindowPatch
    {
        private const string CommandPrefix = "/";

        [HarmonyPatch(typeof(ChatWindow), nameof(ChatWindow.AddPugText))]
        [HarmonyPrefix]
        static bool ReadPugText(ChatWindow.MessageTextType type, ref PugText text)
        {
            TestingUtilsPlugin.logSource.LogInfo("Input text: " + text.textString);
            var textStr = text.textString;
            var feedback = "";
            var args = textStr.Split(' ');
            if (args.Length < 1 || !args[0].StartsWith(CommandPrefix)) return true;
            try
            {
                var commandHandler = Array.Find(CommandRegistry.CommandHandlers,
                    handler => handler.GetTriggerNames().Select(name => CommandPrefix + name).Contains(args[0]));
                var parameters = args.Skip(1).ToArray();
                feedback = commandHandler.Execute(parameters);
                text.textString = textStr + feedback;
                text.Render();
                return true;
            }
            catch
            {
                feedback = "\n\nThat command does not exist yet.";
                text.textString = textStr + feedback;
                text.Render();
                return true;
            }
        }
    }
}