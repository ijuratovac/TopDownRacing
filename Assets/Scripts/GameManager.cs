using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager sharedInstance = null;

	public AudioSource menuMusic;
	public AudioSource A1Music;
    public AudioSource A2Music;
    public AudioSource A3Music;

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
		if (currentScene >= 1) {
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
		SceneManager.LoadScene(0);
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (currentScene != scene.buildIndex) {
            if (scene.buildIndex == 0) {
				SwitchMusic(menuMusic, new List<AudioSource>() { A1Music, A2Music, A3Music });
            }
            else if (scene.buildIndex == 1) {
                SwitchMusic(A1Music, new List<AudioSource>() { menuMusic, A2Music, A3Music });
            }
            else if (scene.buildIndex == 2) {
                SwitchMusic(A2Music, new List<AudioSource>() { menuMusic, A1Music, A3Music });
            }
            else if (scene.buildIndex == 3) {
                SwitchMusic(A3Music, new List<AudioSource>() { menuMusic, A1Music, A2Music });
            }
            currentScene = scene.buildIndex;
        }
    }

	void SwitchMusic(AudioSource musicToPlay, List<AudioSource> musicListToStop) {
        if (!musicToPlay.isPlaying) {
            musicToPlay.Play();
        }
		foreach (AudioSource music in musicListToStop) {
			music.Stop();
		}
    }

    public void DeleteAllRecords() {
		for (int i = 1; i <= 3; i++) {
			PlayerPrefs.DeleteKey($"A{i}");
            PlayerPrefs.DeleteKey($"A{i}_medals");
            PlayerPrefs.DeleteKey($"A{i}_ghost");
        }
    }
}
