using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager sharedInstance = null;

    int currentScene = 0;

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
        currentScene = 0;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        currentScene = scene.buildIndex;
    }
}
