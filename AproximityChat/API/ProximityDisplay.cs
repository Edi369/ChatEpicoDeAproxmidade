namespace AproximityChat.API;

using Exiled.API.Enums;
using Exiled.API.Features;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Radio;
using InventorySystem.Items.Usables.Scp1576;
using PlayerRoles;
using System.Linq;
using UnityEngine;

public class ProximityChatLogic
{
    public static Player? RRreceiver;
    public static string[] BaseMsgBrod = new string[]
    {
        "<size=30><color=#999999>[Prox.C]</color></size>",
        "<size=30><color=#940000>[Scp.C]</color></size>",
        "<size=30><color=#2a9400>[Radio.C]</color></size>",
        "<size=30><color=#999999>[Spec.C]</color></size>",
        "<size=30><color=#ffe62b>[Looby.C]</color></size>",
        "<size=30><color=#940000>[Scp1576.C]</color></size>",
    };

    public static void ProximityChat(Player Sender, string message, Player? _sender = null)
    {
        Log.Info(RRreceiver);
        //normal
        foreach (Player Receiver in Player.List.Where(p => p.Role.Type != RoleTypeId.Scp939 && Vector3.Distance(p.Position, Sender.Position) <= AProximityChat.Instance.Config.distancia && p != RRreceiver && p !=_sender))
        {
            Receiver.Broadcast(3, $"{BaseMsgBrod[0]} <color={RoleClassUtils.GetRoleColor(Sender.Role.Type)}>({Sender.Nickname})</color>\n{message}");
            SpectatorSeeProximityChat(Receiver, message);
        }
        RRreceiver = null;

        //939 ouve a maior distancia lmao
        foreach (Player Receiver in Player.List.Where(p => p.Role.Type == RoleTypeId.Scp939 && Vector3.Distance(p.Position, Sender.Position) <= AProximityChat.Instance.Config.distancia + 2))
        {
            if (Vector3.Distance(Receiver.Position, Sender.Position) < 7)
            {
                Receiver.Broadcast(3, $"<size=25><color=#999999>[perto]</color></size>{BaseMsgBrod[0]} <color={RoleClassUtils.GetRoleColor(Sender.Role.Type)}>({Sender.Nickname})</color>\n{message}");
            }
            else
            {
                Receiver.Broadcast(3, $"<size=25><color=#999999>[longe]</color></size>{BaseMsgBrod[0]} <color={RoleClassUtils.GetRoleColor(Sender.Role.Type)}>({Sender.Nickname})</color>\n{message}");
            }
        }
    }

    public static void ScpChat(Player player, string message)
    {
        foreach (Player Receiver in Player.List.Where(p => p.Role.Side == Side.Scp))
        {
            Receiver.Broadcast(3, $"{BaseMsgBrod[1]} <color=red>({RoleClassUtils.TranslateRole(player.Role.Type)})</color><color=#940000><size=25>({player.Nickname})</size></color>\n<color=#ffb5b5>{message}</color>");
        }
    }

    public static bool RadioChat(Player Sender, string message, out Player? radioWorked)
    {
        RadioItem _radioS;
        RadioItem _radioR;
        if (Sender.Inventory.TryGetInventoryItem(ItemType.Radio, out ItemBase radioS))
        {
            _radioS = (RadioItem)radioS;
            if (!_radioS.IsUsable)
            {
                radioWorked = null;
                return false;

            }
        }
        else
        {
           radioWorked = null;
           return false;
        }
        foreach (Player Receiver in Player.List.Where(p => p != Sender && p.HasItem(ItemType.Radio)))
        {
            Receiver.Inventory.TryGetInventoryItem(ItemType.Radio, out ItemBase radioR);
            _radioR = (RadioItem)radioR;
            if (_radioR.IsUsable && _radioS.RangeLevel >= _radioR.RangeLevel)
            {
                Receiver.Broadcast(3, $"{BaseMsgBrod[2]}-{_radioS.RangeLevel} ({Sender.Nickname})\n{message}");
                RRreceiver = Receiver;
            }
        }
        Sender.Broadcast(3, $"{BaseMsgBrod[2]}-{_radioS.RangeLevel} ({Sender.Nickname})\n{message}");
        radioWorked = Sender;
        return true;
    }

    public static void SpectatorChat(Player player, string message)
    {
        foreach (Player Receiver in Player.List.Where(p => p.Role.Type == RoleTypeId.Spectator))
        {
             Receiver.Broadcast(3, $"{BaseMsgBrod[3]} ({player.Nickname})\n{message}");
        }
    }
    public static bool Scp1576Chat(Player player, string message)
    {
        foreach (Player Sender in Player.List.Where(p => p.CurrentItem != null && p.CurrentItem.Type == ItemType.SCP1576))
        {
            if (Sender.CurrentItem.Base is Scp1576Item scp1576)
            {
                if (scp1576.IsUsing)
                {
                    Sender.Broadcast(3, $"{BaseMsgBrod[5]} ({player.Nickname})\n{message}");
                    SpectatorChat(player, message);
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public static void SpectatorSeeProximityChat(Player player, string message)
    {
        foreach (Player Receiver in Player.List.Where(p => p.IsDead))
        {
            if (Receiver.Role is Exiled.API.Features.Roles.SpectatorRole spect)
            {
                if (spect.SpectatedPlayer != null && spect.SpectatedPlayer == player)
                {
                    Receiver.Broadcast(3, $"{BaseMsgBrod[0]} <color={RoleClassUtils.GetRoleColor(player.Role.Type)}>({player.Nickname})</color>\n{message}");
                }
            }
        }
    }

    public static void LobbyChat(Player player, string message)
    {
        foreach (Player Receiver in Player.List)
        {
            Receiver.Broadcast(3, $"{BaseMsgBrod[4]} ({player.Nickname})\n{message}");
        }
    }
}
