using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // シングルトン（シーンを跨いで唯一無二）
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            // 既にインスタンス生成されている場合、そのまま返却する
            if(_instance != null) return _instance;
            // 保持していないだけで既にインスタンス生成されていた場合、保持してそのまま返却する
            _instance = FindAnyObjectByType<AudioManager>();
            if(_instance != null) return _instance;
            // インスタンスが未生成の場合、初期化して返却する
            return new GameObject("AudioManager").AddComponent<AudioManager>();
        }
    }

    private AudioSource _bgm;

    private AudioSource _se;

    private readonly Dictionary<BGMAudioType, AudioClip> _bgmAudioSources = new();
    private readonly Dictionary<SEAudioType, AudioClip> _seAudioSources = new();

    private void Awake()
    {
        // 既にインスタンス生成されている場合は、自身を破棄する
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // 新規で生成されたインスタンスなので保持する
        _instance = this;
        // シーンを跨いでも自身が破棄されないようにする
        DontDestroyOnLoad(gameObject);
        // BGM
        GameObject bgmGameObject = new("BGM");
        _bgm = bgmGameObject.AddComponent<AudioSource>();
        bgmGameObject.transform.SetParent(gameObject.transform);
        // SE
        GameObject seGameObject = new("SE");
        _se = seGameObject.AddComponent<AudioSource>();
        seGameObject.transform.SetParent(gameObject.transform);
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="type">BGMの種別</param>
    public void PlayBGM(BGMAudioType type)
    {
        AudioClip clip;
        if(_bgmAudioSources.ContainsKey(type))
        {
            clip = _bgmAudioSources[type];
        }
        else
        {
            clip = (AudioClip)Resources.Load(type.GetResourcesString());
            if(clip == null)
            {
                Debug.Log("指定のBGMが存在しない。");
                return;
            }
            // プレハブデータ保存
            _bgmAudioSources.Add(type, clip);
        }
        _bgm.clip = clip;
        _bgm.Play();
    }

    /// <summary>
    /// SEを再生する
    /// </summary>
    /// <param name="type">SEの種別</param>
    public void PlaySE(SEAudioType type)
    {
        AudioClip clip;
        if(_seAudioSources.ContainsKey(type))
        {
            clip = _seAudioSources[type];
        }
        else
        {
            clip = (AudioClip)Resources.Load(type.GetResourcesString());
            if(clip == null)
            {
                Debug.Log("指定のSEが存在しない。");
                return;
            }
            // プレハブデータ保存
            _seAudioSources.Add(type, clip);
        }
        _se.PlayOneShot(clip);
    }
}
