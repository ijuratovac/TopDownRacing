using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour {

    TMP_Text textMesh;

    public CarController carController;

    // Start is called before the first frame update
    void Start() {
        textMesh = GetComponent<TMP_Text>();
        textMesh.text = "3";
    }

    // Update is called once per frame
    void Update() {
        if (Time.timeSinceLevelLoad < 0.7f) {
            textMesh.text = "3";
        }
        else if (Time.timeSinceLevelLoad < 1.4f) {
            textMesh.text = "2";
        }
        else if (Time.timeSinceLevelLoad < 2.1f) {
            textMesh.text = "1";
        }
        else if (Time.timeSinceLevelLoad < 2.8f) {
            textMesh.text = "GO!";
            if (!carController.ControlsAreEnabled()) {
                carController.EnableControls();
            }
        }
        else if (Time.timeSinceLevelLoad < 2.9f && textMesh.isActiveAndEnabled) {
            textMesh.enabled = false;
        }
        else if (!carController.ControlsAreEnabled() && !textMesh.isActiveAndEnabled) {
            textMesh.enabled = true;
            textMesh.text = carController.GetRunTime().ToString("#.##");
        }
    }
}
