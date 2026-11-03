using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public enum TowerEntityType
{
    NONE = 0,
    STANDARD,   // 標準
    RAPIDFIRE,  // 連射
    MAX_NUM
}

// FIXME: ここで定義しない方が良いかも
public struct TowerDisplayData
{
    // 配置コスト
    public int moneyCost;
    // タワー名
    public string towerName;

    public TowerDisplayData(
        int moneyCost = 0, 
        string towerName = "")
    {
        this.moneyCost = moneyCost;
        this.towerName = towerName;
    }
}

public class TowerEntityBase : GameEntityBase
{
    private GameObject bulletprefab;

    private float _timer = 0.0f;
    /* public static Dictionary<GameEntityType, string> _mapPrefabStrings = new Dictionary<GameEntityType, string>
    {
        {GameEntityType.BASE, "Map/Road"},
        {GameEntityType.STANDARDTOWER, "Map/Forest"},
        {GameEntityType.RAPIDFIRETOWER, "Map/Forest"},
        {GameEntityType.STANDARDENEMY, "Map/Forest"},
        {GameEntityType.HEAVYARMORENEMY, "Map/Forest"},
    }; */
    // FIXME: 削除するかも
    protected TowerDisplayData _towerData;

    void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected new void Start()
    {
        bulletprefab = (GameObject)Resources.Load("Bullet/Bullet");
    }

    // Update is called once per frame
    protected void Update()
    {
        if(_timer <_entityData.attackInterval)
        {
            _timer += Time.deltaTime;
            return;
        }
        FieldGrid fieldGrid = MapManager.Instance.GetFieldGrid(Vector2Int.zero);
        if(fieldGrid == null) return;
        Vector3 fieldGridSize = fieldGrid.GetComponent<SpriteRenderer>().bounds.size;
        LayerMask layerMask = LayerMask.GetMask("Enemy"); // FIXME: どこかでenumにしたい
        Vector2 size = new Vector2( fieldGridSize.x * _entityData.range,
                                    fieldGridSize.y * _entityData.range);
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position, size, 0.0f, layerMask);
        if(collider2Ds.Length <= 0) return;
        EnemyEntityBase[] enemyEntityBases = collider2Ds.Select(x => x.GetComponent<EnemyEntityBase>()).ToArray();
        Array.Sort(enemyEntityBases, (a, b) => b.totalMoveDistance.CompareTo(a.totalMoveDistance));
        float addDistance = _entityData.velocity * Time.deltaTime * fieldGridSize.x; // 幅は両方一緒なので片方だけ
        
        Vector3 distance = enemyEntityBases[0].transform.position - transform.position;
        GameObject bulletGameObject = Instantiate(bulletprefab);
        bulletGameObject.name = bulletprefab.name;
        bulletGameObject.transform.position = transform.position;
        bulletGameObject.GetComponent<BulletEntityBase>().SetBulletEntityBaseData(new BulletEntityBaseData(_entityData.attack, addDistance, distance.normalized));
        bulletGameObject.transform.SetParent(gameObject.transform);
        _timer = 0.0f;
    }
}
