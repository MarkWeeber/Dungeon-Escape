using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItemSelectGroupUI : MonoBehaviour
{
    [SerializeField] private ShopItemUI[] shopItems;

    private UnityAction<Button> x;

    private void Start()
    {
        InitButtonCallbacks();
    }

    private void InitButtonCallbacks()
    {
        foreach (ShopItemUI item in shopItems)
        {
            item.OnButtonClicked += OnClickChecker;
        }
    }

    private void OnClickChecker(ShopItemUI shopItem)
    {
        foreach (ShopItemUI item in shopItems)
        {
            if (shopItem == item) continue;
            item.OnDeselect();
        }
    }

    public void ClearSelection()
    {
        foreach (ShopItemUI item in shopItems)
        {
            item.OnDeselect();
        }
    }
}
