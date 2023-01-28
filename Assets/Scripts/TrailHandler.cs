using UnityEngine;

public class TrailHandler : MonoBehaviour {

    CarController carController;
    TrailRenderer[] trails;

    void Awake() {
        carController = GetComponentInParent<CarController>();
        trails = GetComponentsInChildren<TrailRenderer>();
    }

    void Start() {
        foreach (TrailRenderer trailRenderer in trails) {
            trailRenderer.emitting = false;
        }
    }

    void Update() {
        // If the car tires are screeching then show skidmarks based on the surface
        if (carController.IsTireScreeching()) {
            if (carController.surface == "Asphalt") {
                trails[0].emitting = true;
                trails[1].emitting = false;
                trails[2].emitting = false;
                trails[3].emitting = false;
            }
            else if (carController.surface == "Dirt") {
                trails[0].emitting = false;
                trails[1].emitting = true;
                trails[2].emitting = false;
                trails[3].emitting = false;
            }
            else if (carController.surface == "Sand") {
                trails[0].emitting = false;
                trails[1].emitting = false;
                trails[2].emitting = true;
                trails[3].emitting = false;
            }
            else if (carController.surface == "Grass") {
                trails[0].emitting = false;
                trails[1].emitting = false;
                trails[2].emitting = false;
                trails[3].emitting = true;
            }
        }
        else {
            foreach (TrailRenderer trailRenderer in trails) {
                trailRenderer.emitting = false;
            }
        }
    }
}
