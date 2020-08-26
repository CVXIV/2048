using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : View {

    public AudioClip volumeClip;

    /// <summary>
    /// 播放音乐要放在Start而不是Awake；否则会因为执行太早导致AudioManager没有初始化
    /// </summary>
    private void Start() {
        AudioManager.__instance.PlayBgMusic(volumeClip);
    }

    public void OnQuitGame() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

}