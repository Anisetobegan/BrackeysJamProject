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

    void LateUpdate()
    {
        if (pickedUp)
        {
            transform.position = GameManager.Instance.PlayerGet.PickablePos.position;
            transform.rotation = GameManager.Instance.PlayerGet.PickablePos.rotation;
        }
    }

    public override void OnInteract()
    {
        if (!pickedUp)
        {
            GameManager.Instance.PlayerGet.AddToStack(this);
            //GameManager.Instance.PlayerGet.CanInteract();
            ObjectAnimation(GameManager.Instance.PlayerGet.PickablePos.position, pickedUp);
        }
        else
        {
            /*Table table = GameManager.Instance.PlayerGet.InteractiveObject.GetComponent<Table>();

            if (table != null)
            {
                table.PrepIngredient();
            }*/

            if (GameManager.Instance.PlayerGet.InteractiveObject.TryGetComponent(out Table table))
            {
                table.PrepIngredient();
            }
        }
    }
}
