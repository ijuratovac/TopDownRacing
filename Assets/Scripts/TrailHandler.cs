using UnityEngine;

public class TrailHandler : MonoBehaviour {

    CarController carController;
    TrailRenderer trailRenderer;

    void Awake() {
        carController = GetComponentInParent<CarController>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void Start() {
        trailRenderer.emitting = false;
    }

    void Update() {
        // If the car tires are screeching then show skidmarks and smoke
        if (carController.IsTireScreeching()) {
            trailRenderer.emitting = true;
        }
        else {
            trailRenderer.emitting = false;
        }
    }
}
