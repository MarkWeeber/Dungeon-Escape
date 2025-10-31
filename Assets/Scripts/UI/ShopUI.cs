using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    private const string KEY_TO_CASTLE_ITEM_NAME = "Key To Castle";

    [SerializeField] private ShopItemUI _flameSwordButton;
    [SerializeField] private ShopItemUI _bootsOfFlightButton;
    [SerializeField] private ShopItemUI _keyToCastleButton;
    [SerializeField] private Button _buyButton;
    [SerializeField] private ShopItemSelectGroupUI _selectGroup;
    private ShopItemUI _currentChosenItem;
    private Player _player;
    private LogUI _logUI;
    private UIManager _uiManager;

    private void Start()
    {
        _player = Player.Instance;
        _logUI = LogUI.Instance;
        _uiManager = UIManager.Instance;
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
                if (_currentChosenItem.ItemName == KEY_TO_CASTLE_ITEM_NAME)
                {
                    GameManager.Instance.KeyToCastleAquired = true;
                }
                LogUI.Instance.SendLogInformation("Good choice! Come again to buy more stuff", LogUI.MessageType.SUCCESS);
                _player.Diamonds -= _currentChosenItem.ItemCost;
                _uiManager.CloseShopUI();
                _currentChosenItem = null;
                _selectGroup.ClearSelection();
            }
            else
            {
                LogUI.Instance.SendLogInformation("You don't have enogh to buy it, get lost!", LogUI.MessageType.WARNING);
                _uiManager.CloseShopUI();
                _currentChosenItem = null;
                _selectGroup.ClearSelection();
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
