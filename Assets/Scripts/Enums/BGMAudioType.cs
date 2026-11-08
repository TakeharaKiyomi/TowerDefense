using System;
using UnityEngine;

public enum BGMAudioType
{
    NONE = 0,
    TITLE,
    GAME,
    GAME_CLEAR,
    GAME_OVER,
    MAX_NUM
}

public static class BGMAudioTypeExtensions
{
    /// <summary>
    /// リソースの文字列を取得する
    /// </summary>
    /// <param name="type">BGMの種別</param>
    /// <returns>リソースの文字列</returns>
    /// <exception cref="ArgumentException">想定外の種別が指定されていた場合</exception>
    public static string GetResourcesString(this BGMAudioType type)
    {
        return "Audio/BGM/" + type switch
        {
            BGMAudioType.TITLE      => "Title",
            BGMAudioType.GAME       => "Game",
            BGMAudioType.GAME_CLEAR => "GameClear",
            BGMAudioType.GAME_OVER  => "GameOver",
            _                       => throw new ArgumentException(nameof(BGMAudioType) + "(" + type + ")")
        };
    }
}
