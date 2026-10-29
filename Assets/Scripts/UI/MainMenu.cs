using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Easy()
    {
        DifficultyManager.Instance.Difficulty = Level.Easy;
        PlayGame();
    }
    
    public void Normal()
    {
        DifficultyManager.Instance.Difficulty = Level.Normal;
        PlayGame();
    }
    
    public void Hard()
    {
        DifficultyManager.Instance.Difficulty = Level.Hard;
        PlayGame();
    }
    
    public void Expert()
    {
        DifficultyManager.Instance.Difficulty = Level.Expert;
        PlayGame();
    }
    
    private void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
