using UnityEngine;
using UnityEngine.UI;

public class InteractiveObject : MonoBehaviour, IInteractable
{
    [SerializeField] protected int itemQuantity;
    [SerializeField] protected int itemMaxCapacity;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    virtual public void OnInteract()
    {
        
    }

    virtual public void OnTriggerLeave()
    {

    }

    virtual public void Refill(int quantity, float price)
    {
        
    }

    virtual public void Refill(IngredientInfo info, int quantity)
    {

    }

    virtual public string GetItemName()
    {
        return "";
    }

    virtual public Sprite GetItemIcon()
    {
        return null;
    }
}
