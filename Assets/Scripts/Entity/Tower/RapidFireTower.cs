using UnityEngine;

public class RapidFireTower : TowerEntityBase
{
    void Awake()
    {
        // _towerEntityData = new TowerEntityData(TowerEntityType.RAPIDFIRE, 1, 0.3f, 2.0f, 8.0f);
        // _towerData = new TowerDisplayData(70, "連射");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
    }
}
