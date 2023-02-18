using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
				SwitchMusic(menuMusic, new List<AudioSource>() { A1Music, A2Music, A3Music });
            }
            else if (scene.name == "A1") {
                SwitchMusic(A1Music, new List<AudioSource>() { menuMusic, A2Music, A3Music });
            }
            else if (scene.name == "A2") {
                SwitchMusic(A2Music, new List<AudioSource>() { menuMusic, A1Music, A3Music });
            }
            else if (scene.name == "A3") {
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
}
