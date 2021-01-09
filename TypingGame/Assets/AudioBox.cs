using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBox : MonoBehaviour
{
    public float musicValue, effectValue, backgroundMusicTime;

    public AudioClip[] BackgroundMusic, battleThemes;
    AudioSource AS;
    public AudioSource Music;

    private void Start()
    {
        AS = GetComponent<AudioSource>();
        Music.volume = musicValue;
    }

    public void PlayBackgroundMusic(int i)
    {
        Music.time = backgroundMusicTime;
        Music.clip = BackgroundMusic[i];
        Music.Play();
    }

    public void PlayBattleMusic(int i)
    {
        backgroundMusicTime = Music.time;
        Music.time = 0;
        Music.clip =battleThemes[i];
        Music.Play();
    }

    public void PlayRandomBattleTheme()
    {
        //.PlayOneShot(battleThemes[Random.Range(0, battleThemes.Length)], musicValue);
        Music.clip = battleThemes[Random.Range(0, battleThemes.Length)];
        
        Music.Play();
        
    }

    public void PlaySound(AudioClip clip, float value)
    {
        AS.PlayOneShot(clip, value * effectValue);
    }

    public void EndMusic()
    {
        AS.Stop();
    }
}
