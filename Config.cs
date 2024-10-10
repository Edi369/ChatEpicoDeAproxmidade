namespace AproximityChat;

using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

public class Config : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public bool Debug { get; set; }

    [Description("WebHook de log para staff!!! Aqui vai aparecer info de quem foi punido, aviso de msg suspeita etc")]
    public string WebhookDc { get; set; } = "";
    [Description("A distancia BASE do Chat de aproximidade epic!!!")]
    public int distancia { get; set; } = 7;
    [Description("Aqui você pode colocar sua customtag para aparecer na sua msg!!! funciona praticamente igual vc defenir um cargo no config do scp sl")]
    public Dictionary<string, string> CustomTag { get; set; } = new Dictionary<string, string>
    {
        { "steamid64@here", "[customtaghere]" },
        { "patrickbateman@patrickbateman", "[patrick bateman]" }
    };
}

