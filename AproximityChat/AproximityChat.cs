namespace AproximityChat;

using Exiled.API.Features;
using AproximityChat.API;

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
        JsonLists.LoadData();
        if (Config.GlobalTags) JsonLists.UpdateGlobalTags();

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