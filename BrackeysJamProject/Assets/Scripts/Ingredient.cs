using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Ingredient
{
    IngredientInfo info = null;
    
    int minQuantityRange;
    int maxQuantityRange;
    int quantity;

    public Ingredient(string ingredientName)
    {
        minQuantityRange = 5;
        maxQuantityRange = 15;
        RandomizeQuantity();
        GetIngredientInfo(ingredientName);
    }

    public IngredientInfo Info {  get { return info; } }
    public int Quantity { get { return quantity; } }

    public void RandomizeQuantity()
    {
        quantity = Random.Range(minQuantityRange, maxQuantityRange + 1);
    }

    public void GetIngredientInfo(string ingredientName)
    {
        //Check IngredientInfo scriptable objects list for an ingredient of the same name
        List<IngredientInfo> infoList = OrderManager.Instance.InfoList;

        for (int i = 0; i < infoList.Count; i++)
        {
            if (infoList[i].name == ingredientName)
            {
                //If it exists, add the IngredientInfo found to info variable
                info = infoList[i];
                break;
            }
        }
        if (info == null)
        {
            Debug.Log("Ingredient not found");
        }
    }
}
