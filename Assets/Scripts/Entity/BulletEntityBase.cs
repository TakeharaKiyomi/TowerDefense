using UnityEngine;

public struct BulletEntityBaseData
{
    // 攻撃力
    public int attack;
    // 加算距離
    public float addDistance;
    // 進行方向
    public Vector3 direction;

    public BulletEntityBaseData(int attack, float addDistance, Vector3 direction)
    {
        this.attack = attack;
        this.addDistance = addDistance;
        this.direction = direction;
    }
}

public class BulletEntityBase : GameEntityBase
{
    private BulletEntityBaseData _bulletData;

    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 移動
        transform.position += new Vector3(_bulletData.addDistance * _bulletData.direction.x, _bulletData.addDistance * _bulletData.direction.y, 0.0f);

        // 攻撃
        LayerMask layerMask = LayerMask.GetMask("Enemy"); // FIXME: どこかでenumにしたい
        Collider2D collider2D = Physics2D.OverlapPoint(transform.position, layerMask);
        if(collider2D != null)
        {
            EnemyEntityBase enemy = collider2D.GetComponent<EnemyEntityBase>();
            enemy.TakeDamage(_bulletData.attack);
            Destroy(gameObject);
            return;
        }

        // 画面外に行ったら自身を破棄する
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

    public void SetBulletEntityBaseData(BulletEntityBaseData bulletData)
    {
        _bulletData = bulletData;
    }

    public static BulletEntityBase CreateEntity()
    {
        return CreateEntity<BulletEntityBase>(GameEntityPrefabType.STANDARD_BULLET);
    }
}
