using System.Collections.Generic;
using UnityEngine;

public class Oven : InteractiveObject
{
    [SerializeField] List<PickableObject> preppedIngredients = new List<PickableObject>();
    FinishDish finishedDish = null;

    float timer = 5f;
    bool isCooking = false;

    public bool IsCooking {  get { return isCooking; } }

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
        /*if (!isCooking)
        {
            StartCooking();
            return;
        }

        isCooking = false;*/
        //PickUpFinishedDish();
    }

    public void AddIngredient(PickableObject pickable)
    {
        preppedIngredients.Add(pickable);
        pickable.ObjectAnimation(transform.position, pickable.IsPickedUp);
    }

    public List<PickableObject> PickCookedIngredients()
    {
        List<PickableObject> cookedIngredients = new List<PickableObject>();

        for (int i = 0; i < preppedIngredients.Count; i++)
        {
            PickableObject ingredientToPick = preppedIngredients[i];
            cookedIngredients.Add(ingredientToPick);
            ingredientToPick.ObjectAnimation(GameManager.Instance.PlayerGet.transform.position, ingredientToPick.IsPickedUp);
        }
        preppedIngredients.Clear();
        isCooking = false;
        timer = 5f;
        Debug.Log(doneness);
        doneness = Doneness.Raw;
        return cookedIngredients;
    }

    public void StartCooking()
    {
        if (preppedIngredients.Count > 0)
        {
            isCooking = true;
            Debug.Log("Started cooking");
        }
    }

    public override void OnTriggerLeave()
    {
        StartCooking();
    }
}
