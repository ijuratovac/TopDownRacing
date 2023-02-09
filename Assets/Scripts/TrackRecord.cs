using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackRecord : MonoBehaviour {

    CarController carController;

    GhostCarRecorder ghostRecorder;

    string map;
    float time;

    void Awake() {
        map = SceneManager.GetActiveScene().name;
    }

    void Start() {
        //PlayerPrefs.DeleteKey(map); // Delete the record on the map
        //PlayerPrefs.DeleteKey($"{map}_ghost"); // Delete the ghost on the map
        carController = GetComponent<CarController>();
        ghostRecorder = GetComponent<GhostCarRecorder>();
    }

    void FixedUpdate() {
        time = carController.GetRunTime();
    }

    public bool NoTimeSet() {
        if (PlayerPrefs.GetFloat(map, 0) == 0) {
            return true;
        }
        return false;
    }

    public void SetNewRecord() {
        PlayerPrefs.SetFloat(map, time);
        StartCoroutine(ghostRecorder.SaveCarPositionCO());
    }

    public float GetRecord() {
        return PlayerPrefs.GetFloat(map);
    }

    public float GetCurrentTime() {
        return time;
    }

    public float GetTimeDifference() {
        return time - PlayerPrefs.GetFloat(map);
    }
}
