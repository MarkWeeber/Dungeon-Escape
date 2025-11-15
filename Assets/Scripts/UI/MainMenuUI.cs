using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _quitGameButton;

    private void Start()
    {
        _startGameButton.onClick.AddListener(StartGame);
        _quitGameButton.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
