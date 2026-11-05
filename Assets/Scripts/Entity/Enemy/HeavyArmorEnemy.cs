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

    public static HeavyArmorEnemy CreateEntity()
    {
        return CreateEntity<HeavyArmorEnemy>(GameEntityPrefabType.HEAVYARMOR_ENEMY);
    }
}
