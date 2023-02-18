using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager sharedInstance = null;

	public List<AudioSource> musicList;

    int currentScene = -1;

	void Awake() {
		if (sharedInstance != null && sharedInstance != this) {
			Destroy(gameObject);
		}
		else {
			GameObject.DontDestroyOnLoad(gameObject);
			sharedInstance = this;
		}

		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void Start() {
        LoadMainMenu();
	}

	void Update() {
		if (currentScene >= 2) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				LoadMainMenu();
			}
			else if (Input.GetKeyDown(KeyCode.Delete)) {
				ReloadScene();
			}
		}
	}

	void ReloadScene() {
		SceneManager.LoadScene(currentScene);
    }

    void LoadMainMenu() {
		SceneManager.LoadScene("MainMenu");
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (currentScene != scene.buildIndex) {
            if (scene.name == "MainMenu") {
				SwitchMusic(musicList[0]);
            }
            else if (scene.name == "A1") {
                SwitchMusic(musicList[1]);
            }
            else if (scene.name == "A2") {
                SwitchMusic(musicList[2]);
            }
            else if (scene.name == "A3") {
                SwitchMusic(musicList[3]);
            }
            else if (scene.name == "A4") {
                SwitchMusic(musicList[4]);
            }
            else if (scene.name == "A5") {
                SwitchMusic(musicList[5]);
            }
            currentScene = scene.buildIndex;
        }
    }

	void SwitchMusic(AudioSource musicToPlay) {
        foreach (AudioSource music in musicList) {
            music.Stop();
        }
        musicToPlay.Play();
    }
}
