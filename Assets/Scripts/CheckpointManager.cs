using UnityEngine;

public class CheckpointManager : MonoBehaviour {
    int checkpointCount = 0;
    int checkpointsCollected = 0;
    bool finished = false;

    private void Start() {
        // Get number of checkpoints
        checkpointCount = GameObject.FindGameObjectsWithTag("Checkpoint").Length;
        Debug.Log($"Total checkpoints: {checkpointCount}");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Checkpoint")) {
            // Collect the checkpoint if it's active, then deactivate it
            Checkpoint checkpoint = collision.GetComponent<Checkpoint>();
            if (checkpoint.IsActive()) {
                checkpointsCollected++;
                checkpoint.Deactivate();
                Debug.Log($"Collected checkpoints: {checkpointsCollected}");
            }
        }
        else if (collision.CompareTag("Finish")) {
            // If all checkpoints are collected, trigger the finish
            if (checkpointsCollected >= checkpointCount && finished == false) {
                finished = true;
                Debug.Log("Finished!");
                Debug.Log(Time.realtimeSinceStartup);
            }
        }
    }
}
