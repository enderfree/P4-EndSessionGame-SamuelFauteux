using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingPannel;

    // unity
    void Awake()
    {
        // I like for my stuff to crash when there is a gamebreaking error
        settingPannel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // main menu buttons
    public void StartClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void SettingsClicked()
    {
        mainMenuPanel.SetActive(false);
        settingPannel.SetActive(true);
    }

    public void QuitClicked()
    {
        Application.Quit();
    }

    // settings
    public void BackClicked()
    {
        settingPannel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
