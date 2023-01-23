using UnityEngine;

public class Checkpoint : MonoBehaviour {
    bool active = true;

    public bool IsActive() {
        return active;
    }

    public void Deactivate() {
        active = false;
    }
}
