namespace AproximityChat;

using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using System.Linq;
using PlayerRoles;
using BikeUtils;
using Exiled.API.Enums;
using UnityEngine;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using InventorySystem.Items.Usables.Scp1576;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

//era pra eu ter feito isso aqui tudo bunitinho separado
//acabou q eu não fiz ate agora
//é por meme eu não vou fazer isso
//vai ficar tudo aqui!! dessa forma!! eu não ligo para minha sanidade :)

public class AProximityChat : Plugin<Config>
{
    public static AProximityChat? Instance { get; private set; }
    public override string Name => "AproximityChat";
    public override string Author => "bicicreta";
    //public override PluginPriority Priority => PluginPriority.Last;

    public override void OnEnabled()
    {
        Exiled.Events.Handlers.Server.RoundStarted += StartedRound;
        Instance = this;

        //msg sinistra :skull:
        if (Config.MensagemSinistra)
        {
            Log.Info($@"
 ▄▄▄       ██▓███   ██▀███   ▒█████  ▒██   ██▒ ▄████▄   ██░ ██  ▄▄▄     ▄▄▄█████▓ ▐██▌ 
▒████▄    ▓██░  ██▒▓██ ▒ ██▒▒██▒  ██▒▒▒ █ █ ▒░▒██▀ ▀█  ▓██░ ██▒▒████▄   ▓  ██▒ ▓▒ ▐██▌ 
▒██  ▀█▄  ▓██░ ██▓▒▓██ ░▄█ ▒▒██░  ██▒░░  █   ░▒▓█    ▄ ▒██▀▀██░▒██  ▀█▄ ▒ ▓██░ ▒░ ▐██▌ 
░██▄▄▄▄██ ▒██▄█▓▒ ▒▒██▀▀█▄  ▒██   ██░ ░ █ █ ▒ ▒▓▓▄ ▄██▒░▓█ ░██ ░██▄▄▄▄██░ ▓██▓ ░  ▓██▒ 
 ▓█   ▓██▒▒██▒ ░  ░░██▓ ▒██▒░ ████▓▒░▒██▒ ▒██▒▒ ▓███▀ ░░▓█▒░██▓ ▓█   ▓██▒ ▒██▒ ░  ▒▄▄  
 ▒▒   ▓▒█░▒▓▒░ ░  ░░ ▒▓ ░▒▓░░ ▒░▒░▒░ ▒▒ ░ ░▓ ░░ ░▒ ▒  ░ ▒ ░░▒░▒ ▒▒   ▓▒█░ ▒ ░░    ░▀▀▒ 
  ▒   ▒▒ ░░▒ ░       ░▒ ░ ▒░  ░ ▒ ▒░ ░░   ░▒ ░  ░  ▒    ▒ ░▒░ ░  ▒   ▒▒ ░   ░     ░  ░ 
  ░   ▒   ░░         ░░   ░ ░ ░ ░ ▒   ░    ░  ░         ░  ░░ ░  ░   ▒    ░          ░ 
      ░  ░            ░         ░ ░   ░    ░  ░ ░       ░  ░  ░      ░  ░         ░    
");
        }
        JsonListMute.LoadData();
        if (Config.GlobalTags) JsonListMute.UpdateGlobalTags();

        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= StartedRound;
        Instance = null;

        base.OnDisabled();
    }

    public void StartedRound()
    {
        if (Config.GlobalTags) JsonListMute.UpdateGlobalTags();
    }
}

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
        TimeSpan timeRest = JsonListMute.MutePcDict[player.UserId].TimeMute.Subtract(DateTime.Now);
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

public class JsonListMute
{
    public static Dictionary<string, MuteProxCInfo> MutePcDict = new Dictionary<string, MuteProxCInfo>();
    public static Dictionary<string, string> GlobalTags = new Dictionary<string, string>();

    public static bool LoadData()
    {
        if (File.Exists(Path.Combine(Paths.Configs, "PcMute.json")))
        {
            MutePcDict = JsonConvert.DeserializeObject<Dictionary<string, MuteProxCInfo>>(File.ReadAllText(Path.Combine(Paths.Configs, "PcMute.json")));
        }
        else
        {
            Log.Warn("Gerando arquivo json de Mute");
            try
            {
                File.Create(Path.Combine(Paths.Configs, "PcMute.json"));
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao gerar o arquivo json no diretório {Path.Combine(Paths.Configs, "PcMute.json")}!! {ex}");
                return false;
            }
        }
        return true;
    }

    public static bool SaveData()
    {
        try
        {
            File.WriteAllText(Path.Combine(Paths.Configs, "PcMute.json"), JsonConvert.SerializeObject(MutePcDict, Formatting.Indented));
        }
        catch (Exception ex)
        {
            Log.Error($"Erro ao tentar acessar o json {Path.Combine(Paths.Configs, "PcMute.json")}!! {ex}");
            return false;
        }
        return true;
    }

    public static void UpdateGlobalTags()
    {
        try
        {
            WebClient stream = new WebClient();
            Stream data = stream.OpenRead("https://raw.githubusercontent.com/Edi369/ChatEpicoDeAproxmidade/refs/heads/main/GlobalThings/GlobalTags.json");
            string content = new StreamReader(data).ReadToEnd();
            GlobalTags = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
        }
        catch (Exception ex)
        {
            Log.Error($"Não foi possivel atualizar as tags Global, erro: {ex}");
        }
    }
}

public class MuteProxCInfo
{
#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.
    public string Reason { get; set; }
    public string AuthId { get; set; }
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.
    public DateTime TimeMute { get; set; }
}

public class MuteProxCLogic
{
    public static void Mute(Player player, int timeban, string reason)
    {
        if (JsonListMute.MutePcDict.ContainsKey(player.UserId))
        {
            JsonListMute.MutePcDict[player.UserId].Reason = reason;
            JsonListMute.MutePcDict[player.UserId].TimeMute = DateTime.Now.AddMinutes(timeban);
            WebHookMsg.MuteMsg(DateTime.Now, JsonListMute.MutePcDict[player.UserId].TimeMute, player, reason);
        }
        else
        {
            JsonListMute.MutePcDict.Add(player.UserId, new MuteProxCInfo
            {
                Reason = reason,
                AuthId = player.UserId,
                TimeMute = DateTime.Now.AddMinutes(timeban),
            });
            WebHookMsg.MuteMsg(DateTime.Now, JsonListMute.MutePcDict[player.UserId].TimeMute, player, reason);
        }
        JsonListMute.SaveData();
    }
}

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
                if (JsonListMute.MutePcDict.ContainsKey(player.UserId))
                {
                    JsonListMute.MutePcDict[player.UserId].TimeMute = JsonListMute.MutePcDict[player.UserId].TimeMute.AddMinutes(1);
                    JsonListMute.SaveData();
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

public class ProximityChatLogic
{
    public static string[] BaseMsgBrod = new string[]
    {
        "<size=30><color=#999999>[Prox.C]</color></size>",
        "<size=30><color=#940000>[Scp.C]</color></size>",
        "<size=30><color=#2a9400>[Radio.C]</color></size>",
        "<size=30><color=#999999>[Spec.C]</color></size>",
        "<size=30><color=#ffe62b>[Looby.C]</color></size>",
        "<size=30><color=#940000>[Scp1576.C]</color></size>",
    };

    public static void FilterChat(Player player, string message)
    {
        //Global Tag pra aparecer bunitinho
        if (JsonListMute.GlobalTags.ContainsKey(player.UserId))
        {
            message += $"\n<size=25>{JsonListMute.GlobalTags[player.UserId]}</size>";
        }

        //CustomTag, q da pra mudar pelo config!! pra aparecer bunitinho!!!
        if (AProximityChat.Instance.Config.CustomTag.ContainsKey(player.UserId))
        {
            message += $"\n<size=25>{AProximityChat.Instance.Config.CustomTag[player.UserId]}</size>";
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
            ProximityChat(player, message);
            return;
        }

        if (player.IsScp && player.IsAlive)
        {
            ScpChat(player, message);
            return;
        }

        if (player.IsDead && player.Role.Type == RoleTypeId.Spectator)
        {
            SpectatorChat(player, message);
            return;
        }

        if (!Round.IsStarted)
        {
            LobbyChat(player, message);
        }
    }

    public static void ProximityChat(Player player, string message)
    {
        //scp1576
        if (player.CurrentItem != null && player.CurrentItem.Type == ItemType.SCP1576)
        {
            if (player.CurrentItem.Base is Scp1576Item scp1576)
            {
                if (scp1576.IsUsing)
                {
                    SpectatorChat(player, message);
                }
            }
        }

        //normal
        foreach (Player ply in Player.List.Where(p => p.Role.Type != RoleTypeId.Scp939 && Vector3.Distance(p.Position, player.Position) <= AProximityChat.Instance.Config.distancia))
        {
            ply.Broadcast(3, $"{BaseMsgBrod[0]} <color={RoleClassUtils.GetRoleColor(player.Role.Type)}>({player.Nickname})</color>\n{message}");
            SpecChatProx(ply, message);
        }

        //939 ouvidão
        foreach (Player ply in Player.List.Where(p => p.Role.Type == RoleTypeId.Scp939 && Vector3.Distance(p.Position, player.Position) <= AProximityChat.Instance.Config.distancia + 2))
        {
            if (Vector3.Distance(ply.Position, player.Position) < 7)
            {
                ply.Broadcast(3, $"<size=25><color=#999999>[perto]</color></size>{BaseMsgBrod[0]} <color={RoleClassUtils.GetRoleColor(player.Role.Type)}>({player.Nickname})</color>\n{message}");
            }
            else
            {
                ply.Broadcast(3, $"<size=25><color=#999999>[longe]</color></size>{BaseMsgBrod[0]} <color={RoleClassUtils.GetRoleColor(player.Role.Type)}>({player.Nickname})</color>\n{message}");
            }
        }
    }

    public static void ScpChat(Player player, string message)
    {
        foreach (Player ply in Player.List.Where(p => p.Role.Side == Side.Scp))
        {
            ply.Broadcast(3, $"{BaseMsgBrod[1]} <color=red>({RoleClassUtils.TranslateRole(player.Role.Type)})</color><color=#940000><size=25>({player.Nickname})</size></color>\n<color=#ffb5b5>{message}</color>");
        }
    }

    public static void RadioChat(Player player, string message)
    {
        Log.Info($"Radio Chat {message}");
    }

    public static void SpectatorChat(Player player, string message)
    {
        //spectador
        foreach (Player ply in Player.List.Where(p => p.Role.Type == RoleTypeId.Spectator))
        {
            if (player.Role.Type != RoleTypeId.Spectator)
            {
                ply.Broadcast(3, $"{BaseMsgBrod[5]} ({player.Nickname})\n{message}");
            }
            else
            {
                ply.Broadcast(3, $"{BaseMsgBrod[3]} ({player.Nickname})\n{message}");
            }
        }

        //pra quem esta usando o item scp1576
        foreach (Player ply in Player.List.Where(p => p.CurrentItem != null && p.CurrentItem.Type == ItemType.SCP1576 && p != player))
        {
            if (ply.CurrentItem.Base is Scp1576Item scp1576)
            {
                if (scp1576.IsUsing)
                {
                    ply.Broadcast(3, $"{BaseMsgBrod[5]} ({player.Nickname})\n{message}");
                }
            }
        }
    }

    public static void SpecChatProx(Player player, string message)
    {
        if (player.CurrentItem != null)
        {
            if (player.CurrentItem.Base is Scp1576Item scp1576)
            {
                if (scp1576.IsUsing)
                {
                    return;
                }
            }
        }

        foreach (Player ply in Player.List.Where(p => p.IsDead))
        {
            if (ply.Role is Exiled.API.Features.Roles.SpectatorRole spect)
            {
                if (spect.SpectatedPlayer != null && spect.SpectatedPlayer == player)
                {
                    ply.Broadcast(3, $"{BaseMsgBrod[0]} <color={RoleClassUtils.GetRoleColor(player.Role.Type)}>({player.Nickname})</color>\n{message}");
                }
            }
        }
    }

    public static void LobbyChat(Player player, string message)
    {
        foreach (Player ply in Player.List)
        {
            ply.Broadcast(3, $"{BaseMsgBrod[4]} ({player.Nickname})\n{message}");
        }
    }
}

[CommandHandler(typeof(ClientCommandHandler))]
public class ProximityCommand : ICommand
{
    public string Command => "ProximityChat";

    public string[] Aliases => new string[] { "pc", "sc", "rc", "lc" };

    public string Description => "Chat de Proximidade Para MUDOS!!!";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (sender is PlayerCommandSender playerSender)
        {
            if (arguments.Count() < 1)
            {
                response = "Você tem que colocar uma mensagem depois do comando '-'";
                return false;
            }

            Player player = Player.Get(playerSender.ReferenceHub);
            string message = "";

            foreach (string msg in arguments)
            {
                message += msg + " ";
            }

            //impede q palavras feia passa
            if (!FilterChat.FilterChatWords(message, player))
            {
                response = "Essa mensagem não pode ser enviada, pois contem uma palavra que esta na blacklist. Essa mensagem foi enviada para equipe de staff! <size=16>Cuidado...</size>";
                return false;
            }

            if (JsonListMute.MutePcDict.ContainsKey(player.UserId))
            {
                TimeSpan TempoRestante = JsonListMute.MutePcDict[player.UserId].TimeMute.Subtract(DateTime.Now);


                if (DateTime.Compare(JsonListMute.MutePcDict[player.UserId].TimeMute, DateTime.Now) < 0)
                {
                    JsonListMute.MutePcDict.Remove(player.UserId);
                    JsonListMute.SaveData();
                    ProximityChatLogic.FilterChat(player, message);
                    response = "Seu Mute acabou!!! Sua mensagem epica foi enviada agora!!!!";
                    return true;
                }

                response = $"Você esta mutado do Prox Chat!!!!\nMotivo: {JsonListMute.MutePcDict[player.UserId].Reason}\nTempo Restante: {(TempoRestante.Days > 0 ? $"{TempoRestante.Days}d " : "")}{(TempoRestante.Hours > 0 ? $"{TempoRestante.Hours}h " : "")}{TempoRestante.Minutes}m {TempoRestante.Seconds}s";
                return false;
            }

            ProximityChatLogic.FilterChat(player, message);
            response = "Mensagem epica enviada!!!!";
            return true;
        }

        response = "???!!!!";
        return false;
    }
}

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class MuteProximityChat : ICommand, IHelpProvider
{
    public string Command => "pcmute";

    public string[] Aliases => new string[] { };

    public string Description => "Muta um mudo do chat de aproximidade!!!";

    public string GetHelp(ArraySegment<string> arguments)
    {
        return "coco";
    }

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission(new PlayerPermissions[] { PlayerPermissions.KickingAndShortTermBanning }))
        {
            response = "Você não tem perm para executar esse comando...";
            return false;
        }

        if (arguments.Count() < 2)
        {
            response = "seu bobo não é assim que funciona >:(\npcmute [Id/Nome] [Tempo(é por minutos ta!!!)] [Motivo]\nou\npcmute [Id/Nome] remove";
            return false;
        }

        Player player = Player.Get(arguments.At(0));
        if (player == null)
        {
            response = "Player não existe ou não foi encontrado!!!!";
            return false;
        }

        if (arguments.At(1) == "remove")
        {
            if (arguments.Count() < 2)
            {
                response = "seu bobo não é assim que funciona >:(\npcmute [Id/Nome] remove";
                return false;
            }

            if (JsonListMute.MutePcDict.ContainsKey(player.UserId))
            {
                JsonListMute.MutePcDict.Remove(player.UserId);
                JsonListMute.SaveData();
                response = $"{player.Nickname} foi desmutado do prox chat!!";
                WebHookMsg.DesMuteMsg(player);
                return true;
            }

            response = $"{player.Nickname} não esta mutado!!";
            return false;
        }

        string reason = "[REDIGIDO]";
        Int32.TryParse(Regex.Replace(arguments.At(1), @"[^\d]", ""), out int timeBan);

        if (timeBan < 1)
        {
            response = "Insira um tempo valido!!!";
            return false;
        }

        if (arguments.Count() > 2)
        {
            reason = "";
            for (int i = 3; i <= arguments.Count(); i++)
            {
                reason += arguments.At(i - 1) + " ";
            }
            //reason = arguments.At(2);
        }

        if (JsonListMute.MutePcDict.ContainsKey(player.UserId))
        {
            MuteProxCLogic.Mute(player, timeBan, reason);
            response = $"O mute de {player.Nickname} foi atualizado com sucesso! {timeBan}m";
            return true;
        }

        MuteProxCLogic.Mute(player, timeBan, reason);
        response = $"{player.Nickname} foi mutado com sucesso por {timeBan}m";
        return true;
    }
}