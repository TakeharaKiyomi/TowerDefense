using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // 敵のウェーブ情報
    [SerializeField]
    private EnemyWaveMaster[] _wave;

    // シングルトン（シーンを跨がずシーン内でのみ唯一無二）
    private static EnemyManager _instance;
    public static EnemyManager Instance
    {
        get
        {
            // インスタンスが存在しない場合、既に生成しているインスタンスを取得し設定する
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<EnemyManager>();
            }
            return _instance;
        }
    }

    // 現在ウェーブのインデックス
    private int _waveNowIndex = 0;
    public int WaveNowIndex
    {
        get
        {
            return _waveNowIndex;
        }
    }

    // FIXME: Actionの名前にOnを付ける付けない問題は後で確認する
    // 敵がゴールを達成した時
    public event Action<int> OnEnemyGoalAchieved;

    // 全てのウェーブをクリアした時
    public event Action OnAllWaveClear;

    // 敵が撃破された時
    public event Action<DropItemBaseMaster[]> OnEnemyDefeated;

    // 経過時間
    private float _elapsedTime = 0.0f;

    // 現在ウェーブの敵インデックス
    private int _waveNowEnemyIndex = 0;

    // 親オブジェクト
    private GameObject _enamyParent;

    void Awake()
    {
        // 既にインスタンス生成されている場合は、自身を破棄する
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // 新規で生成されたインスタンスなので保持する
        _instance = this;
        // 親オブジェクトを生成する
        _enamyParent = new("EnemyParent");
    }

    void OnDestroy()
    {
        // 明示的に破棄する必要は無いが、static変数の扱いを統一する為自身なら破棄する
        if(_instance != this) return;
        _instance = null;
    }

    /// <summary>
    /// 敵を生成する
    /// </summary>
    /// <param name="fieldPos">設置するフィールドの位置</param>
    public void CreateEntity(Vector2Int fieldPos)
    {
        // 最大ウェーブを超えた場合、イベントを発火させる
        if(_wave.Length <= _waveNowIndex)
        {
            OnAllWaveClear?.Invoke();
            return;
        }
        // 経過時間が出現間隔を満たない場合、時間を加算して何もしない
        if(_elapsedTime < _wave[_waveNowIndex].interval)
        {
            _elapsedTime += Time.deltaTime;
            return;
        }
        // ウェーブが終了した場合
        if(_wave[_waveNowIndex].enemies.Length <= _waveNowEnemyIndex)
        {
            // 出現中の敵が1体でも存在する場合、何もしない
            if(0 < _enamyParent.transform.childCount) return;
            // 次のウェーブを設定する
            _waveNowIndex ++;
            _waveNowEnemyIndex = 0;
            return;
        }
        // 敵をインスタンス生成する
        EnemyEntityType enemyEntityType = _wave[_waveNowIndex].enemies[_waveNowEnemyIndex];
        EnemyEntityBase enemyScript = enemyEntityType switch
        {
            EnemyEntityType.STANDARD    => StandardEnemy.CreateEntity(),
            EnemyEntityType.HEAVYARMOR  => HeavyArmorEnemy.CreateEntity(),
            _                           => throw new ArgumentException("EnemyEntityType(" + enemyEntityType + ")は不正な値です。")
        };
        // 設置するフィールドの位置を設定する
        enemyScript.SetFieldPos(fieldPos);
        // フィールドの位置から座標を取得し設定する
        enemyScript.transform.position = FieldManager.Instance.GetFieldGridTransformPos(fieldPos);
        // 親オブジェクトを設定する
        enemyScript.transform.SetParent(_enamyParent.transform, false);
        // 敵がゴールに到達した時のイベントを発火させる
        enemyScript.OnGoalAchieved += OnEnemyGoalAchieved;
        // 敵が撃破された時のイベントを設定する
        enemyScript.OnDefeated += OnEnemyDefeated;
        // 現在のウェーブの次の敵を設定する
        _waveNowEnemyIndex +=1;
        // 経過時間をリセット
        _elapsedTime = 0.0f;
    }
}
