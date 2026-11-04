using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyWaveMaster[] _wave;

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

    public event Action<int> OnEnemyGoalAchieved;
    public event Action OnWaveClear;
    public event Action<DropItemBaseMaster[]> OnEnemyDefeated;

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

            // インスタンス生成
            EnemyEntityType type = _wave[_waveIndex].enemies[_waveEnemyIndex];
            EnemyEntityBase enemyScript = type switch
            {
                EnemyEntityType.STANDARD    => StandardEnemy.CreateEntity(),
                EnemyEntityType.HEAVYARMOR  => HeavyArmorEnemy.CreateEntity(),
                _                           => StandardEnemy.CreateEntity()
            };
            enemyScript.transform.position = MapManager.Instance.GetFieldGridTransformPos(mapPos);
            enemyScript.transform.SetParent(_enamyParent.transform, false);
            enemyScript.SetMapPos(mapPos);
            enemyScript.OnGoalAchieved += () =>
            { 
                OnEnemyGoalAchieved?.Invoke(enemyScript.EnemyMaster.attack);
            };
            enemyScript.OnDefeated += OnEnemyDefeated;
            _waveEnemyIndex +=1;
            _timer = 0.0f;
            return;
        }
        _timer += Time.deltaTime;
    }
}
