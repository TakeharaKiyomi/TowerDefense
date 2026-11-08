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

    // キャンバス
    [SerializeField]
    private GameObject _canvas;

    // スタートボタン
    [SerializeField]
    private Button _startButton;

    // ウェーブ数の表示ラベル
    [SerializeField]
    private TextMeshProUGUI _waveLabel;

    // 現在HPの表示ラベル
    [SerializeField]
    private TextMeshProUGUI _hitPointLabel;

    // 所持金額の表示ラベル
    [SerializeField]
    private TextMeshProUGUI _moneyLabel;

    // ゲーム開始か否か
    private bool isGameStart = false;

    // 現在HP
    private int _hitPoint;

    // 所持金額
    private int _money;

    void Awake()
    {
        _hitPoint = _stageConfigMaster.initHitPoint;
        _money = _stageConfigMaster.initMoney;
    }

    void Start()
    {
        // BGM再生
        AudioManager.Instance.PlayBGM(BGMAudioType.GAME);

        // フィールド情報を生成する
        FieldManager.Instance.CreateEntity();
        // フィールド単一の押下処理
        FieldManager.Instance.FieldGridClicked += (fieldGrid) => {
            PlaceTowerButton selectedButton = PlaceTowerButton.SelectedButton;
            // 選択中のボタンがない場合、何もしない
            if(selectedButton == null) return;
            // タワー種別が同じタワー情報を取得する
            TowerMaster towerMaster = _towerMasters.Find(item => item.type == selectedButton.TowerType);
            // 配置コスト分以上の所持金額が超える場合、配置コストを減算しタワーを生成する
            if(towerMaster.moneyCost <= _money)
            {
                _money -= towerMaster.moneyCost;

                TowerEntityBase towerGameObject = selectedButton.TowerType switch
                {
                    TowerEntityType.STANDARD    => StandardTower.CreateEntity(),
                    TowerEntityType.RAPIDFIRE   => RapidFireTower.CreateEntity(),
                    _                           => throw new InvalidOperationException(nameof(TowerEntityType) + "(" + selectedButton.TowerType + ")")
                };
                // フィールドの座標をタワーの座標に設定する
                towerGameObject.transform.position = fieldGrid.gameObject.transform.position;
                // フィールド単一を設置済みにする
                fieldGrid.SetIsPlaced(true);
                AudioManager.Instance.PlaySE(SEAudioType.TOWER_INSTALLATION);
            }
        };

        // 拠点を生成する
        BaseCore baseCore = BaseCore.CreateEntity();
        // 拠点を設置するフィールド単一情報を取得する
        FieldGrid baseFieldGrid = FieldManager.Instance.GetFieldGrid(_stageConfigMaster.baseFieldPos);
        // フィールドの座標を拠点の座標に設定する
        baseCore.transform.position = baseFieldGrid.transform.position;
        // フィールド単一を設置済みにする
        baseFieldGrid.SetIsPlaced(true);

        // 敵がゴールを達成した時
        EnemyManager.Instance.OnEnemyGoalAchieved += (attack) =>
        {
            // 攻撃力より現在HPが足りている場合、現在HPを減算する
            if(attack < _hitPoint)
            {
                _hitPoint -= attack;
                AudioManager.Instance.PlaySE(SEAudioType.BASE_DAMAGE);
                return;
            }
            SceneManager.LoadScene(SceneType.GAME_OVER.GetSceneName());
        };
        // 敵が全てのウェーブをクリアした時
        EnemyManager.Instance.OnAllWaveClear += () =>
        {
            SceneManager.LoadScene(SceneType.GAME_CLEAR.GetSceneName());
        };
        // 敵が撃破された時
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
        // タワー設置用ボタンを生成する
        float addPlaceTowerButtonPosX = 0.0f;
        foreach(var item in _towerMasters)
        {
            PlaceTowerButton placeTowerButton = PlaceTowerButton.CreateEntity();
            placeTowerButton.Initializa(item);
            placeTowerButton.transform.SetParent(_canvas.transform, false);
            // 表示位置を設定する
            RectTransform rect = placeTowerButton.GetComponent<RectTransform>();
            placeTowerButton.transform.position += new Vector3(addPlaceTowerButtonPosX, 0.0f, 0.0f);
            addPlaceTowerButtonPosX += rect.rect.width;
        }
        // スタートボタンを押下
        _startButton.onClick.AddListener(() => {
            isGameStart = true;
            _startButton.interactable = false;
            AudioManager.Instance.PlaySE(SEAudioType.BUTTON_CLICK);
        });
    }

    void Update()
    {
        // UI周りの文言を更新する
        _waveLabel.text = "WAVE: " + (EnemyManager.Instance.WaveNowIndex + 1);
        _hitPointLabel.text = "拠点HP: " + _hitPoint;
        _moneyLabel.text = "GOLD: " + _money;

        // ゲーム開始前の場合、何もしない
        if(!isGameStart) return;

        // 敵を生成する
        EnemyManager.Instance.CreateEntity(_stageConfigMaster.enemyFieldInitPos);
    }
}
