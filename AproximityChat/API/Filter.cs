namespace AproximityChat.API;

using Exiled.API.Features;
using PlayerRoles;

public class FilterChat
{
    static string[] BlackList = new string[]
    {
        "nigger",
        "nigge",
        "black",
        "preto",
        "n.i.g.g.e.r",
        "n.i.g.g.e",
        "p.r.e.t.o",
        "negrão",
        "nazismo",
        "nazista",
        "hitler",
        "h.i.t.l.e.r",
        "mongoloide",
        "mongolóide",
    };

    static public bool FilterChatWords(string message, Player player)
    {
        foreach (string blacklistmsg in BlackList)
        {
            if (message.ToLower().Contains(blacklistmsg))
            {
                if (JsonLists.MutePcDict.ContainsKey(player.UserId))
                {
                    JsonLists.MutePcDict[player.UserId].TimeMute = JsonLists.MutePcDict[player.UserId].TimeMute.AddMinutes(1);
                    JsonLists.SaveData();
                }
                else
                {
                    MuteProxCLogic.Mute(player, 1, "[AutoMute] Palavra feia!");
                    WebHookMsg.ReportMsg(message, blacklistmsg, player);
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
            /* if (player.CurrentItem != null)
            {
                if (player.CurrentItem.Type == ItemType.Radio)
                {
                    RadioChat(player, message);
                    return;
                }
            } */
            ProximityChatLogic.ProximityChat(player, message);
            return;
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