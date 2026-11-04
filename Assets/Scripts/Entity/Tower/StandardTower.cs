using UnityEngine;

public class StandardTower : TowerEntityBase
{
    private new void Update()
    {
        base.Update();
    }

    /// <summary>
    /// インスタンスを生成する
    /// </summary>
    /// <returns></returns>
    public static StandardTower CreateEntity()
    {
        return CreateEntity<StandardTower>(GameEntityPrefabType.STANDARD_TOWER);
    }
}
