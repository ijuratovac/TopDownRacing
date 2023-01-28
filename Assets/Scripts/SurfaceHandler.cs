using UnityEngine;
using static CarController;

public class SurfaceHandler : MonoBehaviour {

    string surface;

    void Start() {
        surface = "Grass";
    }

    private void OnTriggerStay2D(Collider2D collision) {
        switch (collision.tag) {
            case "Asphalt":
            case "Dirt":
            case "Sand":
            case "Grass":
                surface = collision.tag;
                break;
        }
    }

    public string GetSurface() {
        return surface;
    }
}
