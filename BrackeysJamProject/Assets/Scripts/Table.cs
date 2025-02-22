using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Table : InteractiveObject
{
    List<PickableObject> ingredientsToPrep = new List<PickableObject>();
    List<PickableObject> preppedIngredients = new List<PickableObject>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void OnInteract()
    {
        PickableObject newObject = GameManager.Instance.PlayerGet.PutDownIngredient();

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
        }
    }
}
