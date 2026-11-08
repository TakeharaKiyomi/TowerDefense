using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class BulletEntityBase : GameEntityBase
{
    // 弾の情報（タワーの情報に影響する）
    protected BulletConfig _bulletConfig;

    // ダメージ対象
    private LayerMask _damageTagetLayer;

    // メインカメラ
    private Camera _mainCamera;

    private SpriteRenderer _spriteRenderer;

    protected void Awake()
    {
        _mainCamera = Camera.main;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Update()
    {
        // 移動
        transform.position += new Vector3(_bulletConfig.addDistance * _bulletConfig.direction.x, _bulletConfig.addDistance * _bulletConfig.direction.y, 0.0f);

        // 攻撃
        Collider2D collider2D = Physics2D.OverlapPoint(transform.position, _damageTagetLayer);
        if(collider2D != null)
        {
            // 衝突後は自身を破棄する
            collider2D.GetComponent<IBulletDamage>().TakeDamage(_bulletConfig.attack);
            Destroy(gameObject);
            return;
        }

        // 弾のワールド座標からカメラ座標に変換し、画面外に行ったら自身を破棄する
        Vector3 min = _mainCamera.WorldToViewportPoint(_spriteRenderer.bounds.min);
        Vector3 max = _mainCamera.WorldToViewportPoint(_spriteRenderer.bounds.max);
        if( min.x > 1.0f ||
            min.y > 1.0f || 
            max.x < 0.0f || 
            max.y < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="bulletConfig">弾の情報</param>
    /// <param name="damageTagetLayer">ダメージ対象</param>
    public void Initializa(BulletConfig bulletConfig, LayerMask damageTagetLayer)
    {
        _bulletConfig = bulletConfig;
        _damageTagetLayer = damageTagetLayer;
    }
}
