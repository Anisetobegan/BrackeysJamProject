using UnityEngine;

public class IngredientContainer : InteractiveObject
{
    [SerializeField] PickableObject _ingredientPrefab;

    public PickableObject IngredientPrefab { get { return _ingredientPrefab; } }

    void Start()
    {
        itemMaxCapacity = 25;
        itemQuantity = itemMaxCapacity;
    }

    void Update()
    {

    }

    private void OnEnable()
    {
        Actions.OnItemBought += Refill;
    }

    private void OnDisable()
    {
        Actions.OnItemBought -= Refill;
    }

    public override void OnInteract()
    {
        if (UseItem())
        {
            PickableObject newIngredient = Instantiate(_ingredientPrefab, transform.position, transform.rotation);
            GameManager.Instance.PlayerGet.AddToStack(newIngredient);
        }

        /*Vector3 playerPos = GameManager.Instance.PlayerGet.transform.position;

        newPotatoe.transform.DOMoveX(playerPos.x, 0.2f).SetEase(Ease.Linear);
        newPotatoe.transform.DOMoveY(playerPos.y + 1f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
        newPotatoe.transform.DOMoveZ(playerPos.z, 0.2f).SetEase(Ease.Linear).OnComplete(() => newPotatoe.PickUp());*/
    }

    public bool UseItem()
    {
        if (itemQuantity > 0)
        {
            itemQuantity--;
            return true;
        }

        Debug.Log("Empty");
        return false;
    }

    public override string GetItemName()
    {
        return $"{_ingredientPrefab.Info.Name}s";
    }

    public override Sprite GetItemIcon()
    {
        return _ingredientPrefab.Info.Icon;
    }

    /*public override void Refill(int quantity, float price)
    {
        /*if (itemQuantity < itemMaxCapacity)
        {
            itemQuantity = itemMaxCapacity;
            return true;
        }
        return false;/*

        /*if (itemQuantity + quantity <= itemMaxCapacity)
        {
            itemQuantity += quantity;
            return true;
        }
        return false;/*

        int refund = (itemQuantity + quantity) - itemMaxCapacity;
        itemQuantity = Mathf.Clamp(itemQuantity + quantity, 0, itemMaxCapacity);
        
        if (refund > 0)
        {
            Actions.OnItemRefund?.Invoke(refund, price);
        }
    }*/

    public override void Refill(IngredientInfo info, int quantity)
    {
        if (info == _ingredientPrefab.Info)
        {
            int refund = (itemQuantity + quantity) - itemMaxCapacity;
            itemQuantity = Mathf.Clamp(itemQuantity + quantity, 0, itemMaxCapacity);

            if (refund > 0)
            {
                Actions.OnItemRefund?.Invoke(refund, _ingredientPrefab.Info.Price);
            }
        }
    }
}
