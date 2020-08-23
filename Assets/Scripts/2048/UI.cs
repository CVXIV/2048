using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public GameObject quitCanvas;
    public GameObject startCanvas;

    private void Start() {
        quitCanvas.SetActive(false);
    }

    public void ExitPress() {
        quitCanvas.SetActive(true);
        startCanvas.SetActive(false);
    }

    public void NoPress() {
        quitCanvas.SetActive(false);
        startCanvas.SetActive(true);
    }


    public void YesPress() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void StartPress() {
        gameObject.SetActive(false);
        quitCanvas.SetActive(false);
    }
}