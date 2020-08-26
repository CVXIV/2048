using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager __instance;

    private AudioSource volume, sound;

    private void Awake() {
        __instance = this;

        AudioSource[] result = GetComponentsInChildren<AudioSource>();
        foreach(var audio in result) {
            if (audio.name == ConstVariable.Sound) {
                sound = audio;
            } else {
                volume = audio;
            }
        }
        // 设置音量大小
        volume.volume = PlayerPrefs.GetFloat(ConstVariable.Volume, 0.5f);
        sound.volume = PlayerPrefs.GetFloat(ConstVariable.Sound, 0.5f);
    }

    public void PlayBgMusic(AudioClip clip) {
        volume.clip = clip;
        volume.loop = true;
        volume.Play();
    }

    public void PlaySoundMusic(AudioClip clip) {
        sound.PlayOneShot(clip);
    }

    public void OnVolumeChange(float value) {
        volume.volume = value;
        PlayerPrefs.SetFloat(ConstVariable.Volume, value);
    }

    public void OnSoundChange(float value) {
        sound.volume = value;
        PlayerPrefs.SetFloat(ConstVariable.Sound, value);
    }

}