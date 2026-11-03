using UnityEngine;

[CreateAssetMenu(fileName = "TowerMaster", menuName = "Scriptable Objects/TowerMaster")]
public class TowerMaster : ScriptableObject
{
    // タワー種別
    public TowerEntityType type;
    // 攻撃力
    public int attack;
    // 攻撃間隔
    public float attackInterval;
    // 射程
    public float range;
    // 弾速
    public float velocity;
    // 配置コスト
    public int moneyCost;
    // タワー名
    public string towerName;
}
