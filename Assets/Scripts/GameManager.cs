using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string nextSceneName;
    [SerializeField]
    private string
    mainMenuSceneName = "MainMenu";
    [SerializeField]
    private string
    resetSceneName = "ResetScene";
    [SerializeField]
    private string
    gameplaySceneName = "Digsite";
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(resetSceneName);
        nextSceneName = gameplaySceneName;
    }
    public void ReturnToMenu()
    {
        // Reset totals before leaving gameplay
        if (CollectibleManager.Instance != null)
        {
            CollectibleManager.Instance.ResetCollectibles();
        }
        SceneManager.LoadScene("ResetScene");
        nextSceneName = "MainMenu";
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}


