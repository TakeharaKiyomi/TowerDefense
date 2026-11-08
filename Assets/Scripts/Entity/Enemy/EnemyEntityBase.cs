using System;
using System.Linq;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class EnemyEntityBase : GameEntityBase, IBulletDamage
{
    // 敵の情報
    [SerializeField]
    protected EnemyMaster _enemyMaster;

    // 移動距離の合計
    private float _totalMoveDistance = 0.0f;
    public float TotalMoveDistance
    {
        get
        {
            return _totalMoveDistance;
        }
    }

    // ゴールを達成した時
    public event Action<int> OnGoalAchieved;

    // 敵が撃破された時
    public event Action<DropItemBaseMaster[]> OnDefeated;

    // 一時停止する用クラス
    private readonly static WaitForSeconds _waitForSeconds = new(0.1f);

    // 現在のHP
    private int _hitPoint;

    // フィールド上での現在位置
    protected Vector2Int _fieldNowPos;

    // 現在の進行方向
    private Vector2Int[] _directions;

    // 前回の進行方向
    private Vector2Int _previousDirection = default;

    // ダメージ用コルーチン
    private Coroutine _damageCoroutine;

    // ベースの色
    private Color _baseColor;

    private SpriteRenderer spriteRenderer;

    protected void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _baseColor = spriteRenderer.color;
        _hitPoint = _enemyMaster.hitPoint;
        _directions = _enemyMaster.directions;
    }

    protected void Update()
    {
        // HPが0以下なら自身を破棄する
        if(_hitPoint <= 0)
        {
            OnDefeated?.Invoke(_enemyMaster.drops);
            Destroy(gameObject);
            return;
        }

        Vector3 gameObjectNowPos = transform.position;
        for(int i = 0; i < _directions.Length; i++)
        {
            // 進行方向のフィールド位置を取得
            Vector2Int nextDirectionFieldPos = _fieldNowPos + _directions[i];
            // 前回通った道の場合、次の進行方向へ進む
            if((_fieldNowPos - _previousDirection) == nextDirectionFieldPos) {
                continue;
            }
            FieldGrid nextDirectionFieldGrid = FieldManager.Instance.GetFieldGrid(nextDirectionFieldPos);
            // 存在しないフィールド、道ではない場合、次の進行方向へ進む
            if(nextDirectionFieldGrid == null || nextDirectionFieldGrid.FieldGridType != FieldGridEntityType.ROAD) continue;
            Vector3 fieldGridSize = nextDirectionFieldGrid.SpriteRenderer.bounds.size;
            // 移動量
            float distance = _enemyMaster.speed * Time.deltaTime * fieldGridSize.x;
            // 移動量に進行方向を掛ける（管理しているフィールドのYの位置がワールド座標と逆なので逆の符号を掛ける）
            Vector3 distanceVector3 = new (
                distance *  _directions[i].x,
                distance * -_directions[i].y,
                0.0f
            );
            // 半分のサイズに進行方向を掛ける（全ての幅を考慮する必要があるので中心座標に半分の幅を足し込む）
            Vector3 enemyHalfSize = new (
                spriteRenderer.bounds.extents.x * -_directions[i].x,
                spriteRenderer.bounds.extents.y *  _directions[i].y,
                0.0f
            );
            // 現在位置＋移動量＋半分サイズを加算した位置を判定する次の座標位置として扱う
            Vector3 nextGameObjectPos = transform.position + distanceVector3 + enemyHalfSize;
            // 次の現在位置のフィールド情報を取得
            LayerMask layerMask = new LayerMaskType[] { LayerMaskType.FIELD_GRID }.GetLayerMask();
            Collider2D collider2D = Physics2D.OverlapPoint(nextGameObjectPos, layerMask);
            if(collider2D != null)
            {
                FieldGrid nextFieldGrid = collider2D.GetComponent<FieldGrid>();
                // 道ではない場合、次の進行方向へ進む
                if(nextFieldGrid.FieldGridType != FieldGridEntityType.ROAD) continue;
                // 現在位置に移動量を加算
                transform.position += distanceVector3;
                // 移動距離の合計に加算
                _totalMoveDistance += Math.Abs(distanceVector3.x) + Math.Abs(distanceVector3.y);
                // 当たっているフィールド部分からフィールド上の位置情報を取得する
                Vector2Int nextFieldGridPos = FieldManager.Instance.GetFieldGridPos(nextFieldGrid);
                // 存在しないフィールドだった場合、何もしない
                if(nextFieldGridPos == FieldManager.NOT_EXIST_FIELD_GRID_POS)
                {
                    Debug.Log("位置が不明なフィールドと衝突しています。");
                    break;
                }
                // 現在のフィールド位置を設定
                _fieldNowPos = nextFieldGridPos;
                // 現在の進行方向を保存
                _previousDirection = _directions[i];
                // 現在の進行方向から順番になるように順番を更新
                // FIXME: ToArray関数の毎フレームの実行はメモリのゴミが残るらしいので要修正
                _directions = _directions[i..].Concat(_directions[..i]).ToArray();
                break;
            }
        }
        // 身動きが取れない場合、進行不可の為自身を破棄する
        if(gameObjectNowPos == transform.position)
        {
            OnGoalAchieved?.Invoke(_enemyMaster.attack);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void TakeDamage(int damage)
    {
        // ダメージ量を減算
        _hitPoint -= damage;
        // 既に演出が発火していれば停止させる
        if(_damageCoroutine != null)
        {
            StopCoroutine(_damageCoroutine);
        }
        // 新たに演出を開始する
        _damageCoroutine = StartCoroutine(DamageEffect());
        // SE
        AudioManager.Instance.PlaySE(SEAudioType.ENEMY_DAMAGE);
    }

    /// <summary>
    /// 現在のフィールド位置を設定する
    /// </summary>
    /// <param name="fieldPos">フィールド位置</param>
    public void SetFieldPos(Vector2Int fieldPos)
    {
        _fieldNowPos = fieldPos;
    }

    /// <summary>
    /// ダメージの演出をする
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageEffect()
    {
        // ダメージの色を設定
        spriteRenderer.color = Color.red;
        // 一時的に停止
        yield return _waitForSeconds;
        // 色を元に戻す
        spriteRenderer.color = _baseColor;
        // 設定しているコルーチンを破棄する
        _damageCoroutine = null;
    }
}
