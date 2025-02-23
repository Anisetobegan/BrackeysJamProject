using UnityEngine;
using System.Collections.Generic;

public class FinishDish : PickableObject
{
    Dictionary<string, int> cookedIngredients = new Dictionary<string, int>();

    List<PickableObject> ingredients = new List<PickableObject>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void OnInteract()
    {
        Oven oven = GameManager.Instance.PlayerGet.InteractiveObject.GetComponent<Oven>();
        Window window = GameManager.Instance.PlayerGet.InteractiveObject.GetComponent<Window>();

        if (oven != null)
        {

        }

        if (window != null)
        {
            /*if (CheckCorrectIngredients())
            {
                window.OrderComplete();
            }*/
        }
    }

    public void PickUpIngredient(PickableObject ingredient)
    {
        ingredients.Add(ingredient);
    }

    /*public void GetFromDictionary(string name)
    {
        if (cookedIngredients.ContainsKey(name))
        {
            cookedIngredients[name]++;
            return;
        }

        cookedIngredients.Add(name, 1);
    }

    public void AddIngredient(PickableObject ingredient)
    {
        string ingredientName = ingredient.Info.Name;

        GetFromDictionary(ingredientName);
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
    }*/
}
