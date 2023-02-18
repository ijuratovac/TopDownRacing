using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

	public Slider slider;
	public GameObject loadingScene;

	public void LoadLevel(string sceneName) {
		StartCoroutine(LoadAsynchronously(sceneName));
	}

	IEnumerator LoadAsynchronously(string sceneName) {
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

		loadingScene.SetActive(true);

		while (!operation.isDone) {
			float progress = Mathf.Clamp01(operation.progress / 0.9f);

			slider.value = progress;

			yield return null;
		}
	}	

    public void DeleteAllRecords() {
        for (int i = 1; i <= 5; i++) {
            PlayerPrefs.DeleteKey($"A{i}");
            PlayerPrefs.DeleteKey($"A{i}_medals");
            PlayerPrefs.DeleteKey($"A{i}_ghost");
        }
    }

    public void Quit() {
		Application.Quit();
	}
}
