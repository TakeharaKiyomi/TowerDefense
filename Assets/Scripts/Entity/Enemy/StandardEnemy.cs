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

    /// <summary>
    /// インスタンスを生成する
    /// </summary>
    /// <returns>生成されたインスタンス</returns>
    public static StandardEnemy CreateEntity()
    {
        return CreateEntity<StandardEnemy>(GameEntityPrefabType.STANDARD_ENEMY);
    }
}
