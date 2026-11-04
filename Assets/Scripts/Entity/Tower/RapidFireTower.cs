using UnityEngine;

public class RapidFireTower : TowerEntityBase
{
    private new void Update()
    {
        base.Update();
    }

    /// <summary>
    /// インスタンスを生成する
    /// </summary>
    /// <returns></returns>
    public static RapidFireTower CreateEntity()
    {
        return CreateEntity<RapidFireTower>(GameEntityPrefabType.RAPIDFIRE_TOWER);
    }
}
