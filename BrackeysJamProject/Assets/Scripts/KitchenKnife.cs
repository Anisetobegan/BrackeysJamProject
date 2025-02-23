using UnityEngine;

public class KitchenKnife : PickableObject
{    

    void Awake()
    {
        stackable = false;
        pickedUp = false;
        isPrepped = false;
        prepAmount = 0;

        trigger = GetComponent<Collider>();

        type = ObjectType.Object;
    }

    void Update()
    {
        if (pickedUp)
        {
            transform.position = GameManager.Instance.PlayerGet.transform.position;
        }
    }

    public override void OnInteract()
    {
        if (!pickedUp)
        {
            GameManager.Instance.PlayerGet.AddToStack(this);
            GameManager.Instance.PlayerGet.CanInteract();
            ObjectAnimation(GameManager.Instance.PlayerGet.transform.position, pickedUp);
        }
        else
        {
            Table table = GameManager.Instance.PlayerGet.InteractiveObject.GetComponent<Table>();

            if (table != null)
            {
                table.PrepIngredient();
            }
        }
    }
}
