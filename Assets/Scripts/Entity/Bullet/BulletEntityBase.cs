using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class BulletEntityBase : GameEntityBase
{
    // 弾の情報（タワーの情報に影響する）
    protected BulletConfig _bulletConfig;

    // ダメージ
    private LayerMask _damageTagetLayer;

    private Camera mainCamera;

    protected void Awake()
    {
        mainCamera = Camera.main;
    }

    protected void Update()
    {
        // 移動
        transform.position += new Vector3(_bulletConfig.addDistance * _bulletConfig.direction.x, _bulletConfig.addDistance * _bulletConfig.direction.y, 0.0f);

        // 攻撃
        Collider2D collider2D = Physics2D.OverlapPoint(transform.position, _damageTagetLayer);
        if(collider2D != null)
        {
            collider2D.GetComponent<IBulletDamage>().TakeDamage(_bulletConfig.attack);
            Destroy(gameObject);
            return;
        }

        // 弾のワールド座標からカメラ座標に変換し、画面外に行ったら自身を破棄する
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector3 min = mainCamera.WorldToViewportPoint(spriteRenderer.bounds.min);
        Vector3 max = mainCamera.WorldToViewportPoint(spriteRenderer.bounds.max);
        if( min.x > 1.0f ||
            min.y > 1.0f || 
            max.x < 0.0f || 
            max.y < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Initializa(BulletConfig bulletConfig, LayerMask damageTagetLayer)
    {
        _bulletConfig = bulletConfig;
        _damageTagetLayer = damageTagetLayer;
    }
}
