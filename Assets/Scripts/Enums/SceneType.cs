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
    /// <summary>
    /// シーン名を取得する
    /// </summary>
    /// <param name="type">シーンの種別</param>
    /// <returns>シーン名</returns>
    /// <exception cref="ArgumentException">想定外の種別が指定されていた場合</exception>
    public static string GetSceneName(this SceneType type)
    {
        return type switch
        {
            SceneType.TITLE         => "Title",
            SceneType.GAME          => "Game",
            SceneType.GAME_CLEAR    => "GameClear",
            SceneType.GAME_OVER     => "GameOver",
            _                       => throw new ArgumentException(nameof(SceneType) + "(" + type + ")")
        };
    }
}