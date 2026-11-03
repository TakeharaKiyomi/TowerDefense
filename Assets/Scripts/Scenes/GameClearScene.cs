using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClearScene : MonoBehaviour
{
    [SerializeField]
    private Button titleButton;

    void Awake()
    {
        titleButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySE(SEAudioType.BUTTON_CLICK);
            SceneManager.LoadScene(SceneNames.title);
        });
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlayBGM(BGMAudioType.GAME_CLEAR);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
