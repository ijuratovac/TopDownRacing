using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

    public AudioMixer mixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public AudioSource clickSFX;

    void Start() {
        SetDefaultValues(1);
        GetVolumeValues();
    }

    void SetDefaultValues(float value) {
        if (PlayerPrefs.GetFloat("MasterVolume", -1) == -1) {
            PlayerPrefs.SetFloat("MasterVolume", value);
            PlayerPrefs.SetFloat("MusicVolume", value);
            PlayerPrefs.SetFloat("SFXVolume", value);
        }
    }

    void GetVolumeValues() {
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume"));
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume"));
    }

    public void SetMasterVolume(float value) {
        mixer.SetFloat("MasterVolume", Mathf.Clamp(Mathf.Log10(value) * 20, -80, 0));
    }

    public void SetMusicVolume(float value) {
        mixer.SetFloat("MusicVolume", Mathf.Clamp(Mathf.Log10(value) * 20, -80, 0));
    }

    public void SetSFXVolume(float value) {
        mixer.SetFloat("SFXVolume", Mathf.Clamp(Mathf.Log10(value) * 20, -80, 0));
    }

    public void SetSliderValues() {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }

    public void SaveChanges() {
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }

    public void ClickSound() {
        clickSFX.Play();
    }
}
