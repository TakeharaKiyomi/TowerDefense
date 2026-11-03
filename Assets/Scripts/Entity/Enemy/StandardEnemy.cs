using UnityEngine;

public class StandardEnemy : EnemyEntityBase
{
    void Awake()
    {
        _enemyData = new EnemyEntityData(
            EnemyEntityType.STANDARD,
            5,
            1, 
            1.5f,
            new ItemEntityData[] { new ItemEntityData(ItemType.GOLD, 10) });

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
