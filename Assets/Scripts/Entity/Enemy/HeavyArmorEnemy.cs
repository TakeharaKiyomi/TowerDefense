using UnityEngine;

public class HeavyArmorEnemy : EnemyEntityBase
{
    void Awake()
    {
        
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

    public static HeavyArmorEnemy CreateEntity()
    {
        return CreateEntity<HeavyArmorEnemy>(GameEntityPrefabType.HEAVYARMOR_ENEMY);
    }
}
