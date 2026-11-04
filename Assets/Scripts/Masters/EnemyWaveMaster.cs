using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWaveMaster", menuName = "Scriptable Objects/EnemyWaveMaster")]
public class EnemyWaveMaster : ScriptableObject
{
    // エネミー情報
    public EnemyEntityType[] enemies;
    // 出現間隔
    public float interval;
}
