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

    virtual public bool Refill()
    {
        return false;
    }

    virtual public string GetItemName()
    {
        return "";
    }

    virtual public Image GetItemIcon()
    {
        return null;
    }
}
