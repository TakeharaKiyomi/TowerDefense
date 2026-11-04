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
    private int[,] _baseMapData = new int[MAX_MAP_Y, MAX_MAP_X]
    {
        {2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,1},
        {2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,1},
        {2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,1,2,2,2,2,2,2,2,2,2,2,2,2},
    };

    // マップの全マスを格納した親
    private GameObject _mapParent;

    // マップをクリック
    public Action<FieldGrid> FieldGridClicked;

    // マップ情報
    private FieldGrid[,] _mapData = new FieldGrid[MAX_MAP_Y, MAX_MAP_X];

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
        for (int i = 0; i < _baseMapData.GetLength(0); i++)
        {
            SpriteRenderer spriteRenderer = null;
            for(int j = 0; j < _baseMapData.GetLength(1); j++)
            {
                // インスタンス生成
                FieldGridType type = (FieldGridType)_baseMapData[i, j];
                _mapData[i, j] = FieldGrid.CreateEntity(type);
                spriteRenderer = _mapData[i, j].GetComponent<SpriteRenderer>();
                _mapData[i, j].transform.position = new Vector3(posX, posY, 0.0f);
                _mapData[i, j].name += "(" + j + "_" + i + ")";
                posX = _mapData[i, j].transform.position.x + spriteRenderer.size.x;
                _mapData[i, j].transform.SetParent(_mapParent.transform);
                FieldGrid fieldGrid = _mapData[i, j];
                // TODO: 一時的に保持が必要かは要確認
                // GameObject obj = _mapData[i, j];
                fieldGrid.Clicked += () => onFieldGridClicked(fieldGrid);
            }
            if(spriteRenderer == null) continue;
            posX = 0.0f;
            posY -= spriteRenderer.size.y;
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
        for (int i = 0; i < _mapData.GetLength(0); i++)
        {
            for(int j = 0; j < _mapData.GetLength(1); j++)
            {
                if(_mapData[i, j] != fieldGrid) continue;
                return new Vector2Int(j, i);
            }
        }
        return null; // FIXME: ?は一般的かどうかも要確認
        /* int index = Array.IndexOf(_mapData, fieldGrid.gameObject);
        if(index == -1) return null;
        return new Vector2Int(index % MAX_MAP_X, index / MAX_MAP_X); */
    }

    // FIXME: 全体的に指定したindexが存在するかどうかの条件分岐を入れるかどうかは考える
    public Vector3 GetFieldGridTransformPos(Vector2Int index)
    {
        return _mapData[index.y, index.x].transform.position;
    }

    public FieldGrid GetFieldGrid(Vector2Int index = default)
    {
        // FIXME: マス目が存在しないことも判定に含まれているから安易に削除はできない
        if(!IsValidIndex(index)) return null;
        return _mapData[index.y, index.x].GetComponent<FieldGrid>();
    }

    public Vector2 GetFieldGridSize(Vector2Int index = default)
    {
        // FIXME: 何をしているか要確認。恐らく元のサイズに自身に掛ける予定のスケールをかけているだけ。
        SpriteRenderer spriteRenderer = _mapData[index.y, index.x].GetComponent<SpriteRenderer>();
        return Vector2.Scale(spriteRenderer.sprite.bounds.size, spriteRenderer.transform.lossyScale);
    }

    public void SetFieldGridPlaced(Vector2Int index)
    {
        // int index = pos.x + pos.y * MAX_MAP_X;
        // FieldGrid fieldGrid = _mapData[index.y, index.x].GetComponent<FieldGrid>();
        _mapData[index.y, index.x].GetComponent<FieldGrid>().SetPlaced();
    }

    private void onFieldGridClicked(FieldGrid fieldGrid)
    {
        FieldGridClicked?.Invoke(fieldGrid);
    }

    private bool IsValidIndex(Vector2Int index)
    {
        return  0 <= index.x && 
                0 <= index.y &&
                index.x < MAX_MAP_X &&
                index.y < MAX_MAP_Y;
    }
}
