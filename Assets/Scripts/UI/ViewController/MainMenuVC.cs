using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuVC : ViewController
{
    [SerializeField] private NavigationController singleplayerNC;
    [SerializeField] private NavigationController multiplayerNC;
    [SerializeField] private NavigationController customizationNC;
    [SerializeField] private TabbarController settingsPanel;

    public void singleplayerPressed()
    {

    }

    public void multiplayerPressed()
    {
        navigationController.push(multiplayerNC);
    }

    public void customizationPressed()
    {
        navigationController.push(customizationNC);
    }

    public void settingsPressed()
    {
        navigationController.push(settingsPanel);
    }

    public void logoutPressed()
    {
        FindObjectOfType<AudioHandler>().stopBgAudio();
        User.currentUser().reset();
        FindObjectOfType<SocketHandler>().Close();
        SceneManager.LoadSceneAsync("Credentials");
    }

    public void quitPressed()
    {
        Application.Quit();
    }
}
