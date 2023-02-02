using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour {

	public TMP_Text countdown;
	public TMP_Text timer;

	public CarController carController;

	void Start() {
		countdown.text = "3";
	}

	void Update() {
		if (Time.timeSinceLevelLoad < 0.7f) {
			countdown.text = "3";
		}
		else if (Time.timeSinceLevelLoad < 1.4f) {
			countdown.text = "2";
		}
		else if (Time.timeSinceLevelLoad < 2.1f) {
			countdown.text = "1";
		}
		else if (Time.timeSinceLevelLoad < 2.8f) {
			countdown.text = "GO!";
			if (!carController.ControlsAreEnabled()) {
				carController.EnableControls();
			}
		}
		else if (Time.timeSinceLevelLoad < 2.9f && countdown.isActiveAndEnabled) {
			countdown.enabled = false;
		}
		else if (!carController.ControlsAreEnabled() && !countdown.isActiveAndEnabled) {
			timer.enabled = false;
			countdown.enabled = true;

            countdown.text = timer.text;
		}

        float time = carController.GetRunTime();
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.Floor(time % 60);
        float decimals = Mathf.Round((time - Mathf.Floor(time)) * 100) % 100;

        timer.text = $"{minutes}:{seconds:00}.{decimals:00}";
    }
}
