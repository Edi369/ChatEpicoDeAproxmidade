namespace AproximityChat.API;

using BikeUtils;
using Exiled.API.Enums;
using Exiled.API.Features;
using InventorySystem.Items.Usables.Scp1576;
using PlayerRoles;
using System.Linq;
using UnityEngine;

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