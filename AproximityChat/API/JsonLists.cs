namespace AproximityChat.API;

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System;
using Exiled.API.Features;

public class JsonLists
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