using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShopItemUI : MonoBehaviour, IDeselectHandler
{
    [SerializeField] private string _itemName = "";
    [SerializeField] private int _itemCost = 100;
    public int ItemCost { get => _itemCost; }
    [SerializeField] private bool _special = false;
    public bool Special { get => _special; }
    [SerializeField] private Text _itemNameText;
    [SerializeField] private Text _itemCostText;

    private Button _button;
    public Action<ShopItemUI> OnButtonDeselect;
    public Action<ShopItemUI> OnButtonClicked;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ButtonClicked);
        _itemNameText.text = _itemName;
        _itemCostText.text = _itemCost.ToString() + " D";
    }

    public void OnDeselect(BaseEventData eventData)
    {
        OnButtonDeselect?.Invoke(this);
    }

    private void ButtonClicked()
    {
        OnButtonClicked?.Invoke(this);
    }
}