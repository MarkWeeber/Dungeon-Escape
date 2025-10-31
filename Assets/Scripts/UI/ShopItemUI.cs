using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private Color _selectedColor = new Color(0.003525077f, 0.5849f, 0.13f, 1f);
    [SerializeField] private Color _deselectedColor = new Color(0f, 0.1058824f, 0.4431373f, 1f);
    [SerializeField] private string _itemName = "";
    public string ItemName { get => _itemName; }
    [SerializeField] private int _itemCost = 100;
    public int ItemCost { get => _itemCost; }
    [SerializeField] private Text _itemNameText;
    [SerializeField] private Text _itemCostText;

    private Image _image;
    private Button _button;
    public Action<ShopItemUI> OnButtonDeselect;
    public Action<ShopItemUI> OnButtonClicked;

    private void Start()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ButtonClicked);
        _itemNameText.text = _itemName;
        _itemCostText.text = _itemCost.ToString() + " D";
    }

    public void OnDeselect()
    {
        OnButtonDeselect?.Invoke(this);
        _image.color = _deselectedColor;
    }

    private void ButtonClicked()
    {
        OnButtonClicked?.Invoke(this);
        _image.color = _selectedColor;
    }

}