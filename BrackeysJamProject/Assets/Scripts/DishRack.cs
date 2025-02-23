using UnityEngine;

public class DishRack : InteractiveObject
{
    [SerializeField] FinishDish dishPrefab;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void OnInteract()
    {
        FinishDish newDish = Instantiate(dishPrefab, transform.position, transform.rotation);
        GameManager.Instance.PlayerGet.AddToStack(newDish);
        newDish.ObjectAnimation(GameManager.Instance.PlayerGet.transform.position, newDish.IsPickedUp);
    }
}
