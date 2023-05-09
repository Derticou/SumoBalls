using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Button startGame;
    public Button restartGame;
    
    public TextMeshProUGUI fallenBallsValueText;
    public TextMeshProUGUI waveValueText;
    public TextMeshProUGUI powerupInfo;

    public GameObject startScreen;
    public GameObject pauseScreen;
    public GameObject gameoverScreen;

    float fallenBallsValue;
    float waveValue;

    public bool isGameActive, isGamePause;
    
    // Start is called before the first frame update
    void Start()
    {
        isGameActive = false;
        isGamePause = false;

        fallenBallsValueText.text = "";

        waveValueText.text = "";
        powerupInfo.text = "";

        startScreen.SetActive(true);
        gameoverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }
    private void Update()
    {
        //Pause Game
        if (isGameActive && Input.GetKeyDown(KeyCode.P) && !startScreen.activeInHierarchy) 
        {
            pauseScreen.SetActive(true);
            isGameActive = false;
            Time.timeScale = 0;
        }

        //Unpause Game
        else if (!isGameActive && Input.GetKeyDown(KeyCode.P) && !startScreen.activeInHierarchy)
        {
            pauseScreen.SetActive(false);
            isGameActive = true;
            Time.timeScale = 1;
        }
    }
    public void StartGame()
    {
        isGameActive = true;
        startScreen.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver(bool gameOver)
    {
        if (gameOver)
        {
            isGameActive = false;
            gameoverScreen.SetActive(true);
        }
    }

    public void FallenBallCounter(float newFallen)
    {
        fallenBallsValue += newFallen;
        fallenBallsValueText.text= fallenBallsValue.ToString();
    }

    public void WaveCounter(float newWave)
    {
        waveValue += newWave;
        waveValueText.text= waveValue.ToString();
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
