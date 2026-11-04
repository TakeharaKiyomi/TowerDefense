using System.Collections.Generic;
using UnityEngine;

public enum BGMAudioType
{
    NONE = 0,
    TITLE,
    GAME,
    GAME_CLEAR,
    GAME_OVER,
    MAX_NUM
}

public enum SEAudioType
{
    NONE = 0,
    BUTTON_CLICK,
    ENEMY_DAMAGE,
    BASE_DAMAGE,
    TOWER_INSTALLATION,
    MAX_NUM
}

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource bgm;

    [SerializeField]
    private AudioSource se;

    private Dictionary<BGMAudioType, AudioClip> bgmAudioSources = new();
    private Dictionary<SEAudioType, AudioClip> seAudioSources = new();

    // シングルトン
    // FIXME: タイトル画面を経由しないとnullで落ちるので、無くても動作するように修正する
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Debug.Log("既にAudioManagerは生成されているので破棄します。");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // シーンを跨いでも自身を保持する
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBGM(BGMAudioType type)
    {
        string audioClipString;
        AudioClip clip;
        switch (type)
        {
            case BGMAudioType.TITLE:
                bgm.loop = true;
                audioClipString = "Audio/BGM/Title";
                break;
            case BGMAudioType.GAME:
                bgm.loop = true;
                audioClipString = "Audio/BGM/Game";
                break;
            case BGMAudioType.GAME_CLEAR:
                bgm.loop = false;
                audioClipString = "Audio/BGM/GameClear";
                break;
            case BGMAudioType.GAME_OVER:
                bgm.loop = false;
                audioClipString = "Audio/BGM/GameOver";
                break;
            default:
                return;
        }
        if(bgmAudioSources.ContainsKey(type))
        {
            clip = bgmAudioSources[type];
        } else
        {
            clip = (AudioClip)Resources.Load(audioClipString);
            if(clip == null)
            {
                Debug.Log("指定のBGMが存在しない。");
                return;
            }
            // プレハブデータ保存
            bgmAudioSources.Add(type, clip);
        }
        bgm.clip = clip;
        bgm.Play();
    }

    public void PlaySE(SEAudioType type)
    {
        string audioClipString;
        AudioClip clip;
        switch (type)
        {
            case SEAudioType.BUTTON_CLICK:
                audioClipString = "Audio/SE/ButtonClick";
                break;
            case SEAudioType.ENEMY_DAMAGE:
                audioClipString = "Audio/SE/EnemyDamage";
                break;
            case SEAudioType.BASE_DAMAGE:
                audioClipString = "Audio/SE/BaseDamage";
                break;
            case SEAudioType.TOWER_INSTALLATION:
                audioClipString = "Audio/SE/TowerInstallation";
                break;
            default:
                return;
        }
        if(seAudioSources.ContainsKey(type))
        {
            clip = seAudioSources[type];
        } else
        {
            clip = (AudioClip)Resources.Load(audioClipString);
            if(clip == null)
            {
                Debug.Log("指定のSEが存在しない。");
                return;
            }
            // プレハブデータ保存
            seAudioSources.Add(type, clip);
        }
        se.PlayOneShot(clip);
    }
}
