using DG.Tweening;
using UnityEngine;

public class PickableObject : MonoBehaviour, IInteractable
{
    protected bool stackable;
    protected bool pickedUp;
    protected bool isPrepped;
    protected int prepAmount;
    
    [SerializeField] protected IngredientInfo _ingredientInfo;
    
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
    public IngredientInfo Info {  get { return _ingredientInfo; } }

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
        transform.DOJump(endPos, 2, 1, 0.2f).OnComplete(() =>
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
