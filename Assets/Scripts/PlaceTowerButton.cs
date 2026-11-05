using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlaceTowerButton : MonoBehaviour
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
    private TowerEntityType _towerType;
    public TowerEntityType TowerType
    {
        get
        {
            return _towerType;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => onClickButton());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void onClickButton()
    {
        // 既に選択中の場合は何もしない
        if(_selectedButton == this) return;
        SetSelected(true, this);
        SetSelected(false, _selectedButton);
        _selectedButton = this;
        AudioManager.Instance.PlaySE(SEAudioType.BUTTON_CLICK);
    }

    private void SetSelected(bool isSelected, PlaceTowerButton placeTowerButton)
    {
        if(placeTowerButton == null) return;
        Button button = placeTowerButton.GetComponent<Button>();
        button.interactable = !isSelected;
    }

    public void SetTextAndType(TowerMaster towerMaster)
    {
        TextMeshProUGUI textMP = GetComponentInChildren<TextMeshProUGUI>();
        textMP.text = towerMaster.moneyCost + "G : " + towerMaster.towerName;
        _towerType = towerMaster.type;
    }
}
