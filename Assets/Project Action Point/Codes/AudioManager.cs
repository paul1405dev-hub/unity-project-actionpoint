using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("BGM")]
    public AudioClip bgmClip;
    AudioSource bgmPlayer;
    public float bgmVolume;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    AudioSource[] sfxPlayers;
    public float sfxVolume;
    public int channels;
    int channelIndex;


    public enum Sfx { Craft, Health, Hit, Dead, Win, Lose, Climb };

    void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;
        bgmPlayer.playOnAwake = false;
        bgmPlayer.clip = bgmClip;
        bgmPlayer.volume = bgmVolume;

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }

    }

    private void LateUpdate()
    {
        // 볼륨을 실시간 반영 (설정 메뉴에서 슬라이더 등으로 조절 가능하게)
        if (bgmPlayer != null)
            bgmPlayer.volume = bgmVolume;

        foreach (AudioSource audioSource in sfxPlayers)
        {
            if (audioSource != null)
                audioSource.volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (!isPlay)
        {
            bgmPlayer.Stop();
        }
        else
        {
            bgmPlayer.Play();
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }

    }
}