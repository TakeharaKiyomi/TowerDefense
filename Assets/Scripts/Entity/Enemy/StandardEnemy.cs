using UnityEngine;

public class StandardEnemy : EnemyEntityBase
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

    public static StandardEnemy CreateEntity()
    {
        return CreateEntity<StandardEnemy>(GameEntityPrefabType.STANDARD_ENEMY);
    }
}
