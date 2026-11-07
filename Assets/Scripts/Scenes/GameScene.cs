using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class GameScene : MonoBehaviour
{
    // ステージの情報
    [SerializeField]
    private StageConfigMaster _stageConfigMaster;

    // タワーの情報
    [SerializeField]
    private List<TowerMaster> _towerMasters;

    [SerializeField]
    private GameObject _canvas;

    [SerializeField]
    private Button _startButton;

    [SerializeField]
    private TextMeshProUGUI _waveLabel;

    [SerializeField]
    private TextMeshProUGUI _hitPointLabel;

    [SerializeField]
    private TextMeshProUGUI _moneyLabel;


    // ゲーム開始か否か
    private bool isGameStart = false;

    private int _hitPoint;
    private int _money;

    void Awake()
    {
        _hitPoint = _stageConfigMaster.initHitPoint;
        _money = _stageConfigMaster.initMoney;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // BGM再生
        AudioManager.Instance.PlayBGM(BGMAudioType.GAME);

        // フィールド情報を生成
        FieldManager.Instance.CreateEntity();
        // フィールドの押下処理
        FieldManager.Instance.FieldGridClicked += (fieldGrid) => {
            PlaceTowerButton selectedButton = PlaceTowerButton.SelectedButton;
            if(selectedButton == null) return; 
            TowerMaster towerMaster = _towerMasters.Find(item => item.type == selectedButton.TowerType);
            if(towerMaster.moneyCost <= _money)
            {
                _money -= towerMaster.moneyCost;

                TowerEntityBase towerGameObject = selectedButton.TowerType switch
                {
                    TowerEntityType.STANDARD    => StandardTower.CreateEntity(),
                    TowerEntityType.RAPIDFIRE   => RapidFireTower.CreateEntity(),
                    _                           => StandardTower.CreateEntity()
                };
                towerGameObject.transform.position = fieldGrid.gameObject.transform.position;
                fieldGrid.SetIsPlaced(true);
                AudioManager.Instance.PlaySE(SEAudioType.TOWER_INSTALLATION);
            }
        };
        // 拠点
        BaseCore baseCore = BaseCore.CreateEntity();
        FieldGrid baseFieldGrid = FieldManager.Instance.GetFieldGrid(_stageConfigMaster.baseFieldPos);
        baseCore.transform.position = baseFieldGrid.transform.position;
        baseFieldGrid.SetIsPlaced(true);

        // 敵
        EnemyManager.Instance.OnEnemyGoalAchieved += (attack) =>
        {
            if(attack < _hitPoint)
            {
                AudioManager.Instance.PlaySE(SEAudioType.BASE_DAMAGE);
                _hitPoint -= attack;
                return;
            }
            // HPが足らない場合、ゲームオーバー画面に進む
            SceneManager.LoadScene(SceneType.GAME_OVER.GetSceneName());
        };
        EnemyManager.Instance.OnAllWaveClear += () =>
        {
            SceneManager.LoadScene(SceneType.GAME_CLEAR.GetSceneName());
        };
        EnemyManager.Instance.OnEnemyDefeated += (drops) =>
        {
            foreach(DropItemBaseMaster drop in drops)
            {
                switch(drop)
                {
                    case GoldItemMaster goldItemMaster:
                        _money += goldItemMaster.amount;
                        break;
                }
            }
        };

        // UI
        float posX = 0.0f;
        foreach(var item in _towerMasters)
        {
            PlaceTowerButton placeTowerButton = PlaceTowerButton.CreateEntity();
            placeTowerButton.Initializa(item);
            placeTowerButton.transform.SetParent(_canvas.transform, false);
            if(item.type == TowerEntityType.STANDARD)
            {
                placeTowerButton.SetSelected();
            }
            // FIXME: 設計上の見直し必要
            RectTransform rect = placeTowerButton.GetComponent<RectTransform>();
            Vector3 pos = placeTowerButton.transform.position;
            pos.x += posX;
            placeTowerButton.transform.position = pos;
            posX += rect.rect.width + 10.0f;
        }
        _startButton.onClick.AddListener(() => {
            isGameStart = true;
            _startButton.interactable = false;
            AudioManager.Instance.PlaySE(SEAudioType.BUTTON_CLICK);
        });
    }

    // Update is called once per frame
    void Update()
    {
        // UI
        _waveLabel.text = "WAVE: " + (EnemyManager.Instance.WaveNowIndex + 1);
        _hitPointLabel.text = "拠点HP: " + _hitPoint;
        _moneyLabel.text = "GOLD: " + _money;

        if(!isGameStart) return;

        // 敵生成
        EnemyManager.Instance.CreateEntity(_stageConfigMaster.enemyFieldInitPos);
    }
}
