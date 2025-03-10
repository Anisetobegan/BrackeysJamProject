using UnityEngine;

public class Axe : PickableObject
{
    int durability;

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
        if (durability == 0)
        {

        }

        if (pickedUp)
        {
            transform.position = GameManager.Instance.PlayerGet.PickablePos.position;
            transform.rotation = GameManager.Instance.PlayerGet.PickablePos.rotation;
        }
    }

    public override void OnInteract()
    {
        if (!pickedUp /* && GameManager.Instance.Gamestate == Gamestate.EnemyChasing*/)
        {
            GameManager.Instance.PlayerGet.AddToStack(this);
            GameManager.Instance.PlayerGet.CanInteract();
            ObjectAnimation(GameManager.Instance.PlayerGet.PickablePos.position, pickedUp);
        }
        else
        {
            //Player Attacks
            Debug.Log("Attacked");
        }
    }

    public void BreakAxe()
    {
        Drop();
        GameManager.Instance.PlayerGet.RemoveFromStack();
        Destroy(gameObject);
        //Now you can buy an Axe again
    }
}
