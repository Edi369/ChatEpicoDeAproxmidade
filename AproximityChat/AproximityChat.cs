namespace AproximityChat;

using Exiled.API.Features;
using AproximityChat.API;

public class AProximityChat : Plugin<Config>
{
    public static AProximityChat? Instance { get; private set; }
    public override string Name => "AproximityChat";
    public override string Author => "bicicreta";
    public string Version = "v0.0.1";
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
        JsonLists.LoadData();
        if (Config.GlobalTags) JsonLists.UpdateGlobalTags();
        JsonLists.UpdateBansWords();
        JsonLists.GetCustomBansWords();
        Log.Info($"Versão {Version}");
        if (JsonLists.GetNewVersion() != Version)
        {
            Log.Warn("Sua Versão pode estar desatualizada! Recomendo olhar o GitHub https://github.com/Edi369/ChatEpicoDeAproxmidade/releases");
        }

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
        if (Config.GlobalTags) JsonLists.UpdateGlobalTags();
    }
}