using System.Collections.Generic;
using UnityEngine;

public class SurfaceHandler : MonoBehaviour {

	List<string> surfaces;

	void Start() {
		surfaces = new List<string>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		surfaces.Insert(0, collision.tag);
	}

	private void OnTriggerExit2D(Collider2D collision) {
		surfaces.Remove(collision.tag);
	}

	public string GetSurface() {
		if (surfaces.Contains("Asphalt")) {
			return "Asphalt";
		}
		if (surfaces.Contains("Dirt")) {
			return "Dirt";
		}
		if (surfaces.Contains("Sand")) {
			return "Sand";
		}
		if (surfaces.Contains("Grass")) {
			return "Grass";
		}
		if (surfaces.Count == 0) {
			return "Grass";
		}
		return surfaces[0];
	}
}
