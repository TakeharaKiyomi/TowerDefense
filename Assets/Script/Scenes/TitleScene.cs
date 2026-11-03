using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

// FIXME: 追々正しい場所に移動させたい
public static class SceneNames
{
    public const string title = "Title";
    public const string game = "Game";
    public const string gameClear = "GameClear";
    public const string gameOver = "GameOver";
}

public class TitleScene : MonoBehaviour
{
    [SerializeField]
    private Button startButton;

    void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySE(SEAudioType.BUTTON_CLICK);
            SceneManager.LoadScene(SceneNames.game);
        });
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlayBGM(BGMAudioType.TITLE);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
