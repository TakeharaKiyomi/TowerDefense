using UnityEngine;

public class HeavyArmorEnemy : EnemyEntityBase
{
    void Awake()
    {
        _entityData = new GameEntityBaseData(15, 2, 0, 0, 0);
        _enemyData = new EnemyEntityData(
            EnemyEntityType.STANDARD,
            new ItemEntityData[] { new ItemEntityData(ItemType.GOLD, 20) }, 
            0.75f);
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
