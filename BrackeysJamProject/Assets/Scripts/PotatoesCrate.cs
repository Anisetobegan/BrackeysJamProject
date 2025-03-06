using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PotatoesCrate : InteractiveObject
{
    [SerializeField] Potatoe _potatoePrefab;

    public Potatoe PotatoePrefab {  get { return _potatoePrefab; } }

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
            Potatoe newPotatoe = Instantiate(_potatoePrefab, transform.position, transform.rotation);
            GameManager.Instance.PlayerGet.AddToStack(newPotatoe);
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
        return $"{_potatoePrefab.Info.Name}s";
    }

    public override Image GetItemIcon()
    {
        return _potatoePrefab.Info.Icon;
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
        if (info == _potatoePrefab.Info)
        {
            int refund = (itemQuantity + quantity) - itemMaxCapacity;
            itemQuantity = Mathf.Clamp(itemQuantity + quantity, 0, itemMaxCapacity);

            if (refund > 0)
            {
                Actions.OnItemRefund?.Invoke(refund, _potatoePrefab.Info.Price);
            }
        }
    }
}
