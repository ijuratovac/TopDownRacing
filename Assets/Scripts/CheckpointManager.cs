using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

	CarController carController;

	public List<GameObject> checkpoints;
	int totalCheckpoints;
	bool finished = false;

	private void Start() {
		carController = GetComponent<CarController>();
		totalCheckpoints = checkpoints.Count;
		Debug.Log($"Checkpoints left: {totalCheckpoints}");
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Checkpoint")) {
			// Remove the checkpoint from the list if it exists
			if (checkpoints.Contains(collision.gameObject)) {
				checkpoints.Remove(collision.gameObject);
				Debug.Log($"Checkpoints left: {checkpoints.Count}");
			}
		}
		else if (collision.CompareTag("Finish")) {
			// If all checkpoints are collected, trigger the finish
			if (checkpoints.Count == 0 && finished == false) {
				finished = true;
				Debug.Log("Finished!");
				carController.DisableControls();
			}
		}
	}
}
