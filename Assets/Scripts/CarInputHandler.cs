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

        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");

        carController.SetInputVector(inputVector);
    }
}
