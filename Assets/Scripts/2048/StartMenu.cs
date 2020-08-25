using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : View {

    public void OnQuitGame() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

}