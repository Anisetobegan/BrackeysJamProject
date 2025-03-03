using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    [SerializeField] InteractiveObject itemContainer;
    [SerializeField] float itemPrice;

    [SerializeField] TextMeshProUGUI itemNameTMP;
    [SerializeField] TextMeshProUGUI itemPriceTMP;
    [SerializeField] Image itemIcon;

    void Start()
    {
        
    }

    private void Awake()
    {
        itemNameTMP.text = itemContainer.GetItemName();
        itemIcon = itemContainer.GetItemIcon();
        itemPriceTMP.text = itemPrice.ToString();
    }

    void Update()
    {
        
    }

    public void BuyItem()
    {
        if (GameManager.Instance.PlayerGet.BuyItem(itemPrice))
        {
            itemContainer.Refill();
        }
        else
        {
            Debug.Log("Not enough cash");
        }
    }
}
