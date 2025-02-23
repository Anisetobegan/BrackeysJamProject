using System.Collections.Generic;
using UnityEngine;

public class Oven : InteractiveObject
{
    List<PickableObject> preppedIngredients = new List<PickableObject>();
    PickableObject finishedDish = null;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void OnInteract()
    {
        if (finishedDish != null && preppedIngredients.Count == 0)
        {
            PickFinishedDish();
        }
    }

    public void PickFinishedDish()
    {
        if (finishedDish != null)
        {
            GameManager.Instance.PlayerGet.AddToStack(finishedDish);
            finishedDish.ObjectAnimation(GameManager.Instance.PlayerGet.transform.position, finishedDish.IsPickedUp);
            finishedDish = null;
        }
    }
}
