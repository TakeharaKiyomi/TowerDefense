using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClearScene : MonoBehaviour
{
    [SerializeField]
    private Button titleButton;

    void Awake()
    {
        // タイトル画面押下でタイトル画面に進む
        titleButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySE(SEAudioType.BUTTON_CLICK);
            SceneManager.LoadScene(SceneType.TITLE.GetSceneName());
        });
    }

    void Start()
    {
        AudioManager.Instance.PlayBGM(BGMAudioType.GAME_CLEAR);
    }
}
