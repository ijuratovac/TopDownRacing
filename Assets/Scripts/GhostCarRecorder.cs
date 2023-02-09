using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostCarRecorder : MonoBehaviour {
    public Transform carSpriteObject;
    public GameObject ghostCarPlaybackPrefab;

    //Local variables
    GhostCarData ghostCarData = new GhostCarData();

    bool isRecording = true;

    //Other components
    Rigidbody2D carRigidbody2D;
    CarInputHandler carInputHandler;

    private void Awake() {
        carRigidbody2D = GetComponent<Rigidbody2D>();
        carInputHandler = GetComponent<CarInputHandler>();
    }

    // Start is called before the first frame update
    void Start() {
        //Create a ghost car
        GameObject ghostCar = Instantiate(ghostCarPlaybackPrefab);

        //Load the data for the current player
        ghostCar.GetComponent<GhostCarPlayback>().LoadData();
    }

    void FixedUpdate() {
        if (isRecording && carSpriteObject != null) {
            ghostCarData.AddDataItem(new GhostCarDataListItem(
                carRigidbody2D.position,
                carRigidbody2D.rotation,
                carSpriteObject.localScale,
                Time.timeSinceLevelLoad
            ));
        }
    }

    public IEnumerator SaveCarPositionCO() {
        yield return new WaitForSeconds(0.3f);

        SaveData();
    }

    void SaveData() {
        string jsonEncodedData = JsonUtility.ToJson(ghostCarData);

        if (carInputHandler != null) {
            PlayerPrefs.SetString($"{SceneManager.GetActiveScene().name}_ghost", jsonEncodedData);
            PlayerPrefs.Save();
        }

        isRecording = false;
    }

}
