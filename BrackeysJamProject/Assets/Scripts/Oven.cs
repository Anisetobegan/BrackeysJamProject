using System.Collections.Generic;
using UnityEngine;

public class Oven : InteractiveObject
{
    [SerializeField] List<PickableObject> preppedIngredients = new List<PickableObject>();
    FinishedDish finishedDish = null;

    float timer = 5f;
    bool isCooking = false;

    enum Doneness
    {
        Raw,
        Rare,
        Medium,
        WellDone,
        Burnt
    }
    Doneness doneness;

    void Start()
    {
        doneness = Doneness.Rare;
    }

    void Update()
    {
        if (isCooking)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if(timer <= 0)
            {
                timer = 5f;
                doneness++;
            }
        }
    }

    public override void OnInteract()
    {
        if (!isCooking)
        {
            StartCooking();
            return;
        }

        isCooking = false;
        PickFinishedDish();
    }

    public void AddIngredient(PickableObject pickable)
    {
        preppedIngredients.Add(pickable);
        pickable.ObjectAnimation(transform.position, pickable.IsPickedUp);
    }

    public void PickFinishedDish()
    {
        
        GameManager.Instance.PlayerGet.AddToStack(finishedDish);
        finishedDish.ObjectAnimation(GameManager.Instance.PlayerGet.transform.position, finishedDish.IsPickedUp);
        finishedDish = null;    
    }

    public void StartCooking()
    {
        isCooking = true;

        for (int i = 0; i < preppedIngredients.Count; i++)
        {
            finishedDish.AddIngredient(preppedIngredients[i]);
        }
    }
}
