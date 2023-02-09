using System.Collections;
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
	public TMP_Text checkpointsCollected;
	public TMP_Text checkpointsTotal;

	public CarController carController;

	public TrackRecord trackRecord;

	public CheckpointManager checkpointManager;

	public bool isTutorial;

	void Start() {
        
        newRecord.enabled = false;
		difference.enabled = false;
		countdown.text = "3";
		checkpointsTotal.text = $"/{checkpointManager.GetTotalCheckpoints()}";
        SetBestTime();
    }

	void FixedUpdate() {
        SetTimer();
		SetSpeed();
		SetCountdown();
		SetCheckpoints();
	}

	void SetBestTime() {
		if (trackRecord.NoTimeSet()) {
			bestTime.text = "(no time set)";
		}
		else {
			bestTime.text = FormatTime(trackRecord.GetRecord());
		}
	}

	void SetTimer() {
		if (carController.ControlsAreEnabled()) {
			timer.text = FormatTime(trackRecord.GetCurrentTime());
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
            if (trackRecord.NoTimeSet()) {
				trackRecord.SetNewRecord();
                newRecord.enabled = true;
            }
			else {
				// Show time difference between the new time and track record
				float timeDifference = trackRecord.GetTimeDifference();
                if (timeDifference > 0) {
                    difference.color = Color.red;
                    difference.text = $"+{FormatTime(Mathf.Abs(timeDifference))}";
                }
                else {
                    difference.color = Color.green;
                    difference.text = $"-{FormatTime(Mathf.Abs(timeDifference))}";
                    trackRecord.SetNewRecord();
                    newRecord.enabled = true;
                }

                difference.enabled = true;
            }

            timer.enabled = false;
			speed.enabled = false;
            countdown.enabled = true;
            if (isTutorial) {
				countdown.text = "";
            }
			else {
                countdown.text = FormatTime(trackRecord.GetCurrentTime());
            }
        }
	}

    void SetCheckpoints() {
        int cpCount = checkpointManager.GetCollectedCheckpoints();

        if (checkpointManager.IsFinishNotTriggered()) {
            checkpointsCollected.color = Color.red;
            checkpointsTotal.color = Color.red;
        }
		else if (checkpointsTotal.color == Color.red) {
            checkpointsCollected.color = Color.white;
            checkpointsTotal.color = Color.white;
        }
		else if (cpCount.ToString() != checkpointsCollected.text) {
            checkpointsCollected.text = cpCount.ToString();

            if ($"/{checkpointsCollected.text}" == checkpointsTotal.text) {
                checkpointsCollected.color = Color.green;
                checkpointsTotal.color = Color.green;
            }
        }
    }

    string FormatTime(float time) {
		float minutes = Mathf.Floor(time / 60);
		float seconds = Mathf.Floor(time % 60);
		float decimals = Mathf.Round((time - Mathf.Floor(time)) * 100) % 100;
		return $"{minutes}:{seconds:00}.{decimals:00}";
	}
}
