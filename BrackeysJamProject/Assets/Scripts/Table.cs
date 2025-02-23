using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Rendering;

public class Table : InteractiveObject
{
    [SerializeField] List<PickableObject> ingredientsToPrep = new List<PickableObject>();
    [SerializeField] List<PickableObject> preppedIngredients = new List<PickableObject>();

    [SerializeField] Transform pickablePos;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void OnInteract()
    {
        if (preppedIngredients.Count > 0 && ingredientsToPrep.Count == 0)
        {
            PickPreppedIngredient();
        }
        /*PickableObject newObject = GameManager.Instance.PlayerGet.PutDownIngredient();

        if (newObject != null)
        {
            ingredientsToPrep.Add(newObject);
            newObject.ObjectAnimation(transform.position, newObject.IsPickedUp);
        }
        else
        {
            if (preppedIngredients.Count > 0) 
            {
                List<PickableObject> reversedList = preppedIngredients;
                reversedList.Reverse();
                newObject = reversedList[0];
                preppedIngredients.Remove(newObject);
                GameManager.Instance.PlayerGet.AddToStack(newObject);
                newObject.ObjectAnimation(GameManager.Instance.PlayerGet.transform.position, newObject.IsPickedUp);
            }
        }*/
    }

    public void AddIngredient(PickableObject pickable)
    {
        ingredientsToPrep.Add(pickable);
        pickable.ObjectAnimation(CalculatePickedPositionOffset() + pickablePos.position, pickable.IsPickedUp);
    }

    public void PrepIngredient()
    {
        if (ingredientsToPrep.Count > 0)
        {
            PickableObject ingredientToPrep = null;
            List<PickableObject> reversedList = new List<PickableObject>(ingredientsToPrep);
            reversedList.Reverse();
            ingredientToPrep = reversedList[0];
            ingredientToPrep.IngredientPrepped();            

            PickableObject preppedIngredient = Instantiate(ingredientToPrep.PreppedIngredientPrefab, transform.position, transform.rotation);
            preppedIngredients.Add(preppedIngredient);

            if (ingredientToPrep.PrepAmount == 0)
            {
                ingredientsToPrep.Remove(ingredientToPrep);
                ingredientToPrep.DestroyIngredient();
            }
        }
    }

    public void PickPreppedIngredient()
    {
        PickableObject ingredientToPick = null;
        List<PickableObject> reversedList = new List<PickableObject>(preppedIngredients);
        reversedList.Reverse();
        ingredientToPick = reversedList[0];
        preppedIngredients.Remove(ingredientToPick);
        GameManager.Instance.PlayerGet.AddToStack(ingredientToPick);
        ingredientToPick.ObjectAnimation(GameManager.Instance.PlayerGet.PickablePos.position, ingredientToPick.IsPickedUp);
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
