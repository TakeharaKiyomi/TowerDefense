using System;
using UnityEngine;

public enum SEAudioType
{
    NONE = 0,
    BUTTON_CLICK,
    ENEMY_DAMAGE,
    BASE_DAMAGE,
    TOWER_INSTALLATION,
    MAX_NUM
}

public static class SEAudioTypeExtensions
{
    public static string GetAudioClipString(this SEAudioType type)
    {
        return type switch
        {
            SEAudioType.BUTTON_CLICK        => "Audio/SE/ButtonClick",
            SEAudioType.ENEMY_DAMAGE        => "Audio/SE/EnemyDamage",
            SEAudioType.BASE_DAMAGE         => "Audio/SE/BaseDamage",
            SEAudioType.TOWER_INSTALLATION  => "Audio/SE/TowerInstallation",
            _                               => throw new ArgumentException("SEAudioType(" + type + ")は不正な値です。"),
        };
    }
}
