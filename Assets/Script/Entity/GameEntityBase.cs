using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameEntityType
{
    NONE = 0,           // 未設定
    BASE,               // 拠点
    STANDARDTOWER,      // 標準タワー
    RAPIDFIRETOWER,     // 連射タワー
    STANDARDENEMY,      // 通常敵
    HEAVYARMORENEMY,    // 重装敵
    MAX_NUM
}

public struct GameEntityBaseData
{
    // HP
    public int hitPoint;
    // 攻撃力
    public int attack;
    // 攻撃間隔
    public float attackInterval;
    // 射程
    public float range;
    // 弾速
    public float velocity;

    public GameEntityBaseData(
        int hitPoint = 0, 
        int attack = 0, 
        float attackInterval = 0.0f, 
        float range = 0, 
        float velocity = 0)
    {
        this.hitPoint = hitPoint;
        this.attack = attack;
        this.attackInterval = attackInterval;
        this.range = range;
        this.velocity = velocity;
    }
}

public class GameEntityBase : MonoBehaviour {

    private static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    /* private Dictionary<GameEntityType, string> _mapPrefabStrings = new Dictionary<GameEntityType, string>
    {
        {GameEntityType.BASE, "Map/Road"},
        {GameEntityType.STANDARDTOWER, "Map/Forest"},
        {GameEntityType.RAPIDFIRETOWER, "Map/Forest"},
        {GameEntityType.STANDARDENEMY, "Map/Forest"},
        {GameEntityType.HEAVYARMORENEMY, "Map/Forest"},
    }; */
    // FIXME: 不必要かも
    protected string _type; // 種別
    public string type
    {
        get { return this._type; }
    }

    protected GameEntityBaseData _entityData;
    public GameEntityBaseData EntityData
    {
        get
        {
            return _entityData;
        }
    }

    private Coroutine damageCoroutine;

    // FIXME: 後で見直す
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        _entityData.hitPoint -= damage;

        if(damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        damageCoroutine = StartCoroutine(DamageEffect());
    }

    private IEnumerator DamageEffect()
    {
        Color color = spriteRenderer.color;
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = color;

        damageCoroutine = null;
    }

    protected static GameObject CreateEntity(string typeString, string prefabString)
    {
        // プレハブデータ取得
        GameObject prefab;
        if(prefabs.ContainsKey(typeString))
        {
            prefab = prefabs[typeString];
        } else
        {
            prefab = (GameObject)Resources.Load(prefabString);
            if(prefab == null)
            {
                Debug.Log("指定のプレハブが存在しない。");
                return null;
            }
            // プレハブデータ保存
            prefabs.Add(typeString, prefab);
        }
        // インスタンス生成
        GameObject entity = Instantiate(prefab);
        entity.name = prefab.name;
        return entity;
    }
}
