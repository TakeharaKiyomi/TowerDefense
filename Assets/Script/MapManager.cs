using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapManager : MonoBehaviour {

    // マップのプレハブの名前
    private Dictionary<FieldGridType, string> _mapPrefabStrings = new Dictionary<FieldGridType, string>
    {
        {FieldGridType.ROAD, "Map/Road"},
        {FieldGridType.FOREST, "Map/Forest"},
    };
    // マップのプレハブの実体
    private Dictionary<FieldGridType, GameObject> _mapPrefabs = new Dictionary<FieldGridType, GameObject>();

    // マップのマス目情報
    public const int MAX_MAP_X = 16; // 最大横幅 
    public const int MAX_MAP_Y = 10; // 最大縦幅
    private int[] _baseMapData = new int[MAX_MAP_X * MAX_MAP_Y]
    {
        2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,
        2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,1,
        2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,1,
        2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,1,
        1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
        2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,
        2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,
        2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,
        2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,
        2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2,
    };

    // マップの全マスを格納した親
    private GameObject _mapParent;

    // マップをクリック
    public Action<Vector3> FieldGridClicked;

    // マップ情報
    private GameObject[] _mapData = new GameObject[MAX_MAP_X * MAX_MAP_Y];

    // シングルトン
    private static MapManager _instance;

    public static MapManager Instance
    {
        get
        {
            // 存在しない場合
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<MapManager>();
            }
            return _instance;
        }
    }
    // シングルトンにするため、外部から呼ばれないようにする
    private MapManager(){
    }

    void Awake()
    {
        _mapParent = new GameObject();
        _mapParent.name = "MapParent";
    }

    // マップ情報生成処理
    public void CreateMapData()
    {
        // マップデータを元に実際のマップOBJを生成
        float posX = 0.0f;
        float posY = 0.0f;
        for (int i = 0; i < _baseMapData.Length; i++)
        {
            // プレハブデータ取得
            GameObject prefab;
            FieldGridType type = (FieldGridType)_baseMapData[i];
            if(_mapPrefabs.ContainsKey(type))
            {
                prefab = _mapPrefabs[type];
            } else
            {
                prefab = (GameObject)Resources.Load(_mapPrefabStrings[type]);
                if(prefab == null)
                {
                    Debug.Log("指定のMap用プレハブが存在しない。");
                    return;
                }
                // プレハブデータ保存
                _mapPrefabs.Add(type, prefab);
            }
            // インスタンス生成
            _mapData[i] = Instantiate(prefab);

            SpriteRenderer renderer = _mapData[i].GetComponent<SpriteRenderer>();
            if ((i % MAX_MAP_X) == 0)
            {
                posX = 0.0f;
                posY -= renderer.size.y;
            }
            _mapData[i].transform.position = new Vector3(posX, posY, 0.0f);
            _mapData[i].name = "map_" + (i % MAX_MAP_X).ToString() + "_" + (i / MAX_MAP_X).ToString();
            posX = _mapData[i].transform.position.x + renderer.size.x;
            _mapData[i].transform.SetParent(_mapParent.transform);
            // FieldGrid fieldGrid = _mapData[i].GetComponent<FieldGrid>();
            // TODO: 一時的に保持が必要かは要確認
            // GameObject obj = _mapData[i];
            // fieldGrid.Clicked += () => onFieldGridClicked(obj.transform.position);
        }

        // マップの大きさ調整
        _mapParent.transform.localScale = new Vector3(0.75f, 0.75f, 1.0f);
        // マップの親OBJの子全体の大きさを元に画面中央にマップを配置
        SpriteRenderer[] renderers = _mapParent.GetComponentsInChildren<SpriteRenderer>();
        Bounds bounds = renderers[0].bounds;
        foreach (SpriteRenderer sr in renderers)
        {
            bounds.Encapsulate(sr.bounds);
        }
        Camera camera = Camera.main;
        Vector3 center = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Mathf.Abs(camera.transform.position.z)));
        _mapParent.transform.position += center - bounds.center;
    }
    
    public Vector2Int? GetFieldGridPos(FieldGrid fieldGrid)
    {
        int index = Array.IndexOf(_mapData, fieldGrid.gameObject);
        if(index == -1) return null;
        return new Vector2Int(index % MAX_MAP_X, index / MAX_MAP_X);
    }

    public Vector3 GetFieldGridTransformPos(Vector2Int pos)
    {
        return _mapData[pos.x + pos.y * MAX_MAP_X].transform.position;
    }

    public FieldGrid GetFieldGrid(Vector2Int pos)
    {
        if(
            pos.x < 0 || 
            pos.y < 0 || 
            (pos.x + pos.y * MAX_MAP_X) >= _mapData.Length ||
            MAX_MAP_X < pos.x ||
            MAX_MAP_Y < pos.y) return null;
        return _mapData[pos.x + pos.y * MAX_MAP_X].GetComponent<FieldGrid>();
    }

    public void SetFieldGridPlaced(Vector2Int pos)
    {
        int index = pos.x + pos.y * MAX_MAP_X;
        FieldGrid fieldGrid = _mapData[index].GetComponent<FieldGrid>();
        fieldGrid.setPlaced();
    }

    public void SetupClicked()
    {
        foreach(GameObject gameObject in _mapData)
        {
            FieldGrid fieldGrid = gameObject.GetComponent<FieldGrid>();
            fieldGrid.Clicked += () => onFieldGridClicked(gameObject.transform.position);
        }
    }

    private void onFieldGridClicked(Vector3 pos)
    {
        FieldGridClicked?.Invoke(pos);
    }
}
