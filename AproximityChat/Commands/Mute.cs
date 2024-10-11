namespace AproximityChat.Commands;

using CommandSystem;
using System.Text.RegularExpressions;
using System;
using Exiled.API.Features;
using System.Linq;
using AproximityChat.API;

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

            if (JsonLists.MutePcDict.ContainsKey(player.UserId))
            {
                JsonLists.MutePcDict.Remove(player.UserId);
                JsonLists.SaveData();
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

        if (JsonLists.MutePcDict.ContainsKey(player.UserId))
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