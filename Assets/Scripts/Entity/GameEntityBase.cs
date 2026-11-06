using UnityEngine;
using System.Collections.Generic;

public abstract class GameEntityBase : MonoBehaviour {

    private readonly static Dictionary<GameEntityPrefabType, GameObject> prefabs = new();

    /// <summary>
    /// インスタンスを生成する
    /// </summary>
    /// <param name="type">プレハブ種別</param>
    /// <returns>生成されたインスタンス</returns>
    protected static T CreateEntity<T>(GameEntityPrefabType type) where T : GameEntityBase
    {
        // プレハブ取得
        GameObject prefab;
        if(prefabs.ContainsKey(type))
        {
            prefab = prefabs[type];
        }
        else
        {
            prefab = (GameObject)Resources.Load(type.GetPrefabString());
            if(prefab == null)
            {
                Debug.Log("指定のプレハブ(" + type.GetPrefabString() + ")が存在しない。");
                return null;
            }
            // プレハブ保存
            prefabs.Add(type, prefab);
        }
        // インスタンス生成
        GameObject entity = Instantiate(prefab);
        entity.name = prefab.name;
        return entity.GetComponent<T>();
    }
}
