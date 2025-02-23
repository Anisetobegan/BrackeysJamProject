using DG.Tweening;
using UnityEditor.Rendering;
using UnityEngine;

public class Potatoe : PickableObject
{

    virtual protected void Awake()
    {
        stackable = true;
        pickedUp = false;
        isPrepped = false;
        prepAmount = 5;

        type = ObjectType.Ingredient;
        
        Vector3 playerPos = GameManager.Instance.PlayerGet.transform.position;

        ObjectAnimation(playerPos, pickedUp);

        /*transform.DOMoveX(playerPos.x, 0.2f).SetEase(Ease.Linear);
        transform.DOMoveY(playerPos.y + 1f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
        transform.DOMoveZ(playerPos.z, 0.2f).SetEase(Ease.Linear).OnComplete(() => PickUp());*/
    }

    protected void Update()
    {
        if (pickedUp)
        {
            transform.position = GameManager.Instance.PlayerGet.transform.position;
        }
    }

    public override void OnInteract()
    {
        Table table = GameManager.Instance.PlayerGet.InteractiveObject.GetComponent<Table>();
        PotatoesCrate potatoeCrate = GameManager.Instance.PlayerGet.InteractiveObject.GetComponent<PotatoesCrate>();
        Oven oven = GameManager.Instance.PlayerGet.InteractiveObject.GetComponentInParent<Oven>();

        if (table != null)
        {
            table.AddIngredient(this);
            GameManager.Instance.PlayerGet.RemoveFromStack();
        }

        if (potatoeCrate != null)
        {
            Potatoe newPotatoe = Instantiate(this, potatoeCrate.transform.position, potatoeCrate.transform.rotation);
            GameManager.Instance.PlayerGet.AddToStack(newPotatoe);
        }

        if (oven != null)
        {
            oven.AddIngredient(this);
            GameManager.Instance.PlayerGet.RemoveFromStack();
        }
    }
}
