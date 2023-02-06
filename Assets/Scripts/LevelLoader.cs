using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

	public Slider slider;
	public GameObject loadingScene;

	public void LoadLevel(int sceneIndex) {
		StartCoroutine(LoadAsynchronously(sceneIndex));
	}

	IEnumerator LoadAsynchronously(int sceneIndex) {
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

		loadingScene.SetActive(true);

		while (!operation.isDone) {
			float progress = Mathf.Clamp01(operation.progress / 0.9f);

			slider.value = progress;

			yield return null;
		}
	}

	public void Quit() {
		UnityEditor.EditorApplication.isPlaying = false;
		Application.Quit();
	}
}
