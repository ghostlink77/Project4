using System.Collections.Generic;
using UnityEngine;
using System;
public enum AudioType
{
    BGM,
    SFX
}

public class AudioManager : SingletonBehaviour<AudioManager>
{
    private AudioSource[] _audioSources;
    private Dictionary<string, AudioClip> _clips = new();

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
        }

        AudioSource bgm = _audioSources[(int)AudioType.BGM];
        bgm.loop = true;
    }

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
            case AudioType.SFX:
                audioSource.PlayOneShot(clip);
                break;
            default:
                Debug.Log($"Fail to Play {audioType}.");
                break;
        }

    }

    public void SetPitch(AudioType audioType, float pitch) =>
        _audioSources[(int)audioType].pitch = pitch;

    public void SetVolume(AudioType audioType, float volume) =>
        _audioSources[(int)audioType].volume = volume;

    public void Pause(AudioType audioType) =>
        _audioSources[(int)audioType].Pause();

    public void Stop(AudioType audioType) =>
        _audioSources[(int)audioType].Stop();

    public void Resome(AudioType audioType) =>
        _audioSources[(int)audioType].UnPause();

    public void StopAll()
    {
        foreach (var source in _audioSources)
        {
            source.Stop();
        }
    }

    public void Mute()
    {
        SetVolume(AudioType.BGM, 0f);
        SetVolume(AudioType.SFX, 0f);
    }

    public void UnMute()
    {
        SetVolume(AudioType.BGM, 1f);
        SetVolume(AudioType.SFX, 1f);
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
