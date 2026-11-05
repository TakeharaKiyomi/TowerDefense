using UnityEngine;

public class StandardEnemy : EnemyEntityBase
{
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
    }

    public static StandardEnemy CreateEntity()
    {
        return CreateEntity<StandardEnemy>(GameEntityPrefabType.STANDARD_ENEMY);
    }
}
