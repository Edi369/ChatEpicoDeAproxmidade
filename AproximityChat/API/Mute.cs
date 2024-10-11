namespace AproximityChat.API;

using Exiled.API.Features;
using System;

public class MuteProxCInfo
{
#pragma warning disable CS8618 // Hor hor hor hor
    public string Reason { get; set; }
    public string AuthId { get; set; }
#pragma warning restore CS8618 // roh roh roh roH
    public DateTime TimeMute { get; set; }
}

public class MuteProxCLogic
{
    public static void Mute(Player player, int timeban, string reason)
    {
        if (JsonLists.MutePcDict.ContainsKey(player.UserId))
        {
            JsonLists.MutePcDict[player.UserId].Reason = reason;
            JsonLists.MutePcDict[player.UserId].TimeMute = DateTime.Now.AddMinutes(timeban);
            WebHookMsg.MuteMsg(DateTime.Now, JsonLists.MutePcDict[player.UserId].TimeMute, player, reason);
        }
        else
        {
            JsonLists.MutePcDict.Add(player.UserId, new MuteProxCInfo
            {
                Reason = reason,
                AuthId = player.UserId,
                TimeMute = DateTime.Now.AddMinutes(timeban),
            });
            WebHookMsg.MuteMsg(DateTime.Now, JsonLists.MutePcDict[player.UserId].TimeMute, player, reason);
        }
        JsonLists.SaveData();
    }
}