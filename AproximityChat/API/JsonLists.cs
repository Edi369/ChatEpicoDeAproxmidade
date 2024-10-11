namespace AproximityChat.API;

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System;
using Exiled.API.Features;
using System.Linq;

public class BannedWord
{
    public string Word;
    public int Weight;

    public BannedWord(string word, int weight)
    {
        Word = word;
        Weight = weight;
    }
}

public class JsonLists
{
    public static Dictionary<string, MuteProxCInfo> MutePcDict = new Dictionary<string, MuteProxCInfo>();
    public static Dictionary<string, string> GlobalTags = new Dictionary<string, string>();
    public static BannedWord[] CustomBlackList = new BannedWord[]{};

    public static BannedWord[] BlackList = new BannedWord[]
    {
        new("nigger", 2),
        new("nigge", 2),
        new("black", 2),
        new("preto", 2),
        new("n.i.g.g.e.r", 2),
        new("n.i.g.g.e", 2),
        new("p.r.e.t.o", 2),
        new("negro", 2),
        new("nazismo", 3),
        new("nazista", 3),
        new("hitler", 3),
        new("h.i.t.l.e.r", 3),
        new("mongoloide", 1),
        new("mongolóide", 1),
    };

    public static bool LoadData()
    {
        if (File.Exists(Path.Combine(Paths.Configs, "PcMute.json")))
        {
            try
            {
                MutePcDict = JsonConvert.DeserializeObject<Dictionary<string, MuteProxCInfo>>(File.ReadAllText(Path.Combine(Paths.Configs, "PcMute.json")));
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao ler a liste de Mute! Certas informações podem estar invalidas ou o arquivo pode estar corrompido. Caminho: {Path.Combine(Paths.Configs, "PcMute.json")}. Erro: {ex}");
                return false;
            }
        }
        else
        {
            Log.Warn("Gerando arquivo json de Mute");
            try
            {
                File.WriteAllText(Path.Combine(Paths.Configs, "PcMute.json"), JsonConvert.SerializeObject(MutePcDict, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao gerar o arquivo json no diretório {Path.Combine(Paths.Configs, "PcMute.json")}.. {ex}");
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

    public static bool UpdateGlobalTags()
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
            return false;
        }
        return true;
    }

    public static bool UpdateBansWords()
    {
        try
        {
            WebClient stream = new WebClient();
            Stream data = stream.OpenRead("https://raw.githubusercontent.com/Edi369/ChatEpicoDeAproxmidade/refs/heads/main/GlobalThings/BannedWords.json");
            string content = new StreamReader(data).ReadToEnd();
            BlackList = JsonConvert.DeserializeObject<BannedWord[]>(content);
        }
        catch (Exception ex)
        {
            Log.Error($"Não foi possivel atualizar as listas de palavras banidas, utilizando uma padrão por segurança, erro: {ex}");
            return false;
        }
        return true;
    }

    public static bool GetCustomBansWords()
    {
        if (File.Exists(Path.Combine(Paths.Configs, "CustomBansWords.json")))
        {
            try
            {
                CustomBlackList = JsonConvert.DeserializeObject<BannedWord[]>(File.ReadAllText(Path.Combine(Paths.Configs, "CustomBansWords.json")));
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao ler a Custom Banned Words! Certas informações podem estar invalidas ou o arquivo pode estar corrompido. Caminho: {Path.Combine(Paths.Configs, "CustomBansWords.json")}. Erro: {ex}");
                return false;
            }
        }
        else
        {
            Log.Warn("Gerando arquivo json de CustomBansWords");
            try
            {
                CustomBlackList = new BannedWord[]
                {
                    new("palavraproibridaaqui:scream:", 0),
                };

                File.WriteAllText(Path.Combine(Paths.Configs, "CustomBansWords.json"), JsonConvert.SerializeObject(CustomBlackList, Formatting.Indented));
                Log.Info("Documentação sobre como arrumar definir palavras custom de ban pode ser encontrado no GitHub https://github.com/Edi369/ChatEpicoDeAproxmidade/blob/main/Documentations/ProxChatMute.md");
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao gerar o arquivo json no diretório {Path.Combine(Paths.Configs, "CustomBansWords.json")}. {ex}");
                return false;
            }
        }
        return true;
    }

    public static string GetNewVersion()
    {
        try
        {
            WebClient stream = new WebClient();
            Stream data = stream.OpenRead("https://raw.githubusercontent.com/Edi369/ChatEpicoDeAproxmidade/refs/heads/main/GlobalThings/NewVersion.txt");
            return new StreamReader(data).ReadToEnd();

        }
        catch (Exception ex)
        {
            Log.Error($"Não foi possivel atualizar as tags Global, erro: {ex}");
            return "error";
        }
    }
}