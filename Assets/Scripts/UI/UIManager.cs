using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : SingletonBehaviour<UIManager>
{
    [Header("Gameplay HUD")]
    [SerializeField] private Text _diamondsCountText;
    [SerializeField] private TMP_Text _diamondsCountTextHUD;
    [SerializeField] private Image _lifeBarImage;
    [Header("Menu UI")]
    [SerializeField] private TMP_Text _menuText;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Transform _menuPanel;
    [SerializeField] private Transform _confirmPanel;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _goToMainMenuButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private TMP_Text _confirmPanelText;
    [Header("Shop UI")]
    [SerializeField] private Transform _shopUI;
    [SerializeField] private Button _showRewardedAdButton;
    [Header("On Player Death")]
    [SerializeField] private float _onPlayerDeathDelayToShowMenu = 2f;

    private void Start()
    {
        Time.timeScale = 1f;
        AssingButtonCallbacks();
    }

    private void AssingButtonCallbacks()
    {
        // assign button to show reward ads
        if (GoogleAdsManager.Instance != null)
        {
            GoogleAdsManager.Instance.AssignButtonToShowRewardedAd(_showRewardedAdButton);
        }
        _pauseButton.onClick.AddListener(PauseGame);
        _cancelButton.onClick.AddListener(() =>
        {
            _menuPanel.gameObject.SetActive(true);
            _confirmPanel.gameObject.SetActive(false);
        });
        _restartButton.onClick.AddListener(() => { ShowConfirmPanelWithAction(RestartGame, "RESTART GAME?"); });
        _goToMainMenuButton.onClick.AddListener(() => { ShowConfirmPanelWithAction(GoToMainMenu, "GO TO MAIN MENU?"); });
        _resumeButton.onClick.AddListener(ResumeGame);
    }

    #region main menu and confirm panel
    private void PauseGame()
    {
        Time.timeScale = 0.0f;
        _pauseButton.interactable = false;
        _menuPanel.gameObject.SetActive(true);
        _confirmPanel.gameObject.SetActive(false);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1.0f;
        _pauseButton.interactable = true;
        _menuPanel.gameObject.SetActive(false);
        _confirmPanel.gameObject.SetActive(false);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void ShowConfirmPanelWithAction(UnityAction action, string text)
    {
        _menuPanel.gameObject.SetActive(false);
        _confirmPanel.gameObject.SetActive(true);
        _confirmButton.onClick.RemoveAllListeners();
        _confirmButton.onClick.AddListener(action);
        _confirmPanelText.text = text;
    }
    #endregion

    #region shop ui
    public void OpenShopUI()
    {
        _shopUI.gameObject.SetActive(true);
        LogUI.Instance.SendLogInformation("Greetings at my shop Stranger!", LogUI.MessageType.SUCCESS);
    }

    public void CloseShopUI()
    {
        _shopUI.gameObject.SetActive(false);
    }

    public void UpdateDiamondsCount(int diamonds)
    {
        _diamondsCountText.text = diamonds.ToString() + " D";
        _diamondsCountTextHUD.text = diamonds.ToString();
    }
    #endregion

    #region gameplay hud
    public void UpdateLifeBar(float rate)
    {
        _lifeBarImage.fillAmount = rate;
    }
    #endregion

    public void OnPlayerDeath()
    {
        Invoke(nameof(CallGameOver), _onPlayerDeathDelayToShowMenu);
        _pauseButton.gameObject.SetActive(false);
        _resumeButton.gameObject.SetActive(false);
        _menuText.text = "GAME OVER";
    }

    private void CallGameOver()
    {
        Time.timeScale = 0f;
        _menuPanel.gameObject.SetActive(true);
    }
}
