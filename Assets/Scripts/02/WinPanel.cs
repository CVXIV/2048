using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : View {

    public GamePanel gamePanel;

    public override void Show() {
        base.Show();
    }

    public override void Hide() {
        base.Hide();
    }

    public void OnRestartClick() {
        this.Hide();
        gamePanel.GamePanelInit();
    }

    public void OnQuitClick() {
        SceneManager.LoadSceneAsync(ConstVariable.StartScene);
    }
}