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
    /// <summary>
    /// LayerMaskを取得する
    /// </summary>
    /// <param name="types">LayerMaskの種別の配列</param>
    /// <returns>LayerMask</returns>
    /// <exception cref="ArgumentException">想定外の種別が指定されていた場合</exception>
    public static LayerMask GetLayerMask(this LayerMaskType[] types)
    {
        string[] layerNames = new string[types.Length];
        for(int i = 0; i < types.Length; i++)
        {
            layerNames[i] = types[i] switch
            {
                LayerMaskType.FIELD_GRID    => "FieldGrid",
                LayerMaskType.ENEMY         => "Enemy",
                LayerMaskType.TOWER         => "Tower",
                _                           => throw new ArgumentException(nameof(LayerMaskType) + "(" + types + ")")
            };
        }
        return LayerMask.GetMask(layerNames);
    }
}
