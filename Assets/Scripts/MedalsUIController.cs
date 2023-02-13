using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MedalsUIController : MonoBehaviour {

    CheckpointManager checkpointManager;
    TrackRecord trackRecord;

    public GameObject trackUI;
    public GameObject medalsUI;

    public float goldTime;
    public float silverTime;
    public float bronzeTime;

    public TMP_Text medalText;

    public GameObject bronzeMedal;
    public GameObject silverMedal;
    public GameObject goldMedal;

    string map;
    int medals;

    bool alreadyCalled = false;
    float oldRecord;

    void Awake() {
        map = SceneManager.GetActiveScene().name;
        checkpointManager = GetComponent<CheckpointManager>();
        trackRecord = GetComponent<TrackRecord>();
    }

    void Start() {
        medals = PlayerPrefs.GetInt($"{map}_medals", 0);
        oldRecord = trackRecord.GetRecord();
        if (oldRecord == 0) {
            oldRecord = 9999;
        }
    }

    void FixedUpdate() {
        if (checkpointManager.Finished() && !alreadyCalled) {
            alreadyCalled = true;
            SetMedals();
        }
    }

    void SetMedals() {
        float newRecord = trackRecord.GetCurrentTime();
        if (oldRecord > newRecord && medals < 3) {
            List<string> awarded = CheckAwardedMedals(newRecord);
            if (awarded.Count > 0) {
                HandleText(awarded.Count);
                ShowMedals(awarded);
                SwitchUI();
                AnimateMedals(awarded);
                PlayerPrefs.SetInt($"{map}_medals", medals + awarded.Count);
            }
        }
    }

    List<string> CheckAwardedMedals(float time) {
        List<string> list = new List<string>();
        if (medals == 0) {
            if (time < bronzeTime) {
                list.Add("bronze");
            }
            if (time < silverTime) {
                list.Add("silver");
            }
            if (time < goldTime) {
                list.Add("gold");
            }
        }
        if (medals == 1) {
            if (time < silverTime) {
                list.Add("silver");
            }
            if (time < goldTime) {
                list.Add("gold");
            }
        }
        if (medals == 2) {
            if (time < goldTime) {
                list.Add("gold");
            }
        }
        return list;
    }

    void HandleText(int count) {
        if (count == 1) {
            if (medals == 0) {
                medalText.text = "You earned a star!";
            }
            else {
                medalText.text = "You earned another star!";
            }
        }
        else {
            medalText.text = $"You earned {count} new stars!";
        }
    }

    void ShowMedals(List<string> awarded) {
        int total = medals + awarded.Count;
        if (total == 1) {
            bronzeMedal.SetActive(true);
            silverMedal.SetActive(false);
            goldMedal.SetActive(false);
        }
        if (total == 2) {
            bronzeMedal.SetActive(true);
            silverMedal.SetActive(true);
            goldMedal.SetActive(false);
        }
        if (total == 3) {
            bronzeMedal.SetActive(true);
            silverMedal.SetActive(true);
            goldMedal.SetActive(true);
        }
    }

    void AnimateMedals(List<string> awarded) {
        if (awarded.Contains("bronze")) {
            bronzeMedal.GetComponent<Animator>().SetBool("rotating", true);
        }
        if (awarded.Contains("silver")) {
            silverMedal.GetComponent<Animator>().SetBool("rotating", true);
        }
        if (awarded.Contains("gold")) {
            goldMedal.GetComponent<Animator>().SetBool("rotating", true);
        }
    }

    void SwitchUI() {
        medalsUI.SetActive(true);
        trackUI.SetActive(false);
    }
}
