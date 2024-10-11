namespace AproximityChat;

using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using AproximityChat.API;

public class Config : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public bool Debug { get; set; }

    [Description("Ativa ou Desativa aquela mensagem bizarra quando você inicia o servidor, tipo do Exiled")]
    public bool MensagemSinistra { get; set; } = true;

    [Description("A distancia BASE do Chat de proximidade epic!!!")]
    public int distancia { get; set; } = 7;

    [Description("WebHook de log para staff!!! Aqui vai aparecer info de quem foi punido, aviso de msg suspeita etc")]
    public string WebhookDc { get; set; } = "";

    [Description("Ativa ou Desativa o update de GlobalTags, desativar isso aqui seria bem paia ó")]
    public bool GlobalTags { get; set; } = true;

    [Description("Ativa ou Desativa o update de BansPerms, recomendado deixar ativado")]
    public bool GlobalBans { get; set; } = true;

    [Description("Aqui você pode colocar suas CustomTag para aparecer na sua msg!!! funciona praticamente igual vc definir um cargo no config do scp sl")]
    public Dictionary<string, string> CustomTag { get; set; } = new Dictionary<string, string>
    {
        { "steamid64@aqui", "[suacustomtagfodonaaquitbm]" },
        { "patrickbateman@patrickbateman", "[patrick bateman]" },
        { "testefunny@test", "[funny]" }
    };
}