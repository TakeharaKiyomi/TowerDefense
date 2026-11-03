using System;
using System.Collections.Generic;
using UnityEngine;

public struct EnamyWaveData
{
    // エネミー情報
    public EnemyEntityType[] enemies;
    // 出現間隔
    public float interval;
    public EnamyWaveData(EnemyEntityType[] enemies, float interval)
    {
        this.enemies = enemies;
        this.interval = interval;
    }
}

public class EnemyManager : MonoBehaviour
{
    // マップのプレハブの名前
    private Dictionary<EnemyEntityType, string> _enemyPrefabStrings = new Dictionary<EnemyEntityType, string>
    {
        {EnemyEntityType.STANDARD, "Enemy/StandardEnemy"},
        {EnemyEntityType.HEAVYARMOR, "Enemy/HeavyArmorEnemy"},
    };

    private EnamyWaveData[] _wave = new EnamyWaveData[]
    {
        new EnamyWaveData(
            new EnemyEntityType[] {
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
            }, 
            1.5f
        ),
        new EnamyWaveData(
            new EnemyEntityType[] {
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.HEAVYARMOR,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
            }, 
            1.3f
        ),
        new EnamyWaveData(
            new EnemyEntityType[] {
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.HEAVYARMOR,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.HEAVYARMOR,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
            }, 
            1.2f
        ),
        new EnamyWaveData(
            new EnemyEntityType[] {
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.HEAVYARMOR,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.HEAVYARMOR,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.HEAVYARMOR,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
            }, 
            1.0f
        ),
        new EnamyWaveData(
            new EnemyEntityType[] {
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.HEAVYARMOR,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.HEAVYARMOR,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.HEAVYARMOR,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.HEAVYARMOR,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
                EnemyEntityType.STANDARD,
            }, 
            0.9f
        ),
    };

    private float _timer = 0.0f;

    private int _waveIndex = 0;
    public int WaveIndex
    {
        get
        {
            return _waveIndex;
        }
    }
    private int _waveEnemyIndex = 0;

    private GameObject _enamyParent;

    // 敵のプレハブの実体
    private Dictionary<EnemyEntityType, GameObject> _enemyPrefabs = new Dictionary<EnemyEntityType, GameObject>();

    public event Action<int> OnEnemyGoalAchieved;
    public event Action OnWaveClear;
    public event Action<ItemEntityData[]> onEnemyDefeated;

    // シングルトン
    private static EnemyManager _instance;

    public static EnemyManager Instance
    {
        get
        {
            // 存在しない場合
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<EnemyManager>();
            }
            return _instance;
        }
    }
    // シングルトンにするため、外部から呼ばれないようにする
    private EnemyManager(){
    }

    void Awake()
    {
        _enamyParent = new GameObject();
        _enamyParent.name = "EnemyParent";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateEnemyData(Vector2Int mapPos)
    {
        if(_wave.Length <= _waveIndex)
        {
            OnWaveClear?.Invoke();
            return;
        }
        if(_timer > _wave[_waveIndex].interval)
        {
            // WEAVが終了した場合
            if(_wave[_waveIndex].enemies.Length <= _waveEnemyIndex)
            {
                // 出現中の敵が存在していれば何もしない
                if(0 < _enamyParent.transform.childCount) return;
                _waveIndex ++;
                _waveEnemyIndex = 0;
                return;
            }
            // プレハブデータ取得
            GameObject prefab;
            EnemyEntityType type = _wave[_waveIndex].enemies[_waveEnemyIndex];
            if(_enemyPrefabs.ContainsKey(type))
            {
                prefab = _enemyPrefabs[type];
            } else
            {
                prefab = (GameObject)Resources.Load(_enemyPrefabStrings[type]);
                if(prefab == null)
                {
                    Debug.Log("指定の敵用プレハブが存在しない。");
                    return;
                }
                // プレハブデータ保存
                _enemyPrefabs.Add(type, prefab);
            }
            // インスタンス生成
            GameObject gameObject = Instantiate(prefab);
            gameObject.transform.position = MapManager.Instance.GetFieldGridTransformPos(mapPos);
            gameObject.transform.SetParent(_enamyParent.transform, false);
            EnemyEntityBase enemyEntityBase = gameObject.GetComponent<EnemyEntityBase>();
            enemyEntityBase.SetMapPos(mapPos);
            enemyEntityBase.OnGoalAchieved += () =>
            { 
                OnEnemyGoalAchieved?.Invoke(enemyEntityBase.EntityData.attack);
            };
            enemyEntityBase.onDefeated += onEnemyDefeated;
            _waveEnemyIndex +=1;
            _timer = 0.0f;
            return;
        }
        _timer += Time.deltaTime;
    }
}
