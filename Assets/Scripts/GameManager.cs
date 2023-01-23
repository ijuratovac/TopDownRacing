using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager sharedInstance = null;

    public Button quitBtn;
    public Button startBtn;

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

    void Start() {
        quitBtn = GameObject.Find("Quit").GetComponent<Button>();
        quitBtn.onClick.AddListener(Quit);

        startBtn = GameObject.Find("Start").GetComponent<Button>();        
        startBtn.onClick.AddListener(LoadSampleScene);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (currentScene == 1) {
                LoadMainMenu();
            }
        }
    }

    void Quit() {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    void LoadSampleScene() {
        SceneManager.LoadScene(1);
        currentScene = 1;
    }

    void LoadMainMenu() {
        SceneManager.LoadScene(0);
        currentScene = 0;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == 0) {
            quitBtn = GameObject.Find("Quit").GetComponent<Button>();
            quitBtn.onClick.AddListener(Quit);

            startBtn = GameObject.Find("Start").GetComponent<Button>();
            startBtn.onClick.AddListener(LoadSampleScene);
        }
    }
}
