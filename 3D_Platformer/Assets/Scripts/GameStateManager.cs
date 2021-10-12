using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameStateManager : MonoBehaviour {
    // Script to handle Game State    
    private GameObject[] pausedObjects;
    private GameObject gameOverText;

    private bool victory;
    public bool Victory {
        get {
            return victory;
        } set {
            victory = value;
        }
    }

    private bool gameOver;
    public bool GameOver {
        get {
            return gameOver;
        }
        set {
            gameOver = value;
            if (gameOver) {
                if (victory) {
                    gameOverText.GetComponent<TextMeshProUGUI>().text = "You Won!";
                }
                Time.timeScale = 0;
                ShowPaused();
            }
        }
    }

    private void Awake() {
        pausedObjects = GameObject.FindGameObjectsWithTag("Paused");
        gameOverText = GameObject.Find("GameOverText");
        GameOver = false;
        Victory = false;
        HidePaused();
    }

    public void ShowPaused() {
        foreach(GameObject paused in pausedObjects) {
            paused.SetActive(true);
        }
    }

    public void HidePaused() {
        foreach (GameObject paused in pausedObjects) {
            paused.SetActive(false);
        }
    }

    public void Restart() {
        Time.timeScale = 1;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
