namespace AproximityChat.API;

using Exiled.API.Features;
using Newtonsoft.Json;
using PlayerRoles;
using System.IO;

public class FilterChat
{
    static public bool FilterChatWords(string message, Player player)
    {
        foreach (BannedWord blacklistmsg in JsonLists.BlackList)
        {
            if (message.ToLower().Contains(blacklistmsg.Word))
            {
                if (JsonLists.MutePcDict.ContainsKey(player.UserId))
                {
                    JsonLists.MutePcDict[player.UserId].TimeMute = JsonLists.MutePcDict[player.UserId].TimeMute.AddMinutes(blacklistmsg.Weight);
                    JsonLists.SaveData();
                }
                else
                {
                    MuteProxCLogic.Mute(player, blacklistmsg.Weight, "[AutoMute] Palavra feia!");
                    WebHookMsg.ReportMsg(message, blacklistmsg.Word, player);
                }
                return false;
            }
        }

        foreach (BannedWord blacklistmsg in JsonLists.CustomBlackList)
        {
            if (message.ToLower().Contains(blacklistmsg.Word))
            {
                if (JsonLists.MutePcDict.ContainsKey(player.UserId))
                {
                    JsonLists.MutePcDict[player.UserId].TimeMute = JsonLists.MutePcDict[player.UserId].TimeMute.AddMinutes(blacklistmsg.Weight);
                    JsonLists.SaveData();
                }
                else
                {
                    MuteProxCLogic.Mute(player, blacklistmsg.Weight, "[AutoMute] Palavra feia!");
                    WebHookMsg.ReportMsg(message, blacklistmsg.Word, player);
                }
                return false;
            }
        }

        return true;
    }
}

public class FilterChatClass
{
    public static void FilterChat(Player player, string message)
    {
        //Global Tag pra aparecer bunitinho
        if (JsonLists.GlobalTags.ContainsKey(player.UserId))
        {
            message += $"\n<size=25>{JsonLists.GlobalTags[player.UserId]}</size>";
        }

        //CustomTag, q da pra mudar pelo config!! pra aparecer bunitinho!!!
        if (AProximityChat.Instance.Config.CustomTag.ContainsKey(player.UserId))
        {
            message += $"\n<size=25>{AProximityChat.Instance.Config.CustomTag[player.UserId]}</size>";
        }

        if (!Round.IsStarted)
        {
            ProximityChatLogic.LobbyChat(player, message);
            return;
        }

        if (player.IsHuman && player.IsAlive)
        {
            ProximityChatLogic.RadioChat(player, message, out Player? radioWorked);
            ProximityChatLogic.ProximityChat(player, message, radioWorked);
            ProximityChatLogic.Scp1576Chat(player, message);
        }

        if (player.IsScp && player.IsAlive)
        {
            ProximityChatLogic.ScpChat(player, message);
            return;
        }

        if (player.IsDead && player.Role.Type == RoleTypeId.Spectator)
        {
            ProximityChatLogic.SpectatorChat(player, message);
            return;
        }
    }
}
