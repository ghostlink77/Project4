using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using UnityEngine.UIElements;
public enum AudioType
{
    BGM,
    UISFX,
    SFX,
}

public class AudioManager : SingletonBehaviour<AudioManager>
{
    private AudioSource[] _audioSources;
    private Dictionary<string, AudioClip> _clips = new();

    [Header("AudioSource pool")]
    [SerializeField] private GameObject _audioSourcePrefab;
    private IObjectPool<AudioSource> _audioSourcePool;
    [SerializeField] private int _initSize = 10;
    [SerializeField] private int _maxSize = 50;
    [SerializeField] Transform _audioSourceParent;

    [SerializeField] private AudioMixer _audioMixer; 

    protected override void Init()
    {
        base.Init();

        string[] soundTypeNames = Enum.GetNames(typeof(AudioType));
        _audioSources = new AudioSource[soundTypeNames.Length];
        for (int i = 0; i < soundTypeNames.Length; ++i)
        {
            GameObject go = new GameObject(soundTypeNames[i]);
            go.transform.parent = transform;
            _audioSources[i] = go.AddComponent<AudioSource>();
            AudioMixerGroup[] _audioMixerGroup = _audioMixer.FindMatchingGroups(soundTypeNames[i]);
            if ( _audioMixerGroup != null ) _audioSources[i].outputAudioMixerGroup = _audioMixerGroup[0];
        }

        AudioSource bgm = _audioSources[(int)AudioType.BGM];
        bgm.loop = true;

        CreatePools();
    }

    #region AudioSource Pool 
    private void CreatePools()
    {
        AudioSource audioSourceObj = _audioSourcePrefab.GetComponent<AudioSource>();
        var pool = new ObjectPool<AudioSource>(
            createFunc: () => Instantiate(audioSourceObj),
            actionOnGet: ActivateAudioSource,
            actionOnRelease: DisableAudioSource,
            collectionCheck: false,
            defaultCapacity: _initSize,
            maxSize: _maxSize);
        _audioSourcePool = pool;
    }

    private void ActivateAudioSource(AudioSource obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void DisableAudioSource(AudioSource obj)
    {
        obj.gameObject.SetActive(false);
    }

    public void RemoveAudioSource(AudioSource obj)
    {
        _audioSourcePool.Release(obj);
    }
    #endregion

    public void Play(AudioType audioType, string fileName)
    {
        AudioClip clip = GetClip(fileName);
        AudioSource audioSource = _audioSources[(int)audioType];
        switch (audioType)
        {
            case AudioType.BGM:
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                audioSource.clip = clip;
                audioSource.Play();
                break;
            case AudioType.UISFX:
                audioSource.PlayOneShot(clip);
                break;
            case AudioType.SFX:
                audioSource.PlayOneShot(clip);
                break;
            default:
                Debug.Log($"Fail to Play {audioType}.");
                break;
        }

    }

    public void PlayAtPoint(string fileName, Vector3 position)
    {
        AudioClip clip = GetClip(fileName);

        if (_audioSourcePool == null)
        {
            Debug.Log("AudioSource Pools is nothing.");
            return;
        }

        var audioSource = _audioSourcePool.Get();
        audioSource.transform.SetParent(gameObject.transform, false);
        audioSource.transform.parent = _audioSourceParent;
        audioSource.clip = clip;
        audioSource.Play();

        StartCoroutine(PlayAudioSourceAtPoint(clip.length, audioSource));
    }

    private IEnumerator PlayAudioSourceAtPoint(float playTime, AudioSource audioSource)
    {
        yield return new WaitForSeconds(playTime);

        _audioSourcePool.Release(audioSource);
    }

    public void SetPitch(AudioType audioType, float pitch) =>
        _audioSources[(int)audioType].pitch = pitch;

    public void SetVolume(AudioType audioType, float volume)
    {
        _audioSources[(int)audioType].volume = volume;
    }

    public void SetVolumeMixer(float volume, string groupName)
    {
        if (volume < 0.0001f) _audioMixer.SetFloat(groupName, -80f);
        else _audioMixer.SetFloat(groupName, Mathf.Log10(volume) * 20);
    }
        

    public void Pause(AudioType audioType) =>
        _audioSources[(int)audioType].Pause();
        //AudioListener.pause = true;

    public void Stop(AudioType audioType) =>
        _audioSources[(int)audioType].Stop();

    public void Resome(AudioType audioType) =>
        _audioSources[(int)audioType].UnPause();
        //AudioListener.pause = false;

    public void StopAll()
    {
        foreach (var source in _audioSources)
        {
            source.Stop();
        }
        
        foreach(var source in _audioSourceParent.GetComponentsInChildren<AudioSource>())
        {
            source.Stop();
        }
    }

    public void Mute()
    {
        _audioMixer.SetFloat("Master", -80f);
    }

    public void UnMute()
    {
        _audioMixer.SetFloat("Master", 0f);
    }

    public void SyncUserSettings()
    {

    }

    public const string AUDIO_PATH = "Audio";
    public AudioClip GetClip(string fileName)
    {
        if (_clips.TryGetValue(fileName, out var clip))
        {
            return clip;
        }

        _clips[fileName] =
            Resources.Load<AudioClip>($"{AUDIO_PATH}/{fileName}");
        return _clips[fileName];
    }

}
