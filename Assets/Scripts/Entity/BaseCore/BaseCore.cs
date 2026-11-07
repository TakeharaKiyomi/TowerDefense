using UnityEngine;

public class BaseCore : GameEntityBase
{
    /// <summary>
    /// インスタンスを生成する
    /// </summary>
    /// <returns>生成されたインスタンス</returns>
    public static BaseCore CreateEntity()
    {
        return CreateEntity<BaseCore>(GameEntityPrefabType.BASE_CORE);
    }
}
