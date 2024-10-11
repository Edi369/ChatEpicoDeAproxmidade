namespace AproximityChat.Commands;

using CommandSystem;
using RemoteAdmin;
using System;
using System.Linq;
using Exiled.API.Features;
using AproximityChat.API;

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

            if (JsonLists.MutePcDict.ContainsKey(player.UserId))
            {
                TimeSpan TempoRestante = JsonLists.MutePcDict[player.UserId].TimeMute.Subtract(DateTime.Now);


                if (DateTime.Compare(JsonLists.MutePcDict[player.UserId].TimeMute, DateTime.Now) < 0)
                {
                    JsonLists.MutePcDict.Remove(player.UserId);
                    JsonLists.SaveData();
                    FilterChatClass.FilterChat(player, message);
                    response = "Seu Mute acabou!!! Sua mensagem epica foi enviada agora!!!!";
                    return true;
                }

                response = $"Você esta mutado do Prox Chat!!!!\nMotivo: {JsonLists.MutePcDict[player.UserId].Reason}\nTempo Restante: {(TempoRestante.Days > 0 ? $"{TempoRestante.Days}d " : "")}{(TempoRestante.Hours > 0 ? $"{TempoRestante.Hours}h " : "")}{TempoRestante.Minutes}m {TempoRestante.Seconds}s";
                return false;
            }

            FilterChatClass.FilterChat(player, message);
            response = "Mensagem epica enviada!!!!";
            return true;
        }

        response = "???!!!!";
        return false;
    }
}