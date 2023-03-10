using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MedalsManager : MonoBehaviour {

    [Header("Texts")]
    public TMP_Text totalMedalsText;
    public TMP_Text lockedText;

    [Header("Locked Buttons")]
    public GameObject lockedA2;
    public GameObject lockedA3;
    public GameObject lockedA4;
    public GameObject lockedA5;

    [Header("")]
    public List<TMP_Text> mapMedalsList;

    int totalMedals = 0;

    public void LoadMedalData() {
        lockedA2.SetActive(true);
        lockedA3.SetActive(true);
        lockedA4.SetActive(true);
        lockedA5.SetActive(true);

        totalMedals = 0;

        for (int i = 0; i < mapMedalsList.Count; i++) {
            int mapMedals = PlayerPrefs.GetInt($"A{i+1}_medals");
            mapMedalsList[i].text = $"{mapMedals}/3";
            if (mapMedals == 3) {
                mapMedalsList[i].color = Color.green;
            }
            else {
                mapMedalsList[i].color = Color.white;
            }
            totalMedals += mapMedals;
        }

        totalMedalsText.text = $"{totalMedals}/{mapMedalsList.Count * 3}";

        if (totalMedals >= 1) {
            lockedA2.SetActive(false);
        }
        if (totalMedals >= 3) {
            lockedA3.SetActive(false);
        }
        if (totalMedals >= 6) {
            lockedA4.SetActive(false);
        }
        if (totalMedals >= 10) {
            lockedA5.SetActive(false);
        }
    }

    public void LockedText(string text) {
        lockedText.text = text;
    }
}
