using UnityEngine;

[CreateAssetMenu(fileName = "EnemyMaster", menuName = "Scriptable Objects/EnemyMaster")]
public class EnemyMaster : ScriptableObject
{
    // HP
    public int hitPoint;
    // 攻撃力
    public int attack;
    // 移動速度
    public float speed;
    // ドロップ報酬
    public DropItemBaseMaster[] drops;
    // 進行方向
    public Vector2Int[] directions;
}
