using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    private static DifficultyManager instance = null;
    public static DifficultyManager Instance => instance;
    
    public Level Difficulty { get; set; }
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
