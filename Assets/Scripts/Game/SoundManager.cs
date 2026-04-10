using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : SingletonScene<SoundManager>
{
    private AudioSource _audioSource;
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private List<GameSound> _gameSounds;
    public Dictionary<SoundType, AudioClip> AudioClips { get; private set; }
    
    
    protected override void Init()
    {
        base.Init();
        _audioSource = GetComponent<AudioSource>();

        AudioClips = new();
        foreach (var item in _gameSounds)
        {
            AudioClips.Add(item.Type, item.Clip);
        }
    }
    
    public void Initialize()
    {
        ActivateSound(GameSave.GetSettings().SFX);
    }
    
    public void ActivateSound(bool value)
    {
        _audioMixer.SetFloat("SFX", value ? 0 : -80);
    }
    
    

    public void Play(SoundType clipType, float volume = 1)
    {
        if(AudioClips.TryGetValue(clipType, out var clip))
            _audioSource.PlayOneShot(clip, volume);
    }
}

[Serializable]
public struct GameSound
{
    public SoundType Type;
    public AudioClip Clip;
}

public enum SoundType
{
    Jump,
    Combo,
    Platform_Break,
    Cloud_Whoosh,
    Win1,
    Win2,
    Open_Chests_Panel,
    Open_Mini_Chest,
    Open_Big_Chest,
    RankedUp_Lower,
    RankedUp_Upr,
    RankedUp_Particle,
    Key_Take
}