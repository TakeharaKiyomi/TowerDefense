using System;
using System.Linq;
using UnityEngine;

// FIXME: 移動させる
public enum ItemType
{
    NONE = 0,
    GOLD, // お金
    MAX_NUM
}

public struct ItemEntityData
{
    // アイテム種別
    public ItemType type;
    // アイテム情報
    public object info;

    public ItemEntityData(ItemType type, object info)
    {
        this.type = type;
        this.info = info;
    }
}

public enum EnemyEntityType
{
    NONE = 0,
    STANDARD,    // 標準
    HEAVYARMOR,  // 重装
    MAX_NUM
}

public struct EnemyEntityData
{
    // 敵種別
    public EnemyEntityType type;
    // ドロップ報酬
    public ItemEntityData[] drops;
    // 移動速度
    public float speed;

    public EnemyEntityData(EnemyEntityType type, ItemEntityData[] drops, float speed)
    {
        this.type = type;
        this.drops = drops;
        this.speed = speed;
    }
}

public class EnemyEntityBase : GameEntityBase
{
    protected EnemyEntityData _enemyData;

    protected Vector2Int mapPos;

    // 進行方向
    protected Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
    };

    // 前回の進行方向
    protected Vector2Int previousDirection = Vector2Int.zero;

    // FIXME: 後で命名規則を考える。頭にonをつけるかどうか。
    // ゴール達成した時
    public event Action OnGoalAchieved;
    // 敵が撃破された時
    public event Action<ItemEntityData[]> onDefeated;

    private float _totalMoveDistance = 0.0f;
    public float totalMoveDistance
    {
        get
        {
            return _totalMoveDistance;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected void Update()
    {
        // HPが0以下なら自身を破棄する
        if(_entityData.hitPoint <= 0.0f)
        {
            onDefeated?.Invoke(_enemyData.drops);
            Destroy(gameObject);
            return;
        }

        Vector3 nowPos = transform.position;
        for(int i = 0; i < directions.Length; i++)
        {
            Vector2Int nextPos = mapPos + directions[i];
            if((mapPos - previousDirection) == nextPos) {
                Debug.Log("前にも行った道だよ");
                continue;
            }
            FieldGrid fieldGrid = MapManager.Instance.GetFieldGrid(nextPos);
            // 存在しないMap情報なら、何もしない
            if(fieldGrid == null) continue;
            if(fieldGrid.FieldGridType != FieldGridType.ROAD) continue;
            SpriteRenderer fieldGridSpriteRenderer = fieldGrid.GetComponent<SpriteRenderer>();
            float distance = _enemyData.speed * Time.deltaTime * fieldGridSpriteRenderer.bounds.size.x; // 幅は両方一緒なので片方だけ
            Vector3 distanceVector3 = new Vector3(distance * directions[i].x, distance * -directions[i].y, 0.0f); // FIXME: Zは0加算でよくないか？
            Vector3 nextPosition = transform.position + distanceVector3;
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            // 現在位置のMapから移動した場合
            LayerMask layerMask = LayerMask.GetMask("Map"); // FIXME: どこかでenumにしたい
            Collider2D collider2D = Physics2D.OverlapPoint(nextPosition, layerMask);
            if(collider2D != null)
            {
                // Mapが道以外なら、何もしない
                FieldGrid nextFieldGrid = collider2D.GetComponent<FieldGrid>();
                if(nextFieldGrid.FieldGridType != FieldGridType.ROAD) continue;
                transform.position += distanceVector3;
                // 移動距離の合計に加算
                _totalMoveDistance += Math.Abs(distanceVector3.x) + Math.Abs(distanceVector3.y);

                Collider2D nowPosCollider2D = Physics2D.OverlapPoint(new Vector2(nextPosition.x + (spriteRenderer.bounds.size.x / 2.0f * -directions[i].x), nextPosition.y + (spriteRenderer.bounds.size.y / 2.0f * directions[i].y)), layerMask);
                if(nowPosCollider2D != null)
                {
                    FieldGrid nextFieldGrid2 = nowPosCollider2D.GetComponent<FieldGrid>();
                    Vector2Int? nextPos2 = MapManager.Instance.GetFieldGridPos(nextFieldGrid2);
                    if(nextPos2 == null)
                    {
                        Debug.Log("衝突している筈のマップ位置が不明です。");
                        break;
                    }
                    mapPos = nextPos2.Value;
                }
                previousDirection = directions[i];
                directions = directions[i..].Concat(directions[..i]).ToArray();
                break;
            }
        }
        // 身動きが取れない場合
        if(nowPos == transform.position)
        {
            // 進行方向が無いので破棄する
            Debug.Log("道が存在しないです");
            OnGoalAchieved?.Invoke();
            Destroy(gameObject);
        }
    }

    public new void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        AudioManager.Instance.PlaySE(SEAudioType.ENEMY_DAMAGE);
    }

    public void SetMapPos(Vector2Int mapPos)
    {
        this.mapPos = mapPos;
    }
}
