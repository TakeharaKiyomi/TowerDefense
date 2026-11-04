using System;
using UnityEngine;

public enum LayerMaskType
{
    DEFAULT     = 0,
    FIELD_GRID  = 6,    // マップ
    ENEMY,              // 敵
    TOWER,
}

public static class LayerMaskTypeExtensions
{
    public static LayerMask GetLayerMask(this LayerMaskType type)
    {
        string typeString = type switch
        {
            LayerMaskType.FIELD_GRID    => "FieldGrid",
            LayerMaskType.ENEMY         => "Enemy",
            LayerMaskType.TOWER         => "Tower",
            _                           => throw new ArgumentException("LayerMaskType(" + type + ")は不正な値です。"),
        };
        return LayerMask.GetMask(typeString);
    }
}
