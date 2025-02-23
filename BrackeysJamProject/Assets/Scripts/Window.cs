using UnityEngine;

public class Window : InteractiveObject
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void OnInteract()
    {
        Debug.Log($"Interacted with: {this}");
    }

    public void OrderComplete()
    {
        OrderManager.Instance.CurrentOrders[0].DishCompleted();
    }
}
