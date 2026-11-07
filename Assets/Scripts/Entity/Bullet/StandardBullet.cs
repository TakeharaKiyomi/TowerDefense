using UnityEngine;

public class StandardBullet : BulletEntityBase
{

    new void Awake()
    {
        base.Awake();
    }

    new void Update()
    {
        base.Update();
    }

    /// <summary>
    /// インスタンスを生成する
    /// </summary>
    /// <returns>生成されたインスタンス</returns>
    public static StandardBullet CreateEntity()
    {
        return CreateEntity<StandardBullet>(GameEntityPrefabType.STANDARD_BULLET);
    }
}
