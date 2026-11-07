using UnityEngine.UI;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Button))]
public class PlaceTowerButton : GameEntityBase
{
    // 選択状態にあるボタン情報
    private static PlaceTowerButton _selectedButton = null;
    public static PlaceTowerButton SelectedButton
    {
        get
        {
            return _selectedButton;
        }
    }

    // タワー種別
    private TowerEntityType _towerType;
    public TowerEntityType TowerType
    {
        get
        {
            return _towerType;
        }
    }

    void Start()
    {
        Button button = GetComponent<Button>();
        button.name += "(" + _towerType + ")";
        button.onClick.AddListener(() =>
        {
            SetSelected();
            AudioManager.Instance.PlaySE(SEAudioType.BUTTON_CLICK);
        });
    }
    
    /// <summary>
    /// インスタンスを生成する
    /// </summary>
    /// <returns>生成されたインスタンス</returns>
    public static PlaceTowerButton CreateEntity()
    {
        return CreateEntity<PlaceTowerButton>(GameEntityPrefabType.PLACE_TOWER_BUTTON);
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="towerMaster">タワーの情報</param>
    public void Initializa(TowerMaster towerMaster)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = towerMaster.moneyCost + "G : " + towerMaster.towerName;
        _towerType = towerMaster.type;
    }

    /// <summary>
    /// 選択状態に設定する
    /// </summary>
    public void SetSelected()
    {
        // 既に選択状態になっているボタンは未選択状態に設定する
        if(_selectedButton != null)
        {
            Button disableButton = _selectedButton.GetComponent<Button>();
            disableButton.interactable = true;
        }
        // 自身を選択状態に設定する
        Button enableButton = GetComponent<Button>();
        enableButton.interactable = false;
        _selectedButton = this;
    }
}
