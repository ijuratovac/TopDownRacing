using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

	CarController carController;

	public List<GameObject> checkpoints;
	int totalCheckpoints;
	bool finished = false;
	bool finishNotTriggered = false;

	private void Awake() {
        totalCheckpoints = checkpoints.Count;
    }

	private void Start() {
		carController = GetComponent<CarController>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Checkpoint")) {
			// Remove the checkpoint from the list if it exists
			if (checkpoints.Contains(collision.gameObject)) {
				checkpoints.Remove(collision.gameObject);
			}
		}
		else if (collision.CompareTag("Finish")) {
			// If all checkpoints are collected, trigger the finish
			if (checkpoints.Count == 0 && finished == false) {
				finished = true;
				carController.DisableControls();
			}
			else if (carController.ControlsAreEnabled()) {
				StartCoroutine(FinishNotTriggered());
			}
		}
	}

	public IEnumerator FinishNotTriggered() {
		finishNotTriggered = true;
		yield return new WaitForSeconds(2);
		finishNotTriggered = false;
    }

	public bool IsFinishNotTriggered() {
		return finishNotTriggered;
	}

	public int GetTotalCheckpoints() {
		return totalCheckpoints;
	}

	public int GetCollectedCheckpoints() {
		return totalCheckpoints - checkpoints.Count;
	}
}
