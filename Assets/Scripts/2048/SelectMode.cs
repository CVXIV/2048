using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMode : View {
    public void OnSelectMode(int index) {
        PlayerPrefs.SetInt(ConstVariable.GameMode, index);
        SceneManager.LoadSceneAsync(ConstVariable.GameScene);
    }

    public override void Show() {
        base.Show();
    }

    public override void Hide() {
        base.Hide();
    }
}