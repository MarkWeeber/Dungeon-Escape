using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private ShopItemUI _flameSwordButton;
    [SerializeField] private ShopItemUI _bootsOfFlightButton;
    [SerializeField] private ShopItemUI _keyToCastleButton;
    [SerializeField] private Button _buyButton;
    private ShopItemUI _currentChosenItem;
    private Player _player;

    private void Start()
    {
        _player = Player.Instance;
        _flameSwordButton.OnButtonClicked += ButtonClicked;
        _bootsOfFlightButton.OnButtonClicked += ButtonClicked;
        _keyToCastleButton.OnButtonClicked += ButtonClicked;
        _flameSwordButton.OnButtonDeselect += ButtonDesselected;
        _bootsOfFlightButton.OnButtonDeselect += ButtonDesselected;
        _keyToCastleButton.OnButtonDeselect += ButtonDesselected;
        _buyButton.onClick.AddListener(PerformShopping);
    }

    private void OnDestroy()
    {
        _flameSwordButton.OnButtonClicked -= ButtonClicked;
        _bootsOfFlightButton.OnButtonClicked -= ButtonClicked;
        _keyToCastleButton.OnButtonClicked -= ButtonClicked;
        _flameSwordButton.OnButtonDeselect -= ButtonDesselected;
        _bootsOfFlightButton.OnButtonDeselect -= ButtonDesselected;
        _keyToCastleButton.OnButtonDeselect -= ButtonDesselected;
    }

    private void PerformShopping()
    {
        if (_currentChosenItem == null)
        {
            LogUI.Instance.SendLogInformation("Choose Item First", LogUI.MessageType.WARNING);
        }
        else
        {
            if (_player.Diamonds >= _currentChosenItem.ItemCost)
            {
                LogUI.Instance.SendLogInformation("Good choice", LogUI.MessageType.SUCCESS);
                _player.Diamonds -= _currentChosenItem.ItemCost;
            }
            else
            {
                LogUI.Instance.SendLogInformation("You don't have enogh to buy it, get lost!", LogUI.MessageType.WARNING);
            }
        }
    }

    private void ButtonClicked(ShopItemUI shopItem)
    {
        _currentChosenItem = shopItem;
    }

    private void ButtonDesselected(ShopItemUI shopItem)
    {
        if (_currentChosenItem == shopItem)
        {
            _currentChosenItem = null;
        }
    }
}
