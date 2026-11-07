using UnityEngine;

public struct BulletConfig
{
    // 攻撃力
    public int attack;
    // 加算距離
    public float addDistance;
    // 進行方向
    public Vector2 direction;

    public BulletConfig(int attack, float addDistance, Vector2 direction)
    {
        this.attack = attack;
        this.addDistance = addDistance;
        this.direction = direction;
    }
}
