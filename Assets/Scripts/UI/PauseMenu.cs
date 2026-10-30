using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused;
    
    [SerializeField] private GameObject pauseMenu;
    
    private InputAction _pause;
    private CursorLockMode _cursorLockMode;

    private void Awake()
    {
        _pause = InputSystem.actions.FindAction("Pause");
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    private void Update()
    {
        if (_pause.triggered)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = _cursorLockMode;
        GameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _cursorLockMode = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
