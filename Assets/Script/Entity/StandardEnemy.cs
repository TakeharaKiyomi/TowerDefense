using UnityEngine;

public class StandardEnemy : EnemyEntityBase
{
    void Awake()
    {
        _entityData = new GameEntityBaseData(5, 1, 0, 0, 0);
        _enemyData = new EnemyEntityData(
            EnemyEntityType.STANDARD,
            new ItemEntityData[] { new ItemEntityData(ItemType.GOLD, 10) }, 
            1.5f);

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
    }
}
