using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager sharedInstance = null;

    public Button quitBtn;
    public Button startBtn;

    int currentScene = 0;

    LevelLoader levelLoader;

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

    void Quit() {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    void ReloadScene() {
        SceneManager.LoadScene(currentScene);
    }

    void LoadMainMenu() {
        SceneManager.LoadScene(0);
        currentScene = 0;
    }

    void LoadSampleScene() {
        SceneManager.LoadScene(1);
        currentScene = 1;
    }

    void LoadA1() {
        levelLoader.LoadLevel(2);
        //SceneManager.LoadScene(2);
        currentScene = 2;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == 0) {
            //quitBtn = GameObject.Find("Quit").GetComponent<Button>();
            quitBtn.onClick.AddListener(Quit);

            //startBtn = GameObject.Find("Start").GetComponent<Button>();
            startBtn.onClick.AddListener(LoadA1);

            levelLoader = GetComponent<LevelLoader>();
        }
    }
}
