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
    /// <summary>
    /// リソースの文字列を取得する
    /// </summary>
    /// <param name="type">SEの種別</param>
    /// <returns>リソースの文字列</returns>
    /// <exception cref="ArgumentException">想定外の種別が指定されていた場合</exception>
    public static string GetResourcesString(this SEAudioType type)
    {
        return "Audio/SE/" + type switch
        {
            SEAudioType.BUTTON_CLICK        => "ButtonClick",
            SEAudioType.ENEMY_DAMAGE        => "EnemyDamage",
            SEAudioType.BASE_DAMAGE         => "BaseDamage",
            SEAudioType.TOWER_INSTALLATION  => "TowerInstallation",
            _                               => throw new ArgumentException(nameof(SEAudioType) + "(" + type + ")")
        };
    }
}
