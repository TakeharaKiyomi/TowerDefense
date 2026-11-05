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
    public static string GetAudioClipString(this BGMAudioType type)
    {
        return type switch
        {
            BGMAudioType.TITLE      => "Audio/BGM/Title",
            BGMAudioType.GAME       => "Audio/BGM/Game",
            BGMAudioType.GAME_CLEAR => "Audio/BGM/GameClear",
            BGMAudioType.GAME_OVER  => "Audio/BGM/GameOver",
            _                       => throw new ArgumentException("BGMAudioType(" + type + ")は不正な値です。")
        };
    }
}
