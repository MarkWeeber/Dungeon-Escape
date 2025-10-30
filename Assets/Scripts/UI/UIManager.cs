using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonBehaviour<UIManager>
{
    [SerializeField] private Transform _shopUI;
    [SerializeField] private Text _diamondsCountText;

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
    }
}
