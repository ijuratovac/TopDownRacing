using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MedalsManager : MonoBehaviour {

    public TMP_Text totalMedals;

    public List<TMP_Text> mapMedalsList;

    public void LoadMedalData() {
        int total = 0;
        for (int i = 0; i < mapMedalsList.Count; i++) {
            int mapMedals = PlayerPrefs.GetInt($"A{i+1}_medals");
            mapMedalsList[i].text = $"{mapMedals}/3";
            if (mapMedals == 3) {
                mapMedalsList[i].color = Color.green;
            }
            else {
                mapMedalsList[i].color = Color.white;
            }
            total += mapMedals;
        }
        totalMedals.text = $"{total}/9";
    }
}
