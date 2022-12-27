using UnityEngine;

public class CarInputHandler : MonoBehaviour {

    CarController carController;

    void Awake() {
        carController = GetComponent<CarController>();
    }

    void Start() {

    }

    void Update() {
        Vector2 inputVector = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            inputVector.y = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            inputVector.y = -1;
        }
        inputVector.x = -Input.GetAxisRaw("Horizontal");

        carController.SetInputVector(inputVector);
    }
}
