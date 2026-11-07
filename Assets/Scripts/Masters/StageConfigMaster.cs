using UnityEngine;

[CreateAssetMenu(fileName = "StageConfigMaster", menuName = "Scriptable Objects/StageConfigMaster")]
public class StageConfigMaster : ScriptableObject
{
    // 拠点のフィールドの位置
    public Vector2Int baseFieldPos;

    // 敵の初期フィールドの位置
    public Vector2Int enemyFieldInitPos;

    // 初期HP
    public int initHitPoint;

    // 初期金額
    public int initMoney;
}
