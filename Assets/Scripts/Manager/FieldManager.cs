using System;
using UnityEngine;

public class FieldManager : MonoBehaviour {

    [SerializeField]
    private FieldMaster fieldMaster;
    
    // 存在しないフィールド座標
    public static readonly Vector2Int NOT_EXIST_FIELD_GRID_POS = new(-1, -1);

    // シングルトン（シーンを跨がずシーン内でのみ唯一無二）
    private static FieldManager _instance;

    public static FieldManager Instance
    {
        get
        {
            return _instance;
        }
    }

    // フィールド単一をクリック
    public event Action<FieldGrid> FieldGridClicked;

    // 全てのフィールドオブジェクトのコンテナ
    private GameObject _fieldContainer;

    // 全フィールド情報
    private readonly FieldGrid[,] _fieldData = new FieldGrid[FieldMaster.Height, FieldMaster.Width];

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
        // 全てのフィールドオブジェクトのコンテナを生成する
        _fieldContainer = new("FieldContainer");
    }

    void OnDestroy()
    {
        if(_instance != this) return;
        _instance = null;
    }

    /// <summary>
    /// フィールドを生成する
    /// </summary>
    public void CreateEntity()
    {
        // フィールドデータを元に実際のフィールド単一を生成
        Bounds bounds = default;
        float posX = 0.0f;
        float posY = 0.0f;
        for (int i = 0; i < FieldMaster.Height; i++)
        {
            for(int j = 0; j < FieldMaster.Width; j++)
            {
                FieldGridEntityType type = fieldMaster.grids[(i * FieldMaster.Width) + j];
                // フィールド単一をインスタンス生成する
                FieldGrid fieldGrid = FieldGrid.CreateEntity(type);
                fieldGrid.transform.position = new Vector3(posX, posY, 0.0f);
                fieldGrid.name += "(" + j + "_" + i + ")";
                fieldGrid.transform.SetParent(_fieldContainer.transform);
                // フィールド単一を押下した時のイベントを発火させる
                fieldGrid.Clicked += () => {
                    FieldGridClicked?.Invoke(fieldGrid);
                };
                // 現在位置＋横幅を次のX座標として設定する
                posX = fieldGrid.transform.position.x + fieldGrid.SpriteRenderer.bounds.size.x;
                // フィールド単一を保持
                _fieldData[i, j] = fieldGrid;
                // 親オブジェクトの中心を全フィールドの位置の中心に移動させるため、境界ボックスを格納する
                bounds.Encapsulate(fieldGrid.SpriteRenderer.bounds);
            }
            // 行が変わるのでX座標をリセット
            posX = 0.0f;
            // 既に生成されている場合、生成済みの高さを取得し次の行へY座標を更新する
            posY -= 0 <= _fieldData.Length ? _fieldData[0, 0].SpriteRenderer.bounds.size.y : 0.0f;
        }
        // 全フィールドの位置を中心に移動させるため、境界ボックスの中心値を引く
        foreach(var item in _fieldData)
        {
            item.transform.position -= bounds.center;
        }
    }
    
    /// <summary>
    /// 指定のフィールド情報からフィールドの位置を取得する
    /// </summary>
    /// <param name="fieldGrid">位置を取得したいフィールド情報</param>
    /// <returns></returns>
    public Vector2Int GetFieldGridPos(FieldGrid fieldGrid)
    {
        for (int i = 0; i < _fieldData.GetLength(0); i++)
        {
            for(int j = 0; j < _fieldData.GetLength(1); j++)
            {
                if(_fieldData[i, j] != fieldGrid) continue;
                return new Vector2Int(j, i);
            }
        }
        return NOT_EXIST_FIELD_GRID_POS;
    }

    /// <summary>
    /// 指定の位置のフィールド情報を取得する
    /// </summary>
    /// <param name="pos">指定の位置</param>
    /// <returns></returns>
    public FieldGrid GetFieldGrid(Vector2Int pos = default)
    {
        if(!IsValidIndex(pos)) return null;
        return _fieldData[pos.y, pos.x];
    }

    /// <summary>
    /// 指定の位置が有効なフィールドの位置であるかを返す
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool IsValidIndex(Vector2Int pos)
    {
        return  0 <= pos.x && 
                0 <= pos.y &&
                pos.x < FieldMaster.Width &&
                pos.y < FieldMaster.Height;
    }
}
