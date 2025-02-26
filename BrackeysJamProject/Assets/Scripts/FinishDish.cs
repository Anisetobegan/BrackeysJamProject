using UnityEngine;
using System.Collections.Generic;

public class FinishDish : PickableObject
{
    Dictionary<string, int> cookedIngredients = new Dictionary<string, int>();

    [SerializeField] List<PickableObject> ingredients = new List<PickableObject>();

    void Awake()
    {
        stackable = false;
        pickedUp = false;
        isPrepped = false;
        prepAmount = 0;

        trigger = GetComponent<Collider>();
        trigger.enabled = false;

        type = ObjectType.Object;
    }

    void Update()
    {
        if (pickedUp)
        {
            transform.position = GameManager.Instance.PlayerGet.PickablePos.position;
            transform.rotation = GameManager.Instance.PlayerGet.PickablePos.rotation * Quaternion.Euler(0, -90, -30);
        }
    }

    public override void OnInteract()
    {
        if (!pickedUp)
        {
            GameManager.Instance.PlayerGet.AddToStack(this);
            GameManager.Instance.PlayerGet.CanInteract();
            ObjectAnimation(GameManager.Instance.PlayerGet.transform.position, pickedUp);
        }
        else
        {
            PotatoesCrate potatoeCrate = GameManager.Instance.PlayerGet.InteractiveObject.GetComponent<PotatoesCrate>();
            Table table = GameManager.Instance.PlayerGet.InteractiveObject.GetComponent<Table>();
            Oven oven = GameManager.Instance.PlayerGet.InteractiveObject.GetComponent<Oven>();
            Window window = GameManager.Instance.PlayerGet.InteractiveObject.GetComponent<Window>();

            if (potatoeCrate != null)
            {
                Potatoe newPotatoe = Instantiate(potatoeCrate.PotatoePrefab, potatoeCrate.transform.position, potatoeCrate.transform.rotation);
                ingredients.Add(newPotatoe);
            }

            if (table != null)
            {
                PickableObject ingredientToPick = null;
                ingredientToPick = table.PickPreppedIngredient();
                if (ingredientToPick != null)
                {
                    ingredients.Add(ingredientToPick);
                }
            }

            if (oven != null)
            {
                if (oven.IsCooking)
                {
                    ingredients = oven.PickCookedIngredients();
                    oven.FinishCooking();
                    AddIngredientsToDictionary();
                }
                else
                {
                    if (ingredients.Count > 0)
                    {
                        PickableObject ingredientToAdd = null;
                        List<PickableObject> reverseList = new List<PickableObject>(ingredients);
                        reverseList.Reverse();
                        ingredientToAdd = reverseList[0];
                        ingredients.Remove(ingredientToAdd);

                        oven.AddIngredient(ingredientToAdd);
                        ingredientToAdd.Drop();

                        //GameManager.Instance.PlayerGet.RemoveFromStack();
                    }
                }
            }

            if (window != null)
            {
                if (CheckCorrectIngredients())
                {
                    window.OrderComplete();
                }
            }
        }
    }

    public void PickUpIngredient(PickableObject ingredient)
    {
        ingredients.Add(ingredient);
    }

    public void GetFromDictionary(string name)
    {
        if (cookedIngredients.ContainsKey(name))
        {
            cookedIngredients[name]++;
            return;
        }

        cookedIngredients.Add(name, 1);
    }

    public void AddIngredientsToDictionary()
    {
        foreach (var ingredient in ingredients)
        {
            string ingredientName = ingredient.Info.Name;
            GetFromDictionary(ingredientName);
        }
    }

    public bool CheckCorrectIngredients()
    {
        List<Ingredient> orderIngredients = new List<Ingredient>(OrderManager.Instance.CurrentOrders[0].Ingredients);

        int correctIngredients = 0;

        foreach (var ingredient  in orderIngredients)
        {
            if (cookedIngredients.ContainsKey(ingredient.Info.Name))
            {
                if (ingredient.Quantity == cookedIngredients[ingredient.Info.Name])
                {
                    correctIngredients++;
                }
            }
        }

        if (correctIngredients == orderIngredients.Count)
        {
            return true;
        }

        return false;
    }
}
