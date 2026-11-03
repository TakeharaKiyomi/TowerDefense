using UnityEngine;

public class RapidFireTower : TowerEntityBase
{
    void Awake()
    {
        _type = TowerEntityType.RAPIDFIRE.ToString();
        _entityData = new GameEntityBaseData(0, 1, 0.3f, 2.0f, 8.0f);
        _towerData = new TowerDisplayData(70, "連射");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {
        base.Update();
    }

    public static GameObject CreateEntity()
    {
        return CreateEntity(TowerEntityType.RAPIDFIRE.ToString(),
         "Tower/RapidFireTower");
    }
}
