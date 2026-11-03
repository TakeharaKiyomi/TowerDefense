using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public enum LayerMaskType
{
    NONE = 0,
    ENEMY,
    FIELD_GRID,
    MAX_NUM
}

public class TowerEntityBase : GameEntityBase
{
    [SerializeField]
    private TowerMaster masterData;

    private float _attackInterval = 0.0f;

    // Update is called once per frame
    protected void Update()
    {
        // 攻撃する時間でない場合、時間を加算して何もしない
        if(_attackInterval <masterData.attackInterval)
        {
            _attackInterval += Time.deltaTime;
            return;
        }
        FieldGrid fieldGrid = MapManager.Instance.GetFieldGrid(Vector2Int.zero);
        Vector3 fieldGridSize = fieldGrid.GetComponent<SpriteRenderer>().bounds.size;
        LayerMask layerMask = LayerMask.GetMask("Enemy"); // FIXME: どこかでenumにしたい
        Vector2 size = new Vector2( fieldGridSize.x * masterData.range,
                                    fieldGridSize.y * masterData.range);
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position, size, 0.0f, layerMask);
        if(collider2Ds.Length <= 0) return;
        EnemyEntityBase[] enemyEntityBases = collider2Ds.Select(x => x.GetComponent<EnemyEntityBase>()).ToArray();
        Array.Sort(enemyEntityBases, (a, b) => b.totalMoveDistance.CompareTo(a.totalMoveDistance));
        float addDistance = masterData.velocity * Time.deltaTime * fieldGridSize.x; // 幅は両方一緒なので片方だけ
        
        Vector3 distance = enemyEntityBases[0].transform.position - transform.position;
        GameObject bulletGameObject = CreateEntity(GameEntityPrefabType.STANDARD_BULLET);
        bulletGameObject.transform.position = transform.position;
        bulletGameObject.GetComponent<BulletEntityBase>().SetBulletEntityBaseData(new BulletEntityBaseData(masterData.attack, addDistance, distance.normalized));
        bulletGameObject.transform.SetParent(gameObject.transform);
        _attackInterval = 0.0f;
    }
}
