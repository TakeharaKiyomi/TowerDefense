using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    public GameObject canvas;

    [SerializeField]
    public GameObject startButton;
    [SerializeField]
    public GameObject waveLabel;
    [SerializeField]
    public GameObject hitPointLabel;
    [SerializeField]
    public GameObject moneyLabel;
    [SerializeField]
    private List<TowerMaster> towerMasters;

    // FIXME: インスペクター上で設定できるようにする方が良いかも
    // 拠点のMap座標
    private Vector2Int baseMapPos = new Vector2Int(0, 3);

    private Vector2Int enemyMapInitPos = new Vector2Int(3, 9);
    
    /* private Dictionary<TowerEntityType, TowerDisplayData> towerDisplayDatas = new Dictionary<TowerEntityType, TowerDisplayData>
    {
        { TowerEntityType.STANDARD, new TowerDisplayData(50, "標準") },
        { TowerEntityType.RAPIDFIRE, new TowerDisplayData(70, "連射") },
    }; */

    // 最初の更新か否か
    private bool isFirstUpdated = false;
    // ゲーム開始か否か
    private bool isGameStart = false;

    private int _hitPoint = 10;
    private int _money = 150;

    void Awake()
    {
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlayBGM(BGMAudioType.GAME);

        // Map情報を生成し、拠点を設定する
        MapManager.Instance.CreateMapData();
        MapManager.Instance.FieldGridClicked += (fieldGrid) => {
            PlaceTowerButton selectedButton = PlaceTowerButton.SelectedButton;
            if(selectedButton == null) return; 
            TowerMaster towerMaster = towerMasters.Find(item => item.type == selectedButton.TowerType);
            if(towerMaster.moneyCost <= _money)
            {
                _money -= towerMaster.moneyCost;

                GameObject towerGameObject;
                switch(selectedButton.TowerType)
                {
                    case TowerEntityType.STANDARD:
                        towerGameObject = GameEntityBase.CreateEntity(GameEntityPrefabType.STANDARD_TOWER);
                        break;
                    case TowerEntityType.RAPIDFIRE:
                        towerGameObject = GameEntityBase.CreateEntity(GameEntityPrefabType.RAPIDFIRE_TOWER);
                        break;
                    default:
                        return;
                }
                towerGameObject.transform.position = fieldGrid.gameObject.transform.position;
                fieldGrid.SetPlaced();
                AudioManager.Instance.PlaySE(SEAudioType.TOWER_INSTALLATION);
            }
        };
        GameObject basePrefab = (GameObject)Resources.Load("Base/base");
        GameObject baseObj = Instantiate(basePrefab);
        baseObj.name = basePrefab.name;
        baseObj.transform.position = MapManager.Instance.GetFieldGridTransformPos(baseMapPos);
        MapManager.Instance.SetFieldGridPlaced(baseMapPos);

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
            // FIXME: 画面遷移させる
            Debug.Log("ゲームオーバー画面に進む。");
            SceneManager.LoadScene(SceneNames.gameOver);
        };
        EnemyManager.Instance.OnWaveClear += () =>
        {
            Debug.Log("ゲームクリア画面に進む。");
            SceneManager.LoadScene(SceneNames.gameClear);
        };
        EnemyManager.Instance.onEnemyDefeated += (drops) =>
        {
            foreach(ItemEntityData drop in drops)
            {
                switch(drop.type)
                {
                    case ItemType.GOLD:
                        _money += (int)drop.info;
                        break;
                }
            }
        };

        // UI
        GameObject buttonPrefab = (GameObject)Resources.Load("UI/Button/PlaceTowerButton");
        float posX = 0.0f;
        foreach(var item in towerMasters)
        {
            GameObject buttonObj = Instantiate(buttonPrefab);
            buttonObj.name = buttonPrefab.name;
            PlaceTowerButton buttonScript = buttonObj.GetComponent<PlaceTowerButton>();
            buttonScript.SetTextAndType(item);
            buttonObj.transform.SetParent(canvas.transform, false);
            // FIXME: 設計上の見直し必要
            RectTransform rect = buttonObj.GetComponent<RectTransform>();
            Vector3 pos = buttonObj.transform.position;
            pos.x += posX;
            buttonObj.transform.position = pos;
            posX += rect.rect.width + 10.0f;
        }
        Button startButtonScript = startButton.GetComponent<Button>();
        startButtonScript.onClick.AddListener(() => {
            isGameStart = true;
            startButtonScript.interactable = false;
        });
    }

    // Update is called once per frame
    void Update()
    {
        /* if(!isFirstUpdated && PlaceTowerButton.SelectedButton != null)
        {
            MapManager.Instance.SetupClicked();
            isFirstUpdated = true;
        } */

        // UI
        TextMeshProUGUI waveLabelScript = waveLabel.GetComponent<TextMeshProUGUI>();
        waveLabelScript.text = "WAVE: " + (EnemyManager.Instance.WaveIndex + 1);
        TextMeshProUGUI hitPointLabelScript = hitPointLabel.GetComponent<TextMeshProUGUI>();
        hitPointLabelScript.text = "拠点HP: " + _hitPoint;
        TextMeshProUGUI moneyLabelScript = moneyLabel.GetComponent<TextMeshProUGUI>();
        moneyLabelScript.text = "GOLD: " + _money;

        if(!isGameStart) return;

        // 敵生成
        EnemyManager.Instance.CreateEnemyData(enemyMapInitPos);

    }

    /* private void CreateTower(Vector3 pos)
    {
        // FIXME: 後でタワーマネージャーを作成する
        GameObject prefab;
        switch(PlaceTowerButton.SelectedButton.TowerData.type)
        {
            case TowerEntityType.STANDARD:
                prefab = (GameObject)Resources.Load("Tower/StandardTower");
                break;
            case TowerEntityType.RAPIDFIRE:
                prefab = (GameObject)Resources.Load("Tower/RapidFireTower");
                break;
            default:
                // それ以外は何もしない
                return;
        }
        GameObject tower = Instantiate(prefab);
        tower.name = prefab.name;
        tower.transform.position = pos;
    } */
}
