using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

	public TMP_Text countdown;
	public TMP_Text timer;
	public TMP_Text speed;
	public TMP_Text newRecord;
	public TMP_Text bestTime;
	public TMP_Text difference;

	public CarController carController;

	string map;

	float time;

	void Start() {
		map = SceneManager.GetActiveScene().name;
        //PlayerPrefs.DeleteKey(map); // Delete the record on the map
        newRecord.enabled = false;
		difference.enabled = false;
		countdown.text = "3";
	}

	void FixedUpdate() {
		SetBestTime();
		SetTimer();
		SetSpeed();
		SetCountdown();
	}

	void SetBestTime() {
		float storedBestTime = PlayerPrefs.GetFloat(map, 0f);
		if (storedBestTime == 0f) {
			bestTime.text = "(no time set)";
		}
		else {
			bestTime.text = FormatTime(storedBestTime);
		}
	}

	void SetTimer() {
		if (carController.ControlsAreEnabled()) {
			time = carController.GetRunTime();
			timer.text = FormatTime(time);
		}
	}

	void SetSpeed() {
		int carSpeed = (int) Mathf.Floor(carController.GetCarSpeed() * 20);
		speed.text = $"{carSpeed}";
	}

	void SetCountdown() {
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
			bool noTimeSet = false;
			if (PlayerPrefs.GetFloat(map, 0) == 0) {
				noTimeSet = true;
            }

			if (noTimeSet) {
				SetNewRecord();
            }
			else {
                // Show time difference between the new time and track record
                float timeDifference = float.Parse((time - PlayerPrefs.GetFloat(map)).ToString("0.00"));
                if (timeDifference > 0) {
                    difference.color = Color.red;
                    difference.text = $"+{FormatTime(Mathf.Abs(timeDifference))}";
                }
                else {
                    difference.color = Color.green;
                    difference.text = $"-{FormatTime(Mathf.Abs(timeDifference))}";
                    SetNewRecord();
                }

                difference.enabled = true;
            }

            timer.enabled = false;
			speed.enabled = false;
			countdown.enabled = true;

			countdown.text = timer.text;
		}
	}

	void SetNewRecord() {
        PlayerPrefs.SetFloat(map, time);
        newRecord.enabled = true;
    }

	string FormatTime(float time) {
		float minutes = Mathf.Floor(time / 60);
		float seconds = Mathf.Floor(time % 60);
		float decimals = Mathf.Round((time - Mathf.Floor(time)) * 100) % 100;
		return $"{minutes}:{seconds:00}.{decimals:00}";
	}
}
