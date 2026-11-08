using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FieldGrid : GameEntityBase
{
    // フィールド単一の種別
    [SerializeField]
    private FieldGridEntityType _fieldGridType;
    public FieldGridEntityType FieldGridType
    {
        get
        { 
            return _fieldGridType;
        }
    }

    private SpriteRenderer _spriteRenderer = default;
    public SpriteRenderer SpriteRenderer
    {
        get
        {
            return _spriteRenderer;
        }
    }

    // フィールドを押下した時
    public event Action Clicked;
    
    // 配置済みか否か
    private bool isPlaced = false;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        // 押下イベントが未登録、種別が道、既に配置済みの場合は何もしない
        if(Clicked == null || FieldGridType == FieldGridEntityType.ROAD || isPlaced) return;
        Debug.Log(name + "(" + FieldGridType + ")" + "を押下");
        Clicked?.Invoke();
    }

    /// <summary>
    /// 設置済みか否かのフラグを設定する
    /// </summary>
    /// <param name="flag">設置済みか否かのフラグ</param>
    public void SetIsPlaced(bool flag)
    {
        isPlaced = flag;
    }

    /// <summary>
    /// インスタンスを生成する
    /// </summary>
    /// <param name="type">フィールドの種別</param>
    /// <returns>生成されたインスタンス</returns>
    /// <exception cref="ArgumentException">想定外の種別が指定されていた場合</exception>
    public static FieldGrid CreateEntity(FieldGridEntityType type)
    {
        GameEntityPrefabType prefabType = type switch
        {
            FieldGridEntityType.ROAD    => GameEntityPrefabType.FIELD_GRID_ROAD,
            FieldGridEntityType.FOREST  => GameEntityPrefabType.FIELD_GRID_FOREST,
            _                           => throw new ArgumentException(nameof(FieldGridEntityType) + "(" + type + ")")
        };
        return CreateEntity<FieldGrid>(prefabType);
    }
}
