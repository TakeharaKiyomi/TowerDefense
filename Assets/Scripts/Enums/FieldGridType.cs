using System;
using UnityEngine;

public enum FieldGridType
{
    NONE = 0,   // 未設定
    ROAD,       // 道
    FOREST,     // 森
    MAX_NUM
}

public static class FieldGridTypeExtensions
{
    public static GameEntityPrefabType GetPrefabType(this FieldGridType type)
    {
        return type switch
        {
            FieldGridType.ROAD      => GameEntityPrefabType.FIELD_GRID_ROAD,
            FieldGridType.FOREST    => GameEntityPrefabType.FIELD_GRID_FOREST,
            _ => throw new ArgumentException("FieldGridType(" + type + ")は不正な値です。")
        };
    }
}