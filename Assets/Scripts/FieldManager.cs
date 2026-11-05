using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldManager : MonoBehaviour {

    // シングルトン（シーンを跨がずシーン内でのみ唯一無二）
    private static FieldManager _instance;

    public static FieldManager Instance
    {
        get
        {
            // インスタンスが存在しない場合、既に生成しているインスタンスを取得し設定する
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<FieldManager>();
            }
            return _instance;
        }
    }

    // マップをクリック
    public Action<FieldGrid> FieldGridClicked;

    // FIXME: マップ情報は一旦コードで管理して後で他のマスター管理と同じにする
    // マップのマス目情報
    private const int MAX_MAP_X = 16; // 最大横幅 
    private const int MAX_MAP_Y = 10; // 最大縦幅
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

    // マップ情報
    private readonly FieldGrid[,] _mapData = new FieldGrid[MAX_MAP_Y, MAX_MAP_X];

    void Awake()
    {
        _mapParent = new("MapParent");
    }

    /// <summary>
    /// フィールドを生成する
    /// </summary>
    public void CreateEntity()
    {
        // フィールドデータを元に実際のフィールド単一を生成
        float posX = 0.0f;
        float posY = 0.0f;
        for (int i = 0; i < _baseMapData.GetLength(0); i++)
        {
            SpriteRenderer spriteRenderer = null;
            for(int j = 0; j < _baseMapData.GetLength(1); j++)
            {
                FieldGridType type = (FieldGridType)_baseMapData[i, j];
                // フィールド単一をインスタンス生成する
                FieldGrid fieldGrid = FieldGrid.CreateEntity(type);
                fieldGrid.transform.position = new Vector3(posX, posY, 0.0f);
                fieldGrid.name += "(" + j + "_" + i + ")";
                fieldGrid.transform.SetParent(_mapParent.transform);
                // フィールド単一を押下した時のイベントを発火させる
                fieldGrid.Clicked += () => {
                    FieldGridClicked?.Invoke(fieldGrid);
                };
                // 現在位置＋横幅分を次のX座標として設定する
                spriteRenderer = fieldGrid.GetComponent<SpriteRenderer>();
                posX = fieldGrid.transform.position.x + spriteRenderer.bounds.size.x;
                // フィールド単一を保持
                _mapData[i, j] = fieldGrid;
            }
            // 行が変わるのでX座標をリセット
            posX = 0.0f;
            // スプライトレンダラーが存在しない場合、1件も取得できていないので次の行へ進む
            if(spriteRenderer == null) continue;
            // 次の行へY座標を更新する
            posY -= spriteRenderer.bounds.size.y;
        }

        // フィールドの親オブジェクトの大きさを調整する
        _mapParent.transform.localScale = new Vector3(0.75f, 0.75f, 1.0f);
        // フィールドの親オブジェクトの子全体の大きさを元に画面中央にマップを配置
        SpriteRenderer[] renderers = _mapParent.GetComponentsInChildren<SpriteRenderer>();
        if(renderers.Length <= 0) return;
        Bounds bounds = renderers[0].bounds;
        foreach (SpriteRenderer sr in renderers)
        {
            bounds.Encapsulate(sr.bounds);
        }
        // FIXME: カメラのZ軸は本当にこの値で正しいものが取れるか再検討する。
        Camera camera = Camera.main;
        Debug.Log(camera.transform.position.z + " : " + camera.nearClipPlane + " : " + transform.position.z + " : カメラのZ軸だよ");
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

    private bool IsValidIndex(Vector2Int index)
    {
        return  0 <= index.x && 
                0 <= index.y &&
                index.x < MAX_MAP_X &&
                index.y < MAX_MAP_Y;
    }
}
