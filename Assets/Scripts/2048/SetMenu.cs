using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMenu : View {

    private Slider sound;
    private Slider volume;

    private void Awake() {
        Slider[] sliders = this.GetComponentsInChildren<Slider>();
        foreach(var slider in sliders) {
            if (slider.name == ConstVariable.Sound) {
                sound = slider;
                sound.onValueChanged.AddListener(OnSoundChange);
            } else {
                volume = slider;
                volume.onValueChanged.AddListener(OnVolumeChange);
            }
        }
    }

    public override void Show() {
        base.Show();
        float soundValue = PlayerPrefs.GetFloat(ConstVariable.Sound, 0);
        float volumeValue = PlayerPrefs.GetFloat(ConstVariable.Volume, 0);
        sound.value = soundValue;
        volume.value = volumeValue;
    }

    public override void Hide() {
        base.Hide();
    }

    public void OnSoundChange(float value) {
        PlayerPrefs.SetFloat(ConstVariable.Sound, value);
    }

    public void OnVolumeChange(float value) {
        PlayerPrefs.SetFloat(ConstVariable.Volume, value);
    }
}