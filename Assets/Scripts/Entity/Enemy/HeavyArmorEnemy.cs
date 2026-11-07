using UnityEngine;

public class HeavyArmorEnemy : EnemyEntityBase
{
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
    }

    /// <summary>
    /// インスタンスを生成する
    /// </summary>
    /// <returns>生成されたインスタンス</returns>
    public static HeavyArmorEnemy CreateEntity()
    {
        return CreateEntity<HeavyArmorEnemy>(GameEntityPrefabType.HEAVYARMOR_ENEMY);
    }
}
