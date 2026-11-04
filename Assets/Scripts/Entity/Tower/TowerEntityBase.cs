using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public abstract class TowerEntityBase : GameEntityBase
{
    // タワーの情報
    [SerializeField]
    private TowerMaster masterData;

    // 経過時間
    private float _elapsedTime = 0.0f;

    // 射程内の敵を一時的に保存するリスト
    private readonly List<EnemyEntityBase> _enemyEntityBasesTemp = new();

    // Update is called once per frame
    protected void Update()
    {
        // 経過時間が攻撃間隔を満たない場合、時間を加算して何もしない
        if(_elapsedTime < masterData.attackInterval)
        {
            _elapsedTime += Time.deltaTime;
            return;
        }
        // 射程範囲内に敵が存在するか確認し、居なければ何もしない
        Vector2 fieldGridSize = MapManager.Instance.GetFieldGridSize();
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(
            transform.position,
            fieldGridSize * masterData.range,
            0.0f,
            LayerMaskType.ENEMY.GetLayerMask()
        );
        if(collider2Ds.Length <= 0) return;
        // 移動距離の合計が一番多い順に敵をソートする
        foreach(var item in collider2Ds)
        {
            _enemyEntityBasesTemp.Add(item.GetComponent<EnemyEntityBase>());
        }
        _enemyEntityBasesTemp.Sort((a, b) => b.totalMoveDistance.CompareTo(a.totalMoveDistance));
        // 弾をインスタンス生成する
        BulletEntityBase bulletScript = BulletEntityBase.CreateEntity();
        bulletScript.transform.position = transform.position;
        bulletScript.transform.SetParent(gameObject.transform);
        // 弾の情報を設定
        float addDistance = masterData.velocity * Time.deltaTime * fieldGridSize.x;
        Vector3 distance = _enemyEntityBasesTemp[0].transform.position - transform.position;
        bulletScript.SetBulletEntityBaseData(new BulletEntityBaseData(masterData.attack, addDistance, distance.normalized));
        // 経過時間をリセット
        _elapsedTime = 0.0f;
        // 射程内の敵リストを破棄
        _enemyEntityBasesTemp.Clear();
    }
}
