using System;
using System.Linq;
using UnityEngine;

public class StandardTower : TowerEntityBase
{
    void Awake()
    {
        _type = TowerEntityType.STANDARD.ToString();
        _entityData = new GameEntityBaseData(0, 1, 0.8f, 2.5f, 6.0f);
        _towerData = new TowerDisplayData(50, "標準");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
    }

    public static GameObject CreateEntity()
    {
        return CreateEntity(TowerEntityType.STANDARD.ToString(),
         "Tower/StandardTower");
    }
}
