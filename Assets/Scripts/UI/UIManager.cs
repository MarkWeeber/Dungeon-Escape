using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonBehaviour<UIManager>
{
    [SerializeField] private Transform _shopUI;
    [SerializeField] private Text _diamondsCountText;
    [SerializeField] private TMP_Text _diamondsCountTextHUD;
    [SerializeField] private Image _lifeBarImage;

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

    public void UpdateLifeBar(float rate)
    {
        _lifeBarImage.fillAmount = rate;
    }    
}
