using System;
using UnityEngine;

public enum GameEntityPrefabType
{
    NONE = 0,           // 未設定
    FIELD_GRID_ROAD,    // マップ（道）
    FIELD_GRID_FOREST,  // マップ（森）
    BASE_CORE,          // 拠点
    STANDARD_TOWER,     // 標準タワー
    RAPIDFIRE_TOWER,    // 連射タワー
    STANDARD_BULLET,    // 通常弾
    STANDARD_ENEMY,     // 通常敵
    HEAVYARMOR_ENEMY,   // 重装敵
    PLACE_TOWER_BUTTON, // タワー設置用ボタン
    MAX_NUM
}

public static class GameEntityPrefabTypeExtensions
{
    /// <summary>
    /// プレハブ種別からリソース名を取得する
    /// </summary>
    /// <param name="type">プレハブ種別</param>
    /// <returns>リソース名</returns>
    public static string GetPrefabString(this GameEntityPrefabType type)
    {
        return type switch
        {
            GameEntityPrefabType.FIELD_GRID_ROAD    => "Field/Road",
            GameEntityPrefabType.FIELD_GRID_FOREST  => "Field/Forest",
            GameEntityPrefabType.BASE_CORE          => "BaseCore/BaseCore",
            GameEntityPrefabType.STANDARD_TOWER     => "Tower/StandardTower",
            GameEntityPrefabType.RAPIDFIRE_TOWER    => "Tower/RapidFireTower",
            GameEntityPrefabType.STANDARD_BULLET    => "Bullet/StandardBullet",
            GameEntityPrefabType.STANDARD_ENEMY     => "Enemy/StandardEnemy",
            GameEntityPrefabType.HEAVYARMOR_ENEMY   => "Enemy/HeavyArmorEnemy",
            GameEntityPrefabType.PLACE_TOWER_BUTTON => "UI/Button/PlaceTowerButton",
            _ => throw new ArgumentException("GameEntityPrefabType(" + type + ")は不正な値です。")
        };
    }
}
