using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Torch;
using VRage.Utils;
using static SEInformationBridge.Factions;
using System.Configuration;


namespace SEInformationBridge
{
    public static class ChatLog
    {
        public static List<ChatMessage> Messages = new List<ChatMessage>();
        public static void Setup()
        {
            MyMultiplayer.Static.ChatMessageReceived += OnMessage;
        }

        private static void OnMessage(ulong steamUserId, string messageText, ChatChannel channel, long targetId, string customAuthorName)
        {
           Messages.Add(new ChatMessage(steamUserId, messageText, channel, targetId));
        }

        public static List<ChatMessage> GetChat()
        {
            if (Plugin.TorchInstance.CurrentSession == null)
                return null;

            return Messages;
        }


        public class ChatMessage
        {
            public ulong SenderSteamId { get; set; }
            public string SenderName { get; set; }
            public string MessageText { get; set; }
            public string ChatChannel { get; set; }
            public string FactionName { get; set; }
            public long TargetId { get; set; }

            public ChatMessage(ulong steamId, string message, Sandbox.Game.Gui.ChatChannel channel, long targetId)
            {
                SenderSteamId = steamId;
                MessageText = message;
                MyPlayer player = null;
                MySession.Static.Players.TryGetPlayerBySteamId(SenderSteamId, out player);
                if (player != null && channel != Sandbox.Game.Gui.ChatChannel.GlobalScripted)
                    SenderName = player.DisplayName;
                else
                    SenderName = string.Empty;

                if (channel == Sandbox.Game.Gui.ChatChannel.Faction)
                {
                    var faction = MySession.Static.Factions.TryGetFactionById(targetId);

                    if (faction != null)
                        FactionName = faction.Name;
                    else
                        FactionName = "None";
                }

                ChatChannel = channel.ToString();
                TargetId = targetId;
            }
        }



    }
}
