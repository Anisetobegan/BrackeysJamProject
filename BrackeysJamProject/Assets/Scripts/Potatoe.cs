using DG.Tweening;
using UnityEditor.Rendering;
using UnityEngine;

public class Potatoe : PickableObject
{

    private Vector3 stackOffset = Vector3.zero;

    virtual protected void Awake()
    {
        stackable = true;
        pickedUp = false;
        isPrepped = false;
        prepAmount = 5;

        type = ObjectType.Ingredient;

        stackOffset = CalculatePickedPositionOffset();

        ObjectAnimation(stackOffset + GameManager.Instance.PlayerGet.PickablePos.position, pickedUp);
        transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        /*transform.DOMoveX(playerPos.x, 0.2f).SetEase(Ease.Linear);
        transform.DOMoveY(playerPos.y + 1f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
        transform.DOMoveZ(playerPos.z, 0.2f).SetEase(Ease.Linear).OnComplete(() => PickUp());*/
    }

    protected void Update()
    {
        if (pickedUp)
        {
            transform.position = stackOffset + GameManager.Instance.PlayerGet.PickablePos.position;
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
            Potatoe newPotatoe = Instantiate(potatoeCrate.PotatoePrefab, potatoeCrate.transform.position, potatoeCrate.transform.rotation);
            GameManager.Instance.PlayerGet.AddToStack(newPotatoe);
        }

        if (oven != null)
        {
            oven.AddIngredient(this);
            GameManager.Instance.PlayerGet.RemoveFromStack();
        }
    }

    private Vector3 CalculatePickedPositionOffset() 
    {
        Vector3 pos = Vector3.zero;
        Vector2 circ = Random.insideUnitCircle * 0.5f;
        pos = new Vector3(circ.x, 0, circ.y) + GameManager.Instance.PlayerGet.PickablePos.position + (Vector3.up * 0.5f * Mathf.FloorToInt(GameManager.Instance.PlayerGet.PickablesAmount / 5));

        pos = pos - GameManager.Instance.PlayerGet.PickablePos.position;

        return pos;
    }
}
