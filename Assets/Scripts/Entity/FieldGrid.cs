using System;
using NUnit.Framework;
using UnityEngine;

public class FieldGrid : MonoBehaviour
{
    [SerializeField]
    private FieldGridType _fieldGridType; // マス種別
    public FieldGridType FieldGridType // FIXME: 大文字で問題ない？
    {
        get { return _fieldGridType; }
    }
    // 配置済みか否か
    public bool isPlaced = false;

    public event Action Clicked;

    void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        // FIXME: タワー設置が出来ないのに設置済みになってしまう不具合
        // 押下イベントが未登録、種別が道、既に配置済みの場合は何もしない
        if(Clicked == null || FieldGridType == FieldGridType.ROAD || isPlaced) return;
        Debug.Log(name + "(" + FieldGridType + ")" + "が押されたよ！");
        // isPlaced = true;
        Clicked?.Invoke();
    }

    // FIXME: 仮置き
    public void SetPlaced()
    {
        isPlaced = true;
    }
}
