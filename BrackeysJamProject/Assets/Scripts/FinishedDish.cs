using UnityEngine;
using System.Collections.Generic;

public class FinishedDish : PickableObject
{
    Dictionary<string, int> cookedIngredients = new Dictionary<string, int>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void OnInteract()
    {
        Window window = GameManager.Instance.PlayerGet.InteractiveObject.GetComponent<Window>();

        if (window != null)
        {
            if (CheckCorrectIngredients())
            {
                window.OrderComplete();
            }
        }
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
    }
}
