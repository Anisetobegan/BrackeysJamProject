using DG.Tweening;
using UnityEngine;

public class PickableObject : MonoBehaviour, IInteractable
{
    protected bool stackable;
    protected bool pickedUp;
    protected bool isPrepped;
    protected int prepAmount;


    [SerializeField] protected PickableObject _preppedIngredientPrefab;

    protected Collider trigger = null;

    protected enum ObjectType
    {
        Ingredient,
        Object
    }
    protected ObjectType type;

    public bool IsStackable { get { return stackable; } }
    public bool IsPickedUp { get { return pickedUp; } }
    public int PrepAmount { get { return prepAmount; } }
    public PickableObject PreppedIngredientPrefab { get { return _preppedIngredientPrefab; } }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PickUp()
    {
        pickedUp = true;
        if (trigger != null)
        {            
            DisableTrigger();
        }        
    }

    public void Drop()
    {
        pickedUp = false;
        if (trigger != null)
        {            
            EnableTrigger();
        }
    }

    public void EnableTrigger()
    {
        trigger.enabled = true;
    }

    public void DisableTrigger()
    {
        trigger.enabled = false;
    }

    public void ObjectAnimation(Vector3 endPos, bool isPicked)
    {
        transform.DOMoveX(endPos.x, 0.2f).SetEase(Ease.Linear);
        transform.DOMoveY(endPos.y + 1f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
        transform.DOMoveZ(endPos.z, 0.2f).SetEase(Ease.Linear).OnComplete(() => 
        {
            if (!isPicked)
            {
                PickUp();
            }            
        });
    }

    public void IngredientPrepped()
    {
        prepAmount--;
    }

    public void DestroyIngredient()
    {
        Destroy(gameObject);
    }

    virtual public void OnInteract()
    {
        
    }
}
