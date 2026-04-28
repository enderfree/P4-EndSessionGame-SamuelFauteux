using UnityEngine;

public class DeathScreenManager : MonoBehaviour
{
    public void RetryClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void MainMenuClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
