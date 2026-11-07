using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    // スタートボタン
    [SerializeField]
    private Button startButton;

    void Awake()
    {
        // スタートボタン押下でゲーム画面に進む
        startButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySE(SEAudioType.BUTTON_CLICK);
            SceneManager.LoadScene(SceneType.GAME.GetSceneName());
        });
    }

    void Start()
    {
        AudioManager.Instance.PlayBGM(BGMAudioType.TITLE);
    }
}
