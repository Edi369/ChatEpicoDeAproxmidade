using Exiled.API.Features;
using PlayerRoles;
using UnityEngine;

//Creditos a MMDDKK - https://github.com/MMDDKK6500/BikeUtils
//Creditos a MMDDKK - https://github.com/MMDDKK6500/BikeUtils
//Creditos a MMDDKK - https://github.com/MMDDKK6500/BikeUtils

/// <summary>
/// A class containing utilities for the game's roles and classes.
/// </summary>
public static class RoleClassUtils
{
    /// <summary>
    /// Gets the player's tag color.
    /// </summary>
    /// <param name="player">The <see cref="Player"/> you want to get the color.</param>
    /// <returns>Returns string with the player color in a #000000 HEX format or #FFFFFF if not found.</returns>
    public static string GetPlayerTagColor(Player player)
    {
        return player.ReferenceHub.serverRoles.MyColor == "default" ? "#FFFFFF" : player.ReferenceHub.serverRoles.MyColor;
    }

    /// <summary>
    /// Translates a <see cref="RoleTypeId"/> into the Brazilian Portuguese name of role.
    /// </summary>
    /// <param name="role">The <see cref="RoleTypeId"/> to be translated.</param>
    /// <returns>A string that contains the translated name, or ??? if role was not found.</returns>
    public static string TranslateRole(RoleTypeId role)
    {
        return role switch
        {
            RoleTypeId.Scp939 => "Cachorrão",
            RoleTypeId.Scp049 => "Doctor",
            RoleTypeId.Scp0492 => "Zumbi",
            RoleTypeId.Scp096 => "Chorão",
            RoleTypeId.Scp079 => "Pc",
            RoleTypeId.Scp173 => "Peanut",
            RoleTypeId.Scp3114 => "Esqueleto",
            RoleTypeId.Scp106 => "Negão",
            RoleTypeId.ClassD => "DBoy",
            RoleTypeId.Scientist => "Cientista",
            RoleTypeId.FacilityGuard => "Guardinha",
            RoleTypeId.NtfSpecialist => "NTF Nerd",
            RoleTypeId.NtfPrivate => "NTF Cabo",
            RoleTypeId.NtfSergeant => "NTF Sargento",
            RoleTypeId.NtfCaptain => "NTF Capitão",
            RoleTypeId.ChaosConscript => "Chaos Boy",
            RoleTypeId.ChaosRifleman => "Chaos Fuzileiro",
            RoleTypeId.ChaosMarauder => "Chaos Saqueador",
            RoleTypeId.ChaosRepressor => "Chaos Repressor",
            RoleTypeId.Tutorial => "ADM Abuser",
            _ => "???",
        };
    }

    /// <summary>
    /// Gets a string and checks if it is a valid playable SCP number.
    /// </summary>
    /// <param name="number">The SCP number, in string format.</param>
    /// <returns>Returns the <see cref="RoleTypeId"/> of the SCP, or Spectator if not an SCP.</returns>
    public static RoleTypeId GetSCPRole(string number)
    {
        return number switch
        {
            "939" => RoleTypeId.Scp939,
            "106" => RoleTypeId.Scp106,
            "096" => RoleTypeId.Scp096,
            "173" => RoleTypeId.Scp173,
            "049" => RoleTypeId.Scp049,
            "079" => RoleTypeId.Scp079,
            "3114" => RoleTypeId.Scp3114,
            _ => RoleTypeId.Spectator,
        };
    }

    /// <summary>
    /// Gets an int and checks if it is a valid playable SCP number.
    /// </summary>
    /// <param name="number">The SCP number, in an int format.</param>
    /// <returns>Returns the <see cref="RoleTypeId"/> of the SCP, or Spectator if not an SCP.</returns>
    public static RoleTypeId GetSCPRole(int number)
    {
        return number switch
        {
            939 => RoleTypeId.Scp939,
            106 => RoleTypeId.Scp106,
            096 => RoleTypeId.Scp096,
            173 => RoleTypeId.Scp173,
            049 => RoleTypeId.Scp049,
            079 => RoleTypeId.Scp079,
            3114 => RoleTypeId.Scp3114,
            _ => RoleTypeId.Spectator,
        };
    }

    /// <summary>
    /// Checks if the given <see cref="RoleTypeId"/> is an SCP Role.
    /// </summary>
    /// <param name="role"><see cref="RoleTypeId"/> that you want to see if it is an SCP Role.</param>
    /// <returns>True if <see cref="RoleTypeId"/> is an SCP role or false if <see cref="RoleTypeId"/> is not an SCP role.</returns>
    public static bool ScpRoles(RoleTypeId role)
    {
        return role == RoleTypeId.Scp173 ||
               role == RoleTypeId.Scp106 ||
               role == RoleTypeId.Scp049 ||
               role == RoleTypeId.Scp079 ||
               role == RoleTypeId.Scp096 ||
               role == RoleTypeId.Scp0492 ||
               role == RoleTypeId.Scp939 ||
               role == RoleTypeId.Scp3114;
    }

    /// <summary>
    /// Gets a <see cref="RoleTypeId"/> and returns what color it should be.
    /// </summary>
    /// <param name="roleType">The RoleTypeId you want the color of.</param>
    /// <returns>A string with the HEX color in the #000000 format.</returns>
    public static string GetRoleColor(RoleTypeId roleType)
    {
        return roleType switch
        {
            RoleTypeId.ClassD => "#FF8E00",
            RoleTypeId.Scientist => "#FFFF7C",
            RoleTypeId.FacilityGuard => "#5B6370",
            RoleTypeId.NtfSergeant => "#0096FF",
            RoleTypeId.NtfCaptain => "#003ECA",
            RoleTypeId.NtfPrivate => "#6FC3FF",
            RoleTypeId.NtfSpecialist => "#52B8FF",
            RoleTypeId.ChaosRifleman => "#0A7D34",
            RoleTypeId.ChaosMarauder => "#00782F",
            RoleTypeId.ChaosRepressor => "#006728",
            RoleTypeId.ChaosConscript => "#008F1E",
            RoleTypeId.Tutorial => "#FC00AB",
            _ => "#8A8A8A",
        };
    }

    /// <summary>
    /// Gets a <see cref="Color">Unity Color</see> and transforms it into HEX.
    /// </summary>
    /// <param name="color"><see cref="Color">Unity Color</see> object.</param>
    /// <param name="boostR">Red color boost.</param>
    /// <param name="boostG">Green color boost.</param>
    /// <param name="boostB">Blue color boost.</param>
    /// <returns>A string with the HEX color in the #000000 format.</returns>
    public static string UnityColorToHex(Color color, float boostR, float boostG, float boostB)
    {
        return $"#{Mathf.FloorToInt(Mathf.Clamp01(color.r + boostR) * 255):X2}{Mathf.FloorToInt(Mathf.Clamp01(color.g + boostG) * 255):X2}{Mathf.FloorToInt(Mathf.Clamp01(color.b + boostB) * 255):X2}";
    }

    /// <summary>
    /// Same thing as the other function, but now the boost variable increases all color channels.
    /// </summary>
    /// <param name="color"><see cref="Color">Unity Color</see> object.</param>
    /// <param name="boost">All channels color boost.</param>
    /// <returns>A string with the HEX color in the #000000 format.</returns>
    public static string UnityColorToHex(Color color, float boost = 0f)
    {
        return $"#{Mathf.FloorToInt(Mathf.Clamp01(color.r + boost) * 255):X2}{Mathf.FloorToInt(Mathf.Clamp01(color.g + boost) * 255):X2}{Mathf.FloorToInt(Mathf.Clamp01(color.b + boost) * 255):X2}";
    }
}
