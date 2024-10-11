namespace AproximityChat.API;

using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System;
using Exiled.API.Features;

public class WebHookMsg
{
    public static void ReportMsg(string message, string blackworldmsg, Player player)
    {
        string webhook = AProximityChat.Instance.Config.WebhookDc;
        if (webhook == "")
        {
            Log.Warn("webhook não configurado!!!!!!!!");
            return;
        }

        WebClient client = new WebClient();
        client.Headers.Add("Content-Type", "application/json");
        string steamid = Regex.Replace(player.UserId, "[^0-9.]", "");
        string payload = "{\r\n  \"content\": null,\r\n  \"embeds\": [\r\n    {\r\n      \"title\": \"AVISO de mensagem suspeita detectada!\",\r\n      \"color\": 16711680,\r\n      \"fields\": [\r\n        {\r\n          \"name\": \"Mensagem:\",\r\n          \"value\": \"" + message + "\"\r\n        },\r\n        {\r\n          \"name\": \"BlackList World:\",\r\n          \"value\": \"" + blackworldmsg + "\"\r\n        },\r\n        {\r\n          \"name\": \"Player info:\",\r\n          \"value\": \"" + player.Nickname + " (" + player.UserId + ")\"\r\n        }\r\n      ],\r\n      \"footer\": {\r\n        \"text\": \"Plugin funny de msg\"\r\n      },\r\n      \"image\": {\r\n        \"url\": \"https://www.steamidfinder.com/signature/" + steamid + ".png\"\r\n      }\r\n    }\r\n  ],\r\n  \"attachments\": []\r\n}";
        client.UploadData(webhook, Encoding.UTF8.GetBytes(payload));
    }

    public static void MuteMsg(DateTime initialTime, DateTime endTime, Player player, string reason)
    {
        string webhook = AProximityChat.Instance.Config.WebhookDc;
        if (webhook == "")
        {
            Log.Warn("webhook não configurado!!!!!!!!");
            return;
        }

        WebClient client = new WebClient();
        client.Headers.Add("Content-Type", "application/json");
        string steamid = Regex.Replace(player.UserId, "[^0-9.]", "");
        TimeSpan timeRest = JsonLists.MutePcDict[player.UserId].TimeMute.Subtract(DateTime.Now);
        string payload = "{\r\n  \"content\": null,\r\n  \"embeds\": [\r\n    {\r\n      \"title\": \"Usuário mutado!\",\r\n      \"color\": 16774973,\r\n      \"fields\": [\r\n        {\r\n          \"name\": \"Motivo:\",\r\n          \"value\": \"" + reason + "\"\r\n        },\r\n        {\r\n          \"name\": \"Tempo:\",\r\n          \"value\": \"" + $"**{(timeRest.Days > 0 ? $"{timeRest.Days}d " : "")}{(timeRest.Hours > 0 ? $"{timeRest.Hours}h " : "")}{timeRest.Minutes}m {timeRest.Seconds}s**\\n``" + initialTime + " - " + endTime + "``\"\r\n        },\r\n        {\r\n          \"name\": \"Player info:\",\r\n          \"value\": \"" + player.Nickname + " (" + player.UserId + ")\"\r\n        }\r\n      ],\r\n      \"footer\": {\r\n        \"text\": \"Plugin funny de msg\"\r\n      },\r\n      \"image\": {\r\n        \"url\": \"\"\r\n      }\r\n    }\r\n  ],\r\n  \"attachments\": []\r\n}";
        client.UploadData(webhook, Encoding.UTF8.GetBytes(payload));
    }

    public static void DesMuteMsg(Player player)
    {
        string webhook = AProximityChat.Instance.Config.WebhookDc;
        if (webhook == "")
        {
            Log.Warn("webhook não configurado!!!!!!!!");
            return;
        }

        WebClient client = new WebClient();
        client.Headers.Add("Content-Type", "application/json");
        string steamid = Regex.Replace(player.UserId, "[^0-9.]", "");
        string payload = "{\r\n  \"content\": null,\r\n  \"embeds\": [\r\n    {\r\n     \"title\": \"Usuário **DES**mutado!\",\r\n      \"color\": 5814783,\r\n      \"fields\": [\r\n        {\r\n          \"name\": \"Player:\",\r\n          \"value\": \"" + player.Nickname + " (" + player.UserId + ")\"\r\n        }\r\n      ],\r\n      \"footer\": {\r\n        \"text\": \"Plugin funny de msg\"\r\n      },\r\n      \"image\": {\r\n        \"url\": \"\"\r\n      }\r\n    }\r\n  ],\r\n  \"attachments\": []\r\n}";
        client.UploadData(webhook, Encoding.UTF8.GetBytes(payload));
    }
}