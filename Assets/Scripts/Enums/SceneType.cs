using System;
using UnityEngine;

public enum SceneType
{
    NONE = 0,
    TITLE,
    GAME,
    GAME_CLEAR,
    GAME_OVER,
    MAX_NUM
}

public static class SceneTypeExtensions
{
    public static string GetSceneName(this SceneType type)
    {
        return type switch
        {
            SceneType.TITLE         => "Title",
            SceneType.GAME          => "Game",
            SceneType.GAME_CLEAR    => "GameClear",
            SceneType.GAME_OVER     => "GameOver",
            _                       => throw new ArgumentException("SceneType(" + type + ")は不正な値です。")
        };
    }
}