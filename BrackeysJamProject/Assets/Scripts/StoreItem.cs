using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    [SerializeField] float itemPrice;
    [SerializeField] float itemTotalPrice;
    [SerializeField] int itemQuantity = 1;

    [SerializeField] TextMeshProUGUI itemNameTMP;
    [SerializeField] TextMeshProUGUI itemPriceTMP;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemQuantityTMP;

    IngredientInfo storeItemInfo = null;

    void Start()
    {
        
    }

    private void Awake()
    {
        /*itemNameTMP.text = itemContainer.GetItemName();
        itemIcon = itemContainer.GetItemIcon();
        itemPriceTMP.text = itemTotalPrice.ToString();*/
    }

    void Update()
    {
        
    }

    public void BuyItem()
    {
        if (GameManager.Instance.PlayerGet.BuyItem(itemTotalPrice))
        {
            /*if (itemContainer.Refill(itemQuantity))
            {
                Debug.Log("Buy successful");
            }
            else
            {
                Debug.Log("You can´t carry anymore");
            }*/

            //itemContainer.Refill(itemQuantity, itemPrice);

            Actions.OnItemBought?.Invoke(storeItemInfo, itemQuantity);
        }
        else
        {
            Debug.Log("Not enough cash");
        }
    }

    public void IncreaseQuantity()
    {
        itemQuantity++;
        itemQuantityTMP.text = itemQuantity.ToString();

        itemTotalPrice = itemPrice * itemQuantity;
        itemPriceTMP.text = itemTotalPrice.ToString();
    }

    public void DecreaseQuantity()
    {
        if (itemQuantity > 1)
        {
            itemQuantity--;
            itemQuantityTMP.text = itemQuantity.ToString();

            itemTotalPrice = itemPrice * itemQuantity;
            itemPriceTMP.text = itemTotalPrice.ToString();
        }
    }

    public void InitializeStoreItem(IngredientInfo info)
    {
        storeItemInfo = info;

        itemPrice = storeItemInfo.Price;
        itemIcon = storeItemInfo.Icon;

        itemNameTMP.text = storeItemInfo.Name;
        itemPriceTMP.text = itemPrice.ToString();
        itemQuantityTMP.text = itemQuantity.ToString();

        itemTotalPrice = itemPrice;
    }
}
